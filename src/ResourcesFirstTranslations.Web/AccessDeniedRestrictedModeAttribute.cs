using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResourcesFirstTranslations.Web
{
    public class AccessDeniedRestrictedModeAttribute : AuthorizeAttribute
    {
        public const string RestrictedModeAppSetting = "RestrictedMode";
        private static bool? _restrictedModeValue;

        public static bool IsAppRunningInRestrictedMode()
        {
            if (_restrictedModeValue.HasValue) return _restrictedModeValue.Value;

            var settingsValue = ConfigurationManager.AppSettings[RestrictedModeAppSetting];

            if (null == settingsValue)
            {
                _restrictedModeValue = false;
            }
            else
            {
                bool bSettingsValue = false;

                // we can ignore the result as we default to standard mode, and restricted mode is only for demo
                bool.TryParse(settingsValue, out bSettingsValue);
                _restrictedModeValue = bSettingsValue;
            }

            return _restrictedModeValue.Value;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // No need to process any further if we are unauthorized
            if (filterContext.Result is HttpUnauthorizedResult) return;

            if (IsAppRunningInRestrictedMode())
            {
                filterContext.Result = new HttpUnauthorizedResult("Running in restricted mode.");
            }
        }
    }
}