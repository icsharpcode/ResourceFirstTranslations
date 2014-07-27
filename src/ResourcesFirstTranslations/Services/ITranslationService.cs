using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Models;

namespace ResourcesFirstTranslations.Services
{
    public interface ITranslationService
    {
        ICacheService CacheService { get; }
        IDataService DataService { get; }
        Task<List<Language>> GetCachedLanguagesAsync();
        Task<List<Language>> GetCachedLanguagesAsync(List<string> cultures);


        Task<List<Branch>> GetCachedBranchesAsync();
        Task<List<ResourceFile>> GetCachedResourceFilesAsync();
        Task<List<BranchResourceFile>> GetCachedBranchResourceFilesAsync();

        Task CreateUserAsync(User user, string subjectFormat, string bodyFormat, string siteUrl);
        Task ResetUserPasswordAsync(User user, string subjectFormat, string bodyFormat, string siteUrl);
        Task UpdateUserAsync(User user);

        Task CreateBranchAsync(Branch branch);
        Task UpdateBranchAsync(Branch branch);
        Task CreateBranchResourceFileAsync(BranchResourceFile bresFile);
        Task UpdateBranchResourceFileAsync(BranchResourceFile bresFile);
        Task CreateResourceFileAsync(ResourceFile resFile);
        Task UpdateResourceFileAsync(ResourceFile resFile);

        Task<List<MissingTranslationsModel>> GetMissingTranslationsAsync(int branchId);
        Task<bool> ChangeOwnEmailAddressAsync(string newEmailAddress);
        Task<Translation> EditTranslationAsync(int id, string translatedValue);
        Task<Translation> EditTranslationMultiBranchAsync(int id, string translatedValue, List<int> branchIds);

        Task<ResourceFileForResult> GetResourceFileForAsync(int branch, int file, string culture, bool fillEmptyWithOriginalResourceString, ResourceFileFormat format);
    }
}
