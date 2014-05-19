using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Models
{
    public class MultiBranchTranslationEditModel : TranslationEditModel
    {
        public List<int> BranchIds { get; set; } 
    }
}