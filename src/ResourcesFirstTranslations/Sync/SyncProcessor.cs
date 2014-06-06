using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Resx;
using ResourcesFirstTranslations.Services;

namespace ResourcesFirstTranslations.Sync
{
    public class SyncProcessor
    {
        public SyncProcessor(IResxLoader resourceFileLoader)
        {
            _resourceFileLoader = resourceFileLoader;
            _resourceReader = new ResxStringResourceReader();

            GetContext = () => new RftContext();
        }

        public SyncProcessor(IResxLoader resourceFileLoader, RftContext ctx)
        {
            _resourceFileLoader = resourceFileLoader;
            _resourceReader = new ResxStringResourceReader();

            GetContext = () => ctx;
        }

        private Func<RftContext> GetContext { get; set; }
        private readonly IResxLoader _resourceFileLoader;
        private readonly ResxStringResourceReader _resourceReader;
        private List<Branch> _branches;
        private List<BranchResourceFile> _resourceFiles;
        private List<Language> _languages;

        // This is for external callers only, internal callers use the private backing fields for performance
        public List<Branch> Branches { get { return _branches; } }
        public List<BranchResourceFile> BranchResourceFiles { get { return _resourceFiles; } }
        public List<Language> Languages { get { return _languages; } }

        public void LoadConfiguration()
        {
            using (var ctx = GetContext())
            {
                _branches = ctx.Branches/*.AsNoTracking()*/.ToList(); // AsNoTracking crashes the unit tests
                _resourceFiles = ctx.BranchResourceFiles.ToList();
                _languages = ctx.Languages.ToList();
            }
        }

        public bool Run(bool stopOnFirstError)
        {
            Trace.TraceInformation("Running for {0} branches total", _branches.Count);

            foreach (var branch in _branches)
            {
                int branchId = branch.Id;

                var branchResourceFiles = _resourceFiles
                        .Where(brf => brf.FK_BranchId == branchId)
                        .ToList();

                if (0 == branchResourceFiles.Count)
                {
                    Trace.TraceWarning("Branch {0} has no branch resource files defined", branchId);
                }

                foreach (var file in branchResourceFiles)
                {
                    Trace.TraceInformation("Start processing branch {0}, branch resource file {1}", branchId, file.Id);

                    bool fileResult = ProcessResourceFile(branch, file);

                    Trace.TraceInformation("Finished processing branch {0}, branch resource file {1}, process result: {2}", branchId, file.Id, fileResult);

                    if (stopOnFirstError && !fileResult)
                    {
                        Trace.TraceInformation("Stopped processing on first error");
                        return false;
                    }
                }
            }

            return true;
        }

        public bool ProcessResourceFile(Branch branch, BranchResourceFile file)
        {
            int branchId = file.FK_BranchId;
            int resourceFileId = file.FK_ResourceFileId;

            string resxContent = _resourceFileLoader.GetAsString(file.SyncRawPathAbsolute, suppressExceptions: true);

            if (null != resxContent)
            {
                var stringResources = _resourceReader.ResxToResourceStringDictionary(new StringReader(resxContent));

                if (null != stringResources)
                {
                    try
                    {
                        return ProcessResourceFile(branchId, resourceFileId, stringResources);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.ToString());
                    }
                }
            }

            return false;
        }

        public bool ProcessResourceFile(int branchId, int resourceFileId, List<ResxStringResource> stringResources)
        {
            using (var ctx = GetContext())  // Context is isolated per branch resource file
            {
                // Get all already in-db string resources in one call
                var dbStringResources = ctx.ResourceStrings
                    .Where(rs => rs.FK_BranchId == branchId && rs.FK_ResourceFileId == resourceFileId)
                    .ToDictionary(rs => rs.ResourceIdentifier);

                Trace.TraceInformation("# of existing db string resources: {0}", dbStringResources.Count);
                Trace.TraceInformation("# of resx string resources to process: {0}", stringResources.Count);
                int newlyAdded = 0, updated = 0;

                foreach (var res in stringResources)
                {
                    ResourceString dbRes;
                    if (!dbStringResources.TryGetValue(res.Name, out dbRes))
                    {
                        // newly add string resource that is being processed
                        dbRes = new ResourceString()
                        {
                            FK_BranchId = branchId,
                            FK_ResourceFileId = resourceFileId,
                            ResourceIdentifier = res.Name,
                            ResxComment = res.Comment,
                            ResxValue = res.Value,
                            LastUpdatedFromRepository = DateTime.UtcNow,
                        };
                        ctx.ResourceStrings.Add(dbRes);
                        newlyAdded++;

                        AddOrUpdateTranslationsForResourceString(ctx, dbRes);
                    }
                    else
                    {
                        if (dbRes.ResxComment != res.Comment)
                        {
                            dbRes.ResxComment = res.Comment;
                            dbRes.LastUpdatedFromRepository = DateTime.UtcNow;
                        }

                        if (dbRes.ResxValue != res.Value)
                        {
                            dbRes.ResxValue = res.Value;
                            dbRes.LastUpdatedFromRepository = DateTime.UtcNow;  // yes, potentially set twice w above comment section
                            updated++;

                            AddOrUpdateTranslationsForResourceString(ctx, dbRes);
                        }
                    }
                }

                // Delete string resources that no longer exist
                var resourceIdentifiers = new HashSet<string>(stringResources.Select(s => s.Name));
                var toRemoveFromDb = dbStringResources.Values
                    .Where(s => !resourceIdentifiers.Contains(s.ResourceIdentifier))
                    .ToList();

                Trace.TraceInformation("# of resources added to db: {0}", newlyAdded);
                Trace.TraceInformation("# of resources updated in db: {0}", updated);
                Trace.TraceInformation("# of db resource strings to remove: {0}", toRemoveFromDb.Count);

                ctx.ResourceStrings.RemoveRange(toRemoveFromDb);
                ctx.SaveChanges();
            }

            return true;
        }

        // Why Add & Update in one method: because of the way how we don't delete translations
        // It could be that on adding a "new" resource, it already exists as translation from a previous commit & rollback
        public void AddOrUpdateTranslationsForResourceString(RftContext ctx, ResourceString dbRes)
        {
            int branchId = dbRes.FK_BranchId;
            int resourceFileId = dbRes.FK_ResourceFileId;
            string resourceIdentifier = dbRes.ResourceIdentifier;
            string resxValue = dbRes.ResxValue;

            foreach (var l in _languages)
            {
                string culture = l.Culture;

                var currentTranslation = ctx.Translations
                    .FirstOrDefault(t => t.FK_ResourceFileId == resourceFileId &&
                            t.ResourceIdentifier == resourceIdentifier &&
                            t.Culture == culture &&
                            t.FK_BranchId == branchId);

                int excludeTranslationId = -1;
                if (currentTranslation != null)
                {
                    // we are reviving an existing translation for this resource string
                    excludeTranslationId = currentTranslation.Id;
                }
                else
                {
                    currentTranslation = new Translation()
                    {
                        FK_BranchId = branchId,
                        FK_ResourceFileId = resourceFileId,
                        ResourceIdentifier = resourceIdentifier,
                        Culture = culture,
                        OriginalResxValueAtTranslation = resxValue,
                        LastUpdated = DateTime.UtcNow,
                        LastUpdatedBy = "sync task"
                    };
                    ctx.Translations.Add(currentTranslation);
                }

                var mergeFromTranslation = ctx.Translations
                          .Where(t => t.ResourceIdentifier == resourceIdentifier &&
                                  t.OriginalResxValueAtTranslation == resxValue &&    // NOTE: this is SQL Server comparing here!!!
                                  t.Culture == culture)
                          .OrderByDescending(t => t.LastUpdated)
                          .FirstOrDefault();
                // Note: mergeFromTranslation may be equal to currentTranslation when reviving a translation

                if (null == mergeFromTranslation)
                {
                    if (-1 == excludeTranslationId)
                    {
                        currentTranslation.TranslatedValue = null;
                    }
                    currentTranslation.OriginalResxValueChangedSinceTranslation = true;
                }
                else
                {
                    currentTranslation.OriginalResxValueChangedSinceTranslation = false;
                    currentTranslation.TranslatedValue = mergeFromTranslation.TranslatedValue;
                    currentTranslation.LastUpdated = mergeFromTranslation.LastUpdated;
                    currentTranslation.LastUpdatedBy = mergeFromTranslation.LastUpdatedBy;
                }
            }
        }



    }
}
