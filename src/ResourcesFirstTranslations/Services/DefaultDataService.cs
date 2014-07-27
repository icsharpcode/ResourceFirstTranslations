using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using LinqKit;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Models;

namespace ResourcesFirstTranslations.Services
{
    public class DefaultDataService : IDataService
    {
        public DefaultDataService()
        {
            GetContext = () => new RftContext();
        }

        private Func<RftContext> GetContext { get; set; }

        public async Task<User> FindUserAsync(string name, string password)
        {
            using (var ctx = GetContext())
            {
                var user = await ctx.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserName == name && u.IsActive)
                    .ConfigureAwait(false);

                if (null == user)
                    return null;

                bool isPasswordValid = Common.Password.IsPasswordValid(password, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordValid)
                    return null;

                return user;
            }
        }

        public async Task<User> FindByNameAsync(string email)
        {
            using (var ctx = GetContext())
            {
                var user = await ctx.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.EmailAddress == email && u.IsActive)
                    .ConfigureAwait(false);

                if (null == user)
                    return null;

                return user;
            } 
        }

        public async Task<bool> ChangeUserPasswordAsync(int userId, string oldPassword, string newPassword)
        {
            using (var ctx = GetContext())
            {
                var user = await ctx.Users
                    .FirstOrDefaultAsync(u => u.Id == userId)
                    .ConfigureAwait(false);

                if (null == user)
                    return false;

                bool isPasswordValid = Common.Password.IsPasswordValid(oldPassword, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordValid)
                    return false;

                var newPasswordTuple = Common.Password.HashPassword(newPassword);

                user.PasswordHash = newPasswordTuple.Item1;
                user.PasswordSalt = newPasswordTuple.Item2;

                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }

        public async Task<List<Branch>> GetBranchesAsync()
        {
            using (var ctx = GetContext())
            {
                return await ctx.Branches
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<List<ResourceFile>> GetResourceFilesAsync()
        {
            using (var ctx = GetContext())
            {
                return await ctx.ResourceFiles
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<List<BranchResourceFile>> GetBranchResourceFilesAsync()
        {
            using (var ctx = GetContext())
            {
                return await ctx.BranchResourceFiles
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public List<AllTranslationsForResource> GetAllTranslationsForResources(List<TranslationItemParameter> items)
        {
            using (var ctx = GetContext())
            {
                string xml = TranslationItemParameter.ListToXml(items);

                ObjectResult<AllTranslationsForResource> result = ctx.GetAllTranslationsForResource(xml);
                return result.ToList();
            }
        }

        public async Task<bool> AddCultureAsync(string culture, string description)
        {
            string providerConnectionString = "";
            List<ResourceString> resourceStrings = null;

            using (var ctx = GetContext())
            {
                // Get all original resources for all branches (no tracking, we make no changes)
                resourceStrings = await ctx.ResourceStrings.AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                // And get the connection string
                providerConnectionString = ctx.GetProviderConnectionString();
            }

            DateTime updateTime = DateTime.UtcNow;
            const string updatedBy = "system/add language";

            var translationProjection = resourceStrings.Select(s => new Translation()
            {
                FK_BranchId = s.FK_BranchId,
                FK_ResourceFileId = s.FK_ResourceFileId,
                ResourceIdentifier = s.ResourceIdentifier,
                Culture = culture,
                TranslatedValue = null,
                OriginalResxValueChangedSinceTranslation = true,
                OriginalResxValueAtTranslation = s.ResxValue,
                LastUpdated = updateTime,
                LastUpdatedBy = updatedBy
            });

            using (var conn = new SqlConnection(providerConnectionString))
            {
                await conn.OpenAsync();

                using (var sqlTxn = conn.BeginTransaction())
                {
                    var cmd =
                        new SqlCommand("INSERT INTO Languages(Culture,Description) VALUES(@param1,@param2)", conn, sqlTxn);

                    cmd.Parameters.Add(new SqlParameter("@param1", culture));
                    cmd.Parameters.Add(new SqlParameter("@param2", description));

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();

                        var bcp = new Overby.Data.BulkInserter<Translation>(conn, "Translations",
                            5000, SqlBulkCopyOptions.Default, sqlTxn);

                        bcp.Insert(translationProjection);

                        sqlTxn.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.ToString());
                        sqlTxn.Rollback();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return false;
        }

        public async Task<List<Language>> GetLanguagesAsync()
        {
            using (var ctx = GetContext())
            {
                return await ctx.Languages
                    .AsNoTracking()
                    .OrderBy(l => l.Description)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<List<TranslationExportModel>> GetNonNullTranslationsForExportAsync(int branch, int file, string culture)
        {
            using (var ctx = GetContext())
            {
                return await ctx.TranslationsJoinedResourceStringsViews
                    .AsNoTracking()
                    .Where(t => t.TranslatedValue != null &&
                                t.TranslatedValue != "" && 
                                t.FK_BranchId == branch &&
                                t.FK_ResourceFileId == file &&
                                t.Culture == culture)
                    .OrderBy(t => t.ResourceIdentifier)
                    .Select(t => new TranslationExportModel()
                    {
                        ResourceIdentifier = t.ResourceIdentifier,
                        Value = t.TranslatedValue
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<List<TranslationExportModel>> GetResourceStringsForExportAsync(int branch, int file)
        {
            using (var ctx = GetContext())
            {
                return await ctx.ResourceStrings
                    .AsNoTracking()
                    .Where(t => t.FK_BranchId == branch &&
                                t.FK_ResourceFileId == file)
                    .OrderBy(t => t.ResourceIdentifier)
                    .Select(t => new TranslationExportModel()
                    {
                        ResourceIdentifier = t.ResourceIdentifier,
                        Value = t.ResxValue
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task CreateUserAsync(User user)
        {
            using (var ctx = GetContext())
            {
                ctx.Users.Add(user);
                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var ctx = GetContext())
            {
                var dbUser = await ctx.Users.FindAsync(user.Id).ConfigureAwait(false);

                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.EmailAddress = user.EmailAddress;
                dbUser.IsActive = user.IsActive;
                dbUser.IsAdmin = user.IsAdmin;
                dbUser.Cultures = user.Cultures;

                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }


        public async Task ResetUserPasswordAsync(User user)
        {
            using (var ctx = GetContext())
            {
                var dbUser = await ctx.Users
                    .SingleAsync(u => u.Id == user.Id && u.EmailAddress == user.EmailAddress)
                    .ConfigureAwait(false);

                dbUser.PasswordHash = user.PasswordHash;
                dbUser.PasswordSalt = user.PasswordSalt;

                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<string>> GetActiveTranslatorsEmailAddressesAsync()
        {
            using (var ctx = GetContext())
            {
                return await ctx.Users
                    .Where(u => u.IsActive)
                    .Select(u => u.EmailAddress)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task CreateBranchAsync(Branch branch)
        {
            using (var ctx = GetContext())
            {
                ctx.Branches.Add(branch);
                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            using (var ctx = GetContext())
            {
                Branch dbBranch = await ctx.Branches.FindAsync(branch.Id).ConfigureAwait(false);

                dbBranch.BranchDisplayName = branch.BranchDisplayName;
                dbBranch.BranchRootUrl = branch.BranchRootUrl;

                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task CreateBranchResourceFileAsync(BranchResourceFile bresFile)
        {
            using (var ctx = GetContext())
            {
                ctx.BranchResourceFiles.Add(bresFile);
                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateBranchResourceFileAsync(BranchResourceFile bresFile)
        {
            using (var ctx = GetContext())
            {
                BranchResourceFile dbBranch = await ctx.BranchResourceFiles.FindAsync(bresFile.Id).ConfigureAwait(false);

                dbBranch.FK_BranchId = bresFile.FK_BranchId;
                dbBranch.FK_ResourceFileId = bresFile.FK_ResourceFileId;
                dbBranch.SyncRawPathAbsolute = bresFile.SyncRawPathAbsolute;

                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task CreateResourceFileAsync(ResourceFile resFile)
        {
            using (var ctx = GetContext())
            {
                ctx.ResourceFiles.Add(resFile);
                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateResourceFileAsync(ResourceFile resFile)
        {
            using (var ctx = GetContext())
            {
                ResourceFile dbBranch = await ctx.ResourceFiles.FindAsync(resFile.Id).ConfigureAwait(false);

                dbBranch.ResourceFileDisplayName = resFile.ResourceFileDisplayName;
                dbBranch.ResourceFileNameFormat = resFile.ResourceFileNameFormat;

                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<MissingTranslationsView>> GetMissingTranslationsAsync(int branchId)
        {
            using (var ctx = GetContext())
            {
                return await ctx.MissingTranslationsViews
                    .Where(m => m.FK_BranchId == branchId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<bool> ChangeOwnEmailAddressAsync(string oldEmailAddress, string newEmailAddress)
        {
            using (var ctx = GetContext())
            {
                var user = await ctx.Users
                    .FirstOrDefaultAsync(u => u.EmailAddress == oldEmailAddress)
                    .ConfigureAwait(false);

                if (null == user)
                    return false;

                user.EmailAddress = newEmailAddress;

                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }

        public async Task<Translation> EditTranslationAsync(int id, string translatedValue, string changedBy, DateTime changedAt)
        {
            using (var ctx = GetContext())
            {
                var translation = await ctx.Translations
                    .FirstOrDefaultAsync(t => t.Id == id)
                    .ConfigureAwait(false);

                if (null == translation)
                    return null;

                translation.TranslatedValue = translatedValue;
                translation.OriginalResxValueChangedSinceTranslation = false;
                translation.LastUpdatedBy = changedBy;
                translation.LastUpdated = changedAt;

                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return translation;
            }
        }

        public async Task<Translation> EditTranslationMultiBranchAsync(int id, string translatedValue, List<int> branchIds, string changedBy, DateTime changedAt)
        {
            using (var ctx = GetContext())
            {
                // Intentional special-casing of translation that was edited
                var translation = await ctx.Translations
                    .FirstOrDefaultAsync(t => t.Id == id)
                    .ConfigureAwait(false);

                if (null == translation)
                    return null;

                translation.TranslatedValue = translatedValue;
                translation.OriginalResxValueChangedSinceTranslation = false;
                translation.LastUpdatedBy = changedBy;
                translation.LastUpdated = changedAt;

                // Make sure we do not edit ourselves again, only the ones we should auto-update
                branchIds.Remove(translation.FK_BranchId);

                var additionalTranslations = await ctx.Translations
                    .Where(t => t.FK_ResourceFileId == translation.FK_ResourceFileId
                                && t.Culture == translation.Culture
                                && t.ResourceIdentifier == translation.ResourceIdentifier)
                    .ToListAsync()
                    .ConfigureAwait(false);

                foreach (var t in additionalTranslations)
                {
                    t.TranslatedValue = translatedValue;
                    t.OriginalResxValueChangedSinceTranslation = false;
                    t.LastUpdatedBy = changedBy;
                    t.LastUpdated = changedAt;
                }

                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return translation;
            }
        }

        public async Task<List<CoTranslator>> GetCoTranslatorsAsync(List<string> cultures)
        {
            using (var ctx = GetContext())
            {
                var predicate = PredicateBuilder.False<User>();
                foreach (string c in cultures)
                {
                    string tempClosure = "|" + c + "|";
                    predicate = predicate.Or(u => u.Cultures.Contains(tempClosure));
                }

                IQueryable<User> query = ctx.Users.AsExpandable()
                    .Where(predicate)
                    .Where(u => u.IsActive);

                var cotranslators = await query
                    .Select(u => new CoTranslator()
                    {
                        EmailAddress = u.EmailAddress,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Cultures = u.Cultures
                    })
                    .ToListAsync().ConfigureAwait(false);

                return cotranslators;
            }
        }
    }
}
