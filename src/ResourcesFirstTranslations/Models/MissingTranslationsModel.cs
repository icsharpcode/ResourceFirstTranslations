using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Models
{
    public class MissingTranslationsModel
    {
        public string Culture { get; set; }
        public string CultureDisplayName { get; set; }
        public int MissingTranslations { get; set; }
    }
}