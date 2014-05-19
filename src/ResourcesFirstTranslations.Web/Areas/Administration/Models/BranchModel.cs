using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Models
{
    public class BranchModel
    {
        public int Id { get; set; }
        public string BranchDisplayName { get; set; }
        public string BranchRootUrl { get; set; }

        public Data.Branch ToBranch()
        {
            return new Data.Branch
            {
                Id = this.Id,
                BranchDisplayName = this.BranchDisplayName,
                BranchRootUrl = this.BranchRootUrl
            };
        }
    }
}