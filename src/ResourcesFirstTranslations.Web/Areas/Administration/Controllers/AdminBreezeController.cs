using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using ResourcesFirstTranslations.Common;
using ResourcesFirstTranslations.Data;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Controllers
{
    [System.Web.Mvc.Authorize(Roles = RftAuthenticationManager.AdministratorRole)]
    [BreezeController]
    public class AdminBreezeController : ApiController
    {
        private readonly EFContextProvider<RftContext> _contextProvider = new EFContextProvider<RftContext>();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpGet]
        [BreezeQueryable]
        public IQueryable<object> Translators()
        {
            // Why? We don't want to send password hash and password salt across the wire
            return _contextProvider.Context.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.EmailAddress,
                u.FirstName,
                u.LastName,
                u.IsActive,
                u.IsAdmin,
                u.Cultures
            });
        }

        [HttpGet]
        public IQueryable<Branch> Branches()
        {
            return _contextProvider.Context.Branches;
        }

        [HttpGet]
        public IQueryable<ResourceFile> ResourceFiles()
        {
            return _contextProvider.Context.ResourceFiles;
        }

        [HttpGet]
        public IQueryable<BranchResourceFilesView> BranchesResourceFiles()
        {
            return _contextProvider.Context.BranchResourceFilesViews;
        }

        [HttpGet]
        public IQueryable<Language> Languages()
        {
            return _contextProvider.Context.Languages;
        }
    }
}
