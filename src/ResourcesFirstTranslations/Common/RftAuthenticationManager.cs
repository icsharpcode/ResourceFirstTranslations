using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ResourcesFirstTranslations.Common
{
    public static class RftAuthenticationManager
    {
        public const string AdministratorRole = "AdminRole";

        public static ClaimsIdentity CreateIdentity(Data.User user)
        {
            var claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.EmailAddress));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            claimsIdentity.AddClaim(
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", 
                        "ASP.NET Identity", 
                        "http://www.w3.org/2001/XMLSchema#string"));

            if (user.IsAdmin)
            {
                var adminClaim = new Claim(ClaimTypes.Role, AdministratorRole); // , null, ClaimIssuerName);
                claimsIdentity.AddClaim(adminClaim);
            }

            // Cannot assign null-value to new claim
            if (null != user.Cultures)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, user.Cultures));
            }

            return claimsIdentity;
        }
    }
}
