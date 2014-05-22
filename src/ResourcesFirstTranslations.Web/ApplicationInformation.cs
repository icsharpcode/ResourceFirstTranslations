using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ResourcesFirstTranslations.Web
{
    public class ApplicationInformation
    {
        private ApplicationInformation()
        {
        }
        
        static ApplicationInformation()
        {
            Current = new ApplicationInformation();
        }

        public static ApplicationInformation Current { get; private set; }

        private string _name;
        public string Name
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_name))
                {
                    _name = ConfigurationManager.AppSettings["ApplicationName"];
                }

                return _name;
            }
        }

        private string _appVersion;
        public string Version
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_appVersion))
                {
                    Assembly assembly = Assembly.GetAssembly(typeof(ApplicationInformation));
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    _appVersion = fvi.FileVersion;
                }

                return _appVersion;
            }
        }

        public string GetSiteBaseUrl(Controller controller)
        {
            return string.Format("{0}://{1}{2}", 
                controller.Request.Url.Scheme, 
                controller.Request.Url.Authority, 
                controller.Url.Content("~"));
        }
    }
}