using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Models
{
    public class AddCultureRequest
    {
        public string Culture { get; set; }
        public string Description { get; set; }
    }
}