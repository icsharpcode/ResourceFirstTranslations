using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResourcesFirstTranslations.Data;

namespace ResourcesFirstTranslations.Web.Models
{
    public class TranslationFilterDefinition
    {
        public bool EnableMultiBranchTranslation { get; set; }
        public List<Language> Languages { get; set; }
        public List<Branch> Branches { get; set; }
    }
}