using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Views.Models.Account
{
    public class ActivationViewModel
    {
        #region Modules models
        public AccountActivationModel AccountActivationModel { get; set; }
        #endregion

        #region methods:
        public static ActivationViewModel Create(System.Data.Objects.ObjectContext context)
        {
            var model = new ActivationViewModel();
            model.AccountActivationModel = AccountActivationModel.Create(context);
            return model;
        }
        #endregion
    }
}