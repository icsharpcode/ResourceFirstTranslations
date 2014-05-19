using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Areas.Administration.Models
{
    public class TranslatorModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Cultures { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public Data.User ToUser()
        {
            var user = ToNewUser();

            user.Id = this.Id;
            return user;
        }

        public Data.User ToNewUser()
        {
            return new Data.User
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                EmailAddress = this.EmailAddress,
                Cultures = this.Cultures,
                IsActive = this.IsActive,
                IsAdmin = this.IsAdmin
            };
        }
    }
}