using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Common
{
    public static class RftClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier);
            return Int32.Parse(userId.Value);
        }

        public static List<string> GetCultures(this ClaimsPrincipal principal)
        {
            var cultures = principal.FindFirst(ClaimTypes.UserData);

            if (null == cultures)
                return new List<string>();

            return Cultures.SplitUserCulturesList(cultures.Value);
        }

        public static string GetEmailAddress(this ClaimsPrincipal principal)
        {
            var email = principal.FindFirst(ClaimTypes.Email);
            return email.Value;
        }
    }
}
