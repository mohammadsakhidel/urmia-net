using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Views.Models.Account
{
    public class RecoverPasswordViewModel
    {
        public PasswordRecoveryModel PasswordRecoveryModel { get; set; }

        public static RecoverPasswordViewModel Create(System.Data.Objects.ObjectContext context)
        {
            var model = new RecoverPasswordViewModel();
            var email = (HttpContext.Current.Request.QueryString["email"] != null ? HttpContext.Current.Request.QueryString["email"].ToString() : "");
            model.PasswordRecoveryModel = PasswordRecoveryModel.Create(email, context);
            return model;
        }
    }
}