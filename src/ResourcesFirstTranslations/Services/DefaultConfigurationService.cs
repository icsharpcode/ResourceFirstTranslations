using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Services
{
    public class DefaultConfigurationService : IConfigurationService
    {
        private string GetSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }

        public string MailFromAddress
        {
            get
            {
                return GetSetting("MailFromAddress");
            }
        }
        public string MailService
        {
            get
            {
                return GetSetting("MailService");
            }
        }

        public string SendGridUsername
        {
            get
            {
                return GetSetting("SendGridUsername");
            }
        }

        public string SendGridPassword
        {
            get
            {
                return GetSetting("SendGridPassword");
            }
        }

        public string SmtpUsername
        {
            get
            {
                return GetSetting("SmtpUsername");
            }
        }

        public string SmtpPassword
        {
            get
            {
                return GetSetting("SmtpPassword");
            }
        }

        public string SmtpHost
        {
            get
            {
                return GetSetting("SmtpHost");
            }
        }

        public string SmtpPort
        {
            get
            {
                return GetSetting("SmtpPort");
            }
        }

        public bool FillEmptyTranslationsWithOriginalValues
        {
            get
            {
                string setting = GetSetting("FillEmptyTranslationsWithOriginalValues");
                bool fillEmpty = false;
                bool.TryParse(setting, out fillEmpty);
                return fillEmpty;
            }
        }
    }
}