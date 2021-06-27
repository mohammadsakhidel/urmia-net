using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ShareDialogModel
    {
        public string RandomId { get; set; }
        public SharedObject SharedObject { get; set; }
        public UserIdentityModel OwnerIdentityModel { get; set; }
        public ActivityDetailsSectionModel ActivityDetailsSectionModel { get; set; }

        public static ShareDialogModel Create(SharedObject sharedObject, string randomId, System.Data.Objects.ObjectContext context)
        {
            var model = new ShareDialogModel();
            model.SharedObject = sharedObject;
            model.RandomId = randomId;
            model.OwnerIdentityModel = UserIdentityModel.Create(model.SharedObject.Member, 40, UserIdentityType.Thumb, "object_sender", "", "", "", context);
            model.ActivityDetailsSectionModel = ActivityDetailsSectionModel.Create(model.SharedObject, model.SharedObject, true, context);
            return model;
        }
    }
}