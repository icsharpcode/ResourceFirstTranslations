using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Common;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Models;

namespace ResourcesFirstTranslations.Services
{
    //
    // Translation Business Logic (mostly caching)
    //
    public class DefaultTranslationService : ITranslationService
    {
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;
        private readonly IMailService _mailService;
        private readonly IConfigurationService _configurationService;

        public DefaultTranslationService(IDataService dataService, ICacheService cacheService, 
            IMailService mailService, IConfigurationService configurationService)
        {
            _dataService = dataService;
            _cacheService = cacheService;
            _mailService = mailService;
            _configurationService = configurationService;
        }

        public ICacheService CacheService { get { return _cacheService; } }
        public IDataService DataService { get { return _dataService; } }

        public async Task<List<Language>> GetCachedLanguagesAsync()
        {
            return await _cacheService
                            .GetAsync(CacheKeys.Languages, () => _dataService.GetLanguagesAsync())
                            .ConfigureAwait(false);
        }

        public async Task<List<Language>> GetCachedLanguagesAsync(List<string> cultures)
        {
            var languages = await GetCachedLanguagesAsync().ConfigureAwait(false);
            return languages.Where(l => cultures.Contains(l.Culture)).ToList();
        }

        public async Task<List<Branch>> GetCachedBranchesAsync()
        {
            return await _cacheService
                            .GetAsync(CacheKeys.Branches, () => _dataService.GetBranchesAsync())
                            .ConfigureAwait(false);
        }

        public async Task<List<ResourceFile>> GetCachedResourceFilesAsync()
        {
            return await _cacheService
                            .GetAsync(CacheKeys.ResourceFiles, () => _dataService.GetResourceFilesAsync())
                            .ConfigureAwait(false);
        }

        public async Task<List<BranchResourceFile>> GetCachedBranchResourceFilesAsync()
        {
            return await _cacheService
                            .GetAsync(CacheKeys.BranchResourceFiles, () => _dataService.GetBranchResourceFilesAsync())
                            .ConfigureAwait(false);
        }

        public async Task CreateUserAsync(User user, string subjectFormat, string bodyFormat, string siteUrl)
        {
            // Create password & password hash
            var password = Password.GeneratePassword();
            var hashedAndSalt = Password.HashPassword(password);
            // Add it to the user object
            user.PasswordHash = hashedAndSalt.Item1;
            user.PasswordSalt = hashedAndSalt.Item2;

            await _dataService.CreateUserAsync(user).ConfigureAwait(false);

            string subject = subjectFormat;
            string body = String.Format(bodyFormat,
                user.FirstName, user.UserName, password, siteUrl);

            var mm = new MailMessage(_configurationService.MailFromAddress, user.EmailAddress, subject, body);
            await _mailService.SendMailAsync(mm).ConfigureAwait(false);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _dataService.UpdateUserAsync(user).ConfigureAwait(false);
        }

        public async Task ResetUserPasswordAsync(User user, string subjectFormat, string bodyFormat, string siteUrl)
        {
            // Create password & password hash
            var password = Password.GeneratePassword();
            var hashedAndSalt = Password.HashPassword(password);
            // Add it to the user object
            user.PasswordHash = hashedAndSalt.Item1;
            user.PasswordSalt = hashedAndSalt.Item2;

            await _dataService.ResetUserPasswordAsync(user).ConfigureAwait(false);

            string subject = subjectFormat;
            string body = String.Format(bodyFormat, password, siteUrl);

            var mm = new MailMessage(_configurationService.MailFromAddress, user.EmailAddress, subject, body);
            await _mailService.SendMailAsync(mm).ConfigureAwait(false);
        }

        public async Task CreateBranchAsync(Branch branch)
        {
            await _dataService.CreateBranchAsync(branch).ConfigureAwait(false);
            _cacheService.Invalidate(CacheKeys.Branches);
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            await _dataService.UpdateBranchAsync(branch).ConfigureAwait(false);
            _cacheService.Invalidate(CacheKeys.Branches);
        }

        public async Task CreateBranchResourceFileAsync(BranchResourceFile bresFile)
        {
            await _dataService.CreateBranchResourceFileAsync(bresFile).ConfigureAwait(false);
            _cacheService.Invalidate(CacheKeys.BranchResourceFiles);
        }

        public async Task UpdateBranchResourceFileAsync(BranchResourceFile bresFile)
        {
            await _dataService.UpdateBranchResourceFileAsync(bresFile).ConfigureAwait(false);
            _cacheService.Invalidate(CacheKeys.BranchResourceFiles);
        }

        public async Task CreateResourceFileAsync(ResourceFile resFile)
        {
            await _dataService.CreateResourceFileAsync(resFile).ConfigureAwait(false);
            _cacheService.Invalidate(CacheKeys.ResourceFiles);
        }

        public async Task UpdateResourceFileAsync(ResourceFile resFile)
        {
            await _dataService.UpdateResourceFileAsync(resFile).ConfigureAwait(false);
            _cacheService.Invalidate(CacheKeys.ResourceFiles);
        }

        public async Task<List<MissingTranslationsModel>> GetMissingTranslationsAsync(int branchId)
        {
            var missingTs = await _dataService.GetMissingTranslationsAsync(branchId).ConfigureAwait(false);
            var cultures = await GetCachedLanguagesAsync().ConfigureAwait(false);

            // This will get us the ones that have missing translation - but will omit those that have zero missing translations
            var projection = from t in missingTs
                             join c in cultures on t.Culture equals c.Culture
                             select new MissingTranslationsModel
                             {
                                 Culture = t.Culture,
                                 CultureDisplayName = c.Description,
                                 MissingTranslations = t.MissingTranslations ?? 0
                             };

            // Add the ones that are completed
            var missingList = projection.OrderByDescending(p => p.MissingTranslations).ToList();
            var completedCultures = cultures
                .Where(c => !(missingList.Select(ml => ml.Culture).ToList().Contains(c.Culture)))
                .OrderBy(c => c.Culture);

            missingList.AddRange(completedCultures
                .Select(completedCulture => new MissingTranslationsModel()
                            {
                                Culture = completedCulture.Culture, 
                                CultureDisplayName = completedCulture.Description, 
                                MissingTranslations = 0
                            }));

            return missingList;
        }

        public async Task<bool> ChangeOwnEmailAddressAsync(string newEmailAddress)
        {
            var p = ClaimsPrincipal.Current;
            var oldEmailAddress = p.GetEmailAddress();

            return await _dataService
                .ChangeOwnEmailAddressAsync(oldEmailAddress, newEmailAddress)
                .ConfigureAwait(false);
        }

        public async Task<Translation> EditTranslationAsync(int id, string translatedValue)
        {
            var userEmail = ClaimsPrincipal.Current.GetEmailAddress();
            var changeTimestamp = DateTime.UtcNow;

            return await _dataService
                .EditTranslationAsync(id, translatedValue, userEmail, changeTimestamp)
                .ConfigureAwait(false);
        }

        public async Task<Translation> EditTranslationMultiBranchAsync(int id, string translatedValue, List<int> branchIds)
        {
            var userEmail = ClaimsPrincipal.Current.GetEmailAddress();
            var changeTimestamp = DateTime.UtcNow;

            return await _dataService
                .EditTranslationMultiBranchAsync(id, translatedValue, branchIds, userEmail, changeTimestamp)
                .ConfigureAwait(false);
        }

        public async Task<ResourceFileForResult> GetResourceFileForAsync(int branch, int file, string culture, 
            bool fillEmptyWithOriginalResourceString, ResourceFileFormat format)
        {
            var branches = await GetCachedBranchesAsync().ConfigureAwait(false);
            var resourceFiles = await GetCachedResourceFilesAsync().ConfigureAwait(false);
            var languages = await GetCachedLanguagesAsync().ConfigureAwait(false);

            var theBranch = branches.FirstOrDefault(b => b.Id == branch);
            var theFile = resourceFiles.FirstOrDefault(rf => rf.Id == file);
            var theLanguage = languages.FirstOrDefault(l => l.Culture == culture);

            // this is a check if a wrong combo is requested
            if (null == theBranch || null == theFile || null == theLanguage)
            {
                return new ResourceFileForResult()
                {
                    Succeeded = false
                };
            }
            
            // Properly format the filename, extension depends on the enum, and we always make it lowercase
            string extension = "." + format.ToString().ToLower();
            string filename = String.Format(theFile.ResourceFileNameFormat, culture) +  extension;

            // Get the translations
            var translations = await _dataService
                .GetNonNullTranslationsForExportAsync(branch, file, culture)
                .ConfigureAwait(false);

            // Get the original resource strings (only when needed! memory & time)
            Dictionary<string, string> resourceStrings = null;
            if (fillEmptyWithOriginalResourceString)
            {
                resourceStrings = (await _dataService
                    .GetResourceStringsForExportAsync(branch, file)
                    .ConfigureAwait(false)).ToDictionary(r => r.ResourceIdentifier, r => r.Value);
            }

            // Create the resource file: First the translations, then fill with the original language strings (if requested)
            var ms = new MemoryStream();
            IResourceWriter writer = null;

            // format decides the writer, both implement IResourceWriter
            if (ResourceFileFormat.ResX == format)
            {
                writer = new ResXResourceWriter(ms);
            }
            else
            {
                writer = new ResourceWriter(ms);
            }

            foreach (var t in translations)
            {
                writer.AddResource(t.ResourceIdentifier, t.Value);
                if (fillEmptyWithOriginalResourceString)
                {
                    resourceStrings.Remove(t.ResourceIdentifier);
                }
            }
            if (fillEmptyWithOriginalResourceString)
            {
                foreach (var t in resourceStrings)
                {
                    writer.AddResource(t.Key, t.Value);
                }
            }

            writer.Generate();
            ms.Seek(0, SeekOrigin.Begin);

            return new ResourceFileForResult()
            {
                Succeeded = true,
                Stream = ms,
                Filename = filename
            };
        }
    }
}
