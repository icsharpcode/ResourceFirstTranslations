using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Models
{
    public class TranslationEditModel
    {
        // Note: FK_BranchId, FK_ResourceFiledId, ResourceIdentifier, Culture cannot be changed!
        //       Last*, *ChangedSinceTranslation fields will be set by Translation Service

        public int Id { get; set; }
        public string TranslatedValue { get; set; }
    }
}