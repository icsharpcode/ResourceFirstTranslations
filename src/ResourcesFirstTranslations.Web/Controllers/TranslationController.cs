using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using ResourcesFirstTranslations.Common;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Services;
using ResourcesFirstTranslations.Web.Models;
using ResourcesFirstTranslations.Web.Resources;

namespace ResourcesFirstTranslations.Web.Controllers
{
    [Authorize]
    public class TranslationController : Controller
    {
        private readonly ITranslationService _translationService;
        private readonly IConfigurationService _configurationService;

        public TranslationController(ITranslationService translationService, IConfigurationService configurationService)
        {
            _translationService = translationService;
            _configurationService = configurationService;
        }

        public ActionResult TopNav()
        {
            return View();
        }

        // This is the SPA starting point (list/edit)
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetDashboardInfo()
        {
            var p = ClaimsPrincipal.Current;

            var languages = await GetLanguagesAsync();
            var branches = await _translationService.GetCachedBranchesAsync();
            var resourceFiles = await _translationService.GetCachedResourceFilesAsync();

            var dashboardinfo = new TranslatorDashboardInfo()
            {
                Name = p.Identity.Name,
                EmailAddress = p.GetEmailAddress(),
                Languages = languages,
                ResourceFiles = resourceFiles,
                Branches = branches
            };
            return Json(dashboardinfo);
        }

        private async Task<List<Language>> GetLanguagesAsync()
        {
            var cultures = ClaimsPrincipal.Current.GetCultures();
            var languages = await _translationService.GetCachedLanguagesAsync(cultures);
            return languages;
        }

        [HttpPost]
        public async Task<JsonResult> GetTranslationFilterDefinition()
        {
            var languages = await GetLanguagesAsync();
            var branches = await _translationService.GetCachedBranchesAsync();

            var filterdef = new TranslationFilterDefinition()
            {
                Languages = languages,
                Branches = branches,
                EnableMultiBranchTranslation = _configurationService.EnableMultiBranchTranslation
            };

            return Json(filterdef);
        }

        [HttpPost]
        public JsonResult GetAdditionalTranslations(List<TranslationItemParameter> items)
        {
            try
            {
                var additionalTranslations = _translationService.DataService.GetAllTranslationsForResources(items);
                return Json(additionalTranslations);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.ToString()));
            }
        }

        [HttpPost]
        public JsonResult GetAdditionalTranslationsExceptBranch(List<TranslationItemParameter> items, int branchId)
        {
            List<AllTranslationsForResource> additionalTranslations =
                _translationService.DataService.GetAllTranslationsForResources(items);

            var trimmed = additionalTranslations.Where(t => t.FK_BranchId != branchId).ToList();

            return Json(trimmed);
        }

        [HttpPost]
        [AccessDeniedRestrictedMode]
        public async Task<JsonResult> ChangeEmailAddress(string newEmailAddress)
        {
            try
            {
                await _translationService.ChangeOwnEmailAddressAsync(newEmailAddress);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new GenericResponse(false, ex.ToString()));
            }

            return Json(new GenericResponse(true, AppResources.ChangeEmailAddressSuccess));
        }

        [HttpPost]
        public async Task<JsonResult> EditTranslation(TranslationEditModel vm)
        {
            Translation t = null;
            try
            {
                t = await _translationService.EditTranslationAsync(vm.Id, vm.TranslatedValue);
                if (null == t)
                {
                    return Json(new TranslationResponse(false, AppResources.TranslationNotFound));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new TranslationResponse(false, ex.ToString()));
            }

            return Json(new TranslationResponse(true, AppResources.TranslationUpdatedSuccessfully, t.LastUpdated, t.LastUpdatedBy, t.OriginalResxValueChangedSinceTranslation));
        }

        [HttpPost]
        public async Task<JsonResult> EditTranslationMultiBranch(MultiBranchTranslationEditModel vm)
        {
            Translation t = null;
            try
            {
                t = await _translationService.EditTranslationMultiBranchAsync(vm.Id, vm.TranslatedValue, vm.BranchIds);
                if (null == t)
                {
                    return Json(new TranslationResponse(false, AppResources.TranslationNotFound));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return Json(new TranslationResponse(false, ex.ToString()));
            }

            return Json(new TranslationResponse(true, AppResources.TranslationUpdatedSuccessfully, t.LastUpdated, t.LastUpdatedBy, t.OriginalResxValueChangedSinceTranslation));
        }

        [HttpPost]
        public async Task<ActionResult> GetCoTranslators()
        {
            try
            {
                var cultures = ClaimsPrincipal.Current.GetCultures();
                var cotranslators = await _translationService.DataService.GetCoTranslatorsAsync(cultures);

                return Json(cotranslators);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return HttpNotFound();
            }
        }
    }
}