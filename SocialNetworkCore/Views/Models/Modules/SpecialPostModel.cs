using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class SpecialPostModel
    {
        #region Proprties:
        public Member PostOwner { get; set; }
        public SpecialPost SpecialPost { get; set; }
        //***** modules:
        public UserIdentityModel PostOwnerThumbIdentityModel { get; set; }
        public UserIdentityModel PostOwnerNameIdentityModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        public SpecialPostSettingsModel SpecialPostSettingsModel { get; set; }
        //*****
        #endregion

        #region Methods:
        public static SpecialPostModel Create(SpecialPost post, System.Data.Objects.ObjectContext context)
        {
            var model = new SpecialPostModel();
            model.SpecialPost = post;
            model.PostOwner = model.SpecialPost.Member;
            //***** modules:
            model.PostOwnerThumbIdentityModel = UserIdentityModel.Create(model.PostOwner, 40, UserIdentityType.Thumb, "", "", "", "", context);
            model.PostOwnerNameIdentityModel = UserIdentityModel.Create(model.PostOwner, null, UserIdentityType.FullName, "", "", "", "", context);
            model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.SpecialPost, context);
            model.SpecialPostSettingsModel = SpecialPostSettingsModel.Create(model.SpecialPost, context);
            //*****
            return model;
        }
        #endregion
    }
}