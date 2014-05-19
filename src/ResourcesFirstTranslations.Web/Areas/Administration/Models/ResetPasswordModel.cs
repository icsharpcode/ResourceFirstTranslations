using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Models
{
    public class ResetPasswordModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }
}