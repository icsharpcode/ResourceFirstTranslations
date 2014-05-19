using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResourcesFirstTranslations.Data;

namespace ResourcesFirstTranslations.Web.Models
{
    public class TranslatorDashboardInfo
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public List<Language> Languages { get; set; } 
        public List<ResourceFile> ResourceFiles { get; set; }
        public List<Branch> Branches { get; set; }
    }
}