using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ChangeCoverActivityModel
    {
        #region Properties:
        public ChangeCoverActivity Activity { get; set; }
        public Member ActivityActor { get; set; }
        public ActivityVisibilityCheckLevel AVCL { get; set; }
        public ProfileCoverPhoto PCP { get; set; }
        public bool IsActivityVisibleToUser { get; set; }
        //****** modules models:
        public UserIdentityModel ActorThumbIdentityModel { get; set; }
        public UserIdentityModel ActorNameIdentityModel { get; set; }
        public ActivityInfoModel ActivityInfoModel { get; set; }
        public ActivitySettingsModel ActivitySettingsModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        //******
        #endregion

        #region Methods:
        public static ChangeCoverActivityModel Create(Activity activity, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            var model = new ChangeCoverActivityModel();
            model.Activity = (ChangeCoverActivity)activity;
            model.AVCL = (param["AVCL"] != null ? (ActivityVisibilityCheckLevel)param["AVCL"] : ActivityVisibilityCheckLevel.CheckBoth);
            model.IsActivityVisibleToUser = model.Activity.IsVisibleTo(HttpContext.Current.User.Identity.Name, model.AVCL, context);
            if (model.IsActivityVisibleToUser)
            {
                model.ActivityActor = model.Activity.Member;
                model.PCP = model.Activity.ProfileCoverPhoto;
                //****** modules models:
                model.ActorThumbIdentityModel = UserIdentityModel.Create(model.ActivityActor, 40, UserIdentityType.Thumb, "actor", "", "", "", context);
                model.ActorNameIdentityModel = UserIdentityModel.Create(model.ActivityActor, null, UserIdentityType.FullName, "actor_name", "", "", "", context);
                model.ActivityInfoModel = ActivityInfoModel.Create(model.Activity.TimeOfAct, null, "", context);
                model.ActivitySettingsModel = ActivitySettingsModel.Create(model.Activity, model.ActivityActor, context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.PCP, context);
                //******
            }
            return model;
        }
        #endregion
    }
}