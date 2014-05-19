using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Models
{
    public class BranchResourceFileModel
    {
        public int Id { get; set; }
        public int FK_BranchId { get; set; }
        public int FK_ResourceFileId { get; set; }
        public string SyncRawPathAbsolute { get; set; }

        public Data.BranchResourceFile ToBranchResourceFile()
        {
            return new Data.BranchResourceFile
            {
                Id = this.Id,
                FK_BranchId = this.FK_BranchId,
                FK_ResourceFileId = this.FK_ResourceFileId,
                SyncRawPathAbsolute = this.SyncRawPathAbsolute
            };
        }
    }
}