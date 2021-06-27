using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Views.Models.Account
{
    public class AlternativeEmailActivationViewModel
    {
        public AltEmailActivationModel AltEmailActivationModel { get; set; }

        public static AlternativeEmailActivationViewModel Create(string email, string code, System.Data.Objects.ObjectContext context)
        {
            var model = new AlternativeEmailActivationViewModel();
            model.AltEmailActivationModel = AltEmailActivationModel.Create(email, code, context);
            return model;
        }
    }
}