using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Models
{
    public class SendEmailRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}