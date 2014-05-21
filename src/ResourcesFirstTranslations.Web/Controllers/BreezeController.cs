using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web.Http;
using Breeze.WebApi2;
using Breeze.ContextProvider.EF6;
using ResourcesFirstTranslations.Data;

namespace ResourcesFirstTranslations.Web.Controllers
{
    [Authorize]
    [BreezeController]
    public class BreezeController : ApiController
    {
        private readonly EFContextProvider<RftContext> _contextProvider = new EFContextProvider<RftContext>();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        // User *has* to have a branch selected, otherwise this will return invalid results
        [HttpGet]
        public IQueryable<TranslationsJoinedResourceStringsView> QueryPerBranch()
        {
            return _contextProvider.Context.TranslationsJoinedResourceStringsViews;
        }
       
    }
}
