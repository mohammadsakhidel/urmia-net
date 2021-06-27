using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Modules
{
    public class PasswordRecoveryModel
    {
        public string Email { get; set; }

        public static PasswordRecoveryModel Create(string email, System.Data.Objects.ObjectContext context)
        {
            var model = new PasswordRecoveryModel();
            model.Email = email;
            return model;
        }
    }
}