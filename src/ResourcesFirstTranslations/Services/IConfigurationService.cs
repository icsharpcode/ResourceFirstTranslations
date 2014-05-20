using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Services
{
    public interface IConfigurationService
    {
        string MailFromAddress { get; }
        string MailService { get; }

        string SendGridUsername { get; }
        string SendGridPassword { get; }

        string SmtpUsername { get; }
        string SmtpPassword { get; }
        string SmtpHost { get; }
        string SmtpPort { get; }

        bool FillEmptyTranslationsWithOriginalValues { get; }
        bool EnableMultiBranchTranslation { get; }
    }
}
