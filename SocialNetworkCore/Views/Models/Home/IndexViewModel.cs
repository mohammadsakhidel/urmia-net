using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Home
{
    public class IndexViewModel
    {
        #region Properties:
        public RegisterModel RegisterModel { get; set; }
        public HomePixelAdvModel HomePixelAdvModel { get; set; }
        public HomeMemberCollectionModel HomeMemberCollectionModel { get; set; }
        #endregion

        #region Methods:
        public static IndexViewModel Create(System.Data.Objects.ObjectContext context)
        {
            var model = new IndexViewModel();
            model.RegisterModel = RegisterModel.Create(RegisterType.OnHome, "register_on_homepage", context);
            model.HomePixelAdvModel = HomePixelAdvModel.Create(context);
            model.HomeMemberCollectionModel = HomeMemberCollectionModel.Create(context);
            return model;
        }
        #endregion
    }
}