using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ResourcesFirstTranslations.Common;
using ResourcesFirstTranslations.Services;
using ResourcesFirstTranslations.Web.Areas.Administration.Models;
using ResourcesFirstTranslations.Web.Models;
using ResourcesFirstTranslations.Web.Resources;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = RftAuthenticationManager.AdministratorRole)]
    public class AdministrationHomeController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;
        private readonly ITranslationService _translationService;
        private readonly IMailService _mailService;
        private readonly IConfigurationService _configurationService;

        public AdministrationHomeController(IDataService dataService, ICacheService cacheService,
            ITranslationService translationService, IMailService mailService, IConfigurationService configurationService)
        {
            _dataService = dataService;
            _cacheService = cacheService;
            _translationService = translationService;
            _mailService = mailService;
            _configurationService = configurationService;
        }

        public ActionResult TopNav()
        {
            return View();
        }

        // GET: /Administration/AdministrationHome/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendEmail(SendEmailRequest emailRequest)
        {
            try
            {
                var fromAddress = _configurationService.MailFromAddress;

                var to = await _dataService.GetActiveTranslatorsEmailAddressesAsync();

                await _mailService.SendMailAsync(new MailMessage()
                {
                    Subject = emailRequest.Subject,
                    Body = emailRequest.Body,
                    From = fromAddress,
                    To = to
                });

                return Json(new GenericResponse(true, AppResources.EmailSentSuccessfully));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.ToString()));
            }
        }

        [HttpPost]
        [AccessDeniedRestrictedMode]
        public async Task<JsonResult> CreateNewLanguage(AddCultureRequest vm)
        {
            try
            {
                bool result = await _dataService.AddCultureAsync(vm.Culture, vm.Description);

                if (result)
                {
                    _cacheService.Invalidate(CacheKeys.Languages);
                    return Json(new GenericResponse(true, AppResources.LanguageCreatedSuccessfully));
                }

                return Json(new GenericResponse(false, AppResources.LanguageCreationFailed));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.ToString()));
            }
        }

        [HttpPost]
        public JsonResult DeleteLanguage(AddCultureRequest vm)
        {
            return Json(new GenericResponse(false, "This feature is not supported"));
        }

        [HttpPost]
        [AccessDeniedRestrictedMode]
        public async Task<JsonResult> CreateNewTranslator(TranslatorModel vm)
        {
            try
            {
                bool isValidCulturesList = await Cultures.IsValidCulturesListAsync(vm.Cultures, _translationService);
                if (!isValidCulturesList)
                {
                    return Json(new GenericResponse(false, AppResources.InvalidCultureList));
                }

                var user = vm.ToNewUser();

                await _translationService.CreateUserAsync(user,
                    MailTemplates.NewUserSubjectFormat, MailTemplates.NewUserBodyFormat, ApplicationInformation.Current.GetSiteBaseUrl(this));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        [AccessDeniedRestrictedMode]
        public async Task<JsonResult> EditTranslator(TranslatorModel vm)
        {
            try
            {
                bool isValidCulturesList = await Cultures.IsValidCulturesListAsync(vm.Cultures, _translationService);
                if (!isValidCulturesList)
                {
                    return Json(new GenericResponse(false, AppResources.InvalidCultureList));
                }

                var user = vm.ToUser();
                await _translationService.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public async Task<JsonResult> CreateNewBranch(BranchModel vm)
        {
            try
            {
                var branch = vm.ToBranch();
                await _translationService.CreateBranchAsync(branch);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public async Task<JsonResult> EditBranch(BranchModel vm)
        {
            try
            {
                var branch = vm.ToBranch();
                await _translationService.UpdateBranchAsync(branch);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public JsonResult DeleteBranch(BranchModel vm)
        {
            return Json(new GenericResponse(false, "This feature is not supported"));
        }

        [HttpPost]
        public async Task<JsonResult> CreateNewBranchResourceFile(BranchResourceFileModel vm)
        {
            try
            {
                var bresFile = vm.ToBranchResourceFile();
                await _translationService.CreateBranchResourceFileAsync(bresFile);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public async Task<JsonResult> EditBranchResourceFile(BranchResourceFileModel vm)
        {
            try
            {
                var bresFile = vm.ToBranchResourceFile();
                await _translationService.UpdateBranchResourceFileAsync(bresFile);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public JsonResult DeleteBranchResourceFile(BranchResourceFileModel vm)
        {
            return Json(new GenericResponse(false, "This feature is not supported"));
        }

        [HttpPost]
        public async Task<JsonResult> CreateNewResourceFile(ResourceFileModel vm)
        {
            try
            {
                var bresFile = vm.ToResourceFile();
                await _translationService.CreateResourceFileAsync(bresFile);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public async Task<JsonResult> EditResourceFile(ResourceFileModel vm)
        {
            try
            {
                var bresFile = vm.ToResourceFile();
                await _translationService.UpdateResourceFileAsync(bresFile);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public JsonResult DeleteResourceFile(ResourceFileModel vm)
        {
            return Json(new GenericResponse(false, "This feature is not supported"));
        }

        [HttpPost]
        [AccessDeniedRestrictedMode]
        public async Task<JsonResult> SendPasswordEmail(ResetPasswordModel vm)
        {
            try
            {
                var user = new Data.User()
                {
                    Id = vm.Id,
                    EmailAddress = vm.EmailAddress
                };

                await _translationService.ResetUserPasswordAsync(user,
                    MailTemplates.PasswordResetSubjectFormat, 
                    MailTemplates.PasswordResetBodyFormat, 
                    ApplicationInformation.Current.GetSiteBaseUrl(this));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.Message));
            }

            return Json(new GenericResponse(true, AppResources.GenericSaveSuccess));
        }

        [HttpPost]
        public async Task<JsonResult> GetMissingTranslations(int branchId)
        {
            try
            {
                var missing = await _translationService.GetMissingTranslationsAsync(branchId);
                return Json(missing);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return Json(new GenericResponse(false));
        }
    }
}