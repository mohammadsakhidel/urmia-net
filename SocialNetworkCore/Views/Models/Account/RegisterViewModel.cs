using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Account
{
    public class RegisterViewModel
    {
        public RegisterModel RegisterModel { get; set; }

        public static RegisterViewModel Create(System.Data.Objects.ObjectContext context)
        {
            var model = new RegisterViewModel();
            model.RegisterModel = RegisterModel.Create(RegisterType.OnItsPage, "", context);
            return model;
        }
    }
}