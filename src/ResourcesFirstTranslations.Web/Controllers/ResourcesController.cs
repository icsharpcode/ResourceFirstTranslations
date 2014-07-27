using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Models;
using ResourcesFirstTranslations.Services;

namespace ResourcesFirstTranslations.Web.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly ITranslationService _translationService;
        private readonly IConfigurationService _configurationService;

        public ResourcesController(ITranslationService translationService, IConfigurationService configurationService)
        {
            _translationService = translationService;
            _configurationService = configurationService;
        }

        // http://localhost:19890/Resources/For?branch=500&file=1&culture=de
        public async Task<ActionResult> For(int branch, int file, string culture, string format)
        {
            try
            {
                var resFormat = ResourceFormatStringToEnum(format);
                bool fillEmpty = _configurationService.FillEmptyTranslationsWithOriginalValues;
                var result = await _translationService.GetResourceFileForAsync(branch, file, culture, fillEmpty, resFormat);

                if (!result.Succeeded)
                {
                    return HttpNotFound();
                }

                string contentType = "application/octet-stream";
                if (ResourceFileFormat.ResX == resFormat)
                {
                    contentType = "text/xml";
                }

                return File(result.Stream, contentType, result.Filename);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return HttpNotFound();
            }
        }

        // We default to .resources if the format cannot be parsed
        private ResourceFileFormat ResourceFormatStringToEnum(string format)
        {
            if (0 == String.Compare("resx", format, StringComparison.InvariantCultureIgnoreCase))
            {
                return ResourceFileFormat.ResX;
            }

            return ResourceFileFormat.Resources;
        }

        // http://localhost:19890/Resources/Missing?branch=500
        public async Task<ActionResult> Missing(int branch)
        {
            try
            {
                var missing = await _translationService.GetMissingTranslationsAsync(branch);
                return Json(missing, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return HttpNotFound();
            }
        }

        // http://localhost:19890/Resources/ResourceFiles?branch=500
        public async Task<ActionResult> ResourceFiles(int branch)
        {
            try
            {
                var branchFiles = await _translationService.GetCachedBranchResourceFilesAsync();

                var rfIds = branchFiles.Where(bf => bf.FK_BranchId == branch)
                    .Select(bf => bf.FK_ResourceFileId)
                    .ToList();

                return Json(rfIds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return HttpNotFound();
            }
        }
    }
}