using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Models
{
    public class ResourceFileModel
    {
        public int Id { get; set; }
        public string ResourceFileDisplayName { get; set; }
        public string ResourceFileNameFormat { get; set; }

        public Data.ResourceFile ToResourceFile()
        {
            return new Data.ResourceFile
            {
                Id = this.Id,
                ResourceFileDisplayName = this.ResourceFileDisplayName,
                ResourceFileNameFormat = this.ResourceFileNameFormat
            };
        }
    }
}