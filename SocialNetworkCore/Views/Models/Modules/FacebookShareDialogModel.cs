using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class FacebookShareDialogModel
    {
        public SharedObject SharedObject { get; set; }
        public UserIdentityModel OwnerIdentityModel { get; set; }
        public ActivityDetailsSectionModel ActivityDetailsSectionModel { get; set; }

        public static FacebookShareDialogModel Create(SharedObject obj, System.Data.Objects.ObjectContext context)
        {
            var model = new FacebookShareDialogModel();
            model.SharedObject = obj;
            model.OwnerIdentityModel = UserIdentityModel.Create(model.SharedObject.Member, 40, UserIdentityType.Thumb, "object_sender", "", "", "", context);
            model.ActivityDetailsSectionModel = ActivityDetailsSectionModel.Create(model.SharedObject, model.SharedObject, true, context);
            return model;
        }
    }
}