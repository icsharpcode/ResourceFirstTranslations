using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Models;

namespace ResourcesFirstTranslations.Services
{
    public interface IDataService
    {
        Task<User> FindUserAsync(string name, string password);
        Task<User> FindByNameAsync(string email);
        Task<bool> ChangeUserPasswordAsync(int userId, string oldPassword, string newPassword);

        Task<List<Branch>> GetBranchesAsync();
        Task<List<ResourceFile>> GetResourceFilesAsync();
        Task<List<BranchResourceFile>> GetBranchResourceFilesAsync();
        List<AllTranslationsForResource> GetAllTranslationsForResources(List<TranslationItemParameter> items);
        Task<bool> AddCultureAsync(string culture, string description);
        Task<List<Language>> GetLanguagesAsync();

        Task<List<TranslationExportModel>> GetNonNullTranslationsForExportAsync(int branch, int file, string culture);
        Task<List<TranslationExportModel>> GetResourceStringsForExportAsync(int branch, int file);

        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task ResetUserPasswordAsync(User user);
        Task<List<string>> GetActiveTranslatorsEmailAddressesAsync();

        Task CreateBranchAsync(Branch branch);
        Task UpdateBranchAsync(Branch branch);
        Task CreateBranchResourceFileAsync(BranchResourceFile bresFile);
        Task UpdateBranchResourceFileAsync(BranchResourceFile bresFile);
        Task CreateResourceFileAsync(ResourceFile resFile);
        Task UpdateResourceFileAsync(ResourceFile resFile);

        Task<List<MissingTranslationsView>> GetMissingTranslationsAsync(int branchId);

        Task<bool> ChangeOwnEmailAddressAsync(string oldEmailAddress, string newEmailAddress);
        Task<Translation> EditTranslationAsync(int id, string translatedValue, string changedBy, DateTime changedAt);
        Task<Translation> EditTranslationMultiBranchAsync(int id, string translatedValue, List<int> branchIds, string changedBy, DateTime changedAt);

        Task<List<CoTranslator>> GetCoTranslatorsAsync(List<string> cultures);
    }
}
