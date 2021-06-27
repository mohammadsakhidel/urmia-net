using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class SharingActivityModel
    {
        #region Properties:
        public SharingActivity Activity { get; set; }
        public ActivityVisibilityCheckLevel AVCL { get; set; }
        public bool IsActivityVisibleToUser { get; set; }
        public Member ActivityActor { get; set; }
        public Sharing AssociatedSharing { get; set; }
        public SharedObject SharingRootObject { get; set; }
        public Member RootObjectOwner { get; set; }
        //****** modules models:
        public UserIdentityModel ActorThumbIdentityModel { get; set; }
        public UserIdentityModel ActorNameIdentityModel { get; set; }
        public ActivityInfoModel ActivityInfoModel { get; set; }
        public ActivitySettingsModel ActivitySettingsModel { get; set; }
        public ActivityDetailsSectionModel ActivityDetailsSectionModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        //******
        #endregion

        #region Methods:
        public static SharingActivityModel Create(Activity activity, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            var model = new SharingActivityModel();
            model.Activity = (SharingActivity)activity;
            model.AVCL = (param["AVCL"] != null ? (ActivityVisibilityCheckLevel)param["AVCL"] : ActivityVisibilityCheckLevel.CheckBoth);
            model.IsActivityVisibleToUser = model.Activity.IsVisibleTo(HttpContext.Current.User.Identity.Name, model.AVCL, context);
            if (model.IsActivityVisibleToUser)
            {
                model.ActivityActor = model.Activity.Member;
                model.AssociatedSharing = model.Activity.Sharing;
                model.SharingRootObject = model.AssociatedSharing.SharedObject;
                model.RootObjectOwner = model.SharingRootObject.Member;
                //****** modules models:
                model.ActorThumbIdentityModel = UserIdentityModel.Create(model.ActivityActor, 40, UserIdentityType.Thumb, "actor", "", "", "", context);
                model.ActorNameIdentityModel = UserIdentityModel.Create(model.ActivityActor, null, UserIdentityType.FullName, "actor_name", "", "", "", context);
                model.ActivityInfoModel = ActivityInfoModel.Create(model.Activity.TimeOfAct, model.AssociatedSharing.VisibleTo, model.AssociatedSharing.UrlOfDetails, context);
                model.ActivitySettingsModel = ActivitySettingsModel.Create(model.Activity, model.ActivityActor, context);
                model.ActivityDetailsSectionModel = ActivityDetailsSectionModel.Create(model.SharingRootObject, model.AssociatedSharing, false, context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.AssociatedSharing, context);
                //******
            }
            return model;
        }
        #endregion
    }
}