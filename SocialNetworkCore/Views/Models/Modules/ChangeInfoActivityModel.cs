using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ChangeInfoActivityModel
    {
        #region Properties:
        public ChangeInfoActivity Activity { get; set; }
        public Member ActivityActor { get; set; }
        public ActivityVisibilityCheckLevel AVCL { get; set; }
        public bool IsActivityVisibleToUser { get; set; }
        //****** modules models:
        public UserIdentityModel ActorThumbIdentityModel { get; set; }
        public UserIdentityModel ActorNameIdentityModel { get; set; }
        public ActivityInfoModel ActivityInfoModel { get; set; }
        public ActivitySettingsModel ActivitySettingsModel { get; set; }
        //******
        #endregion

        #region Methods:
        public static ChangeInfoActivityModel Create(Activity activity, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            var model = new ChangeInfoActivityModel();
            model.Activity = (ChangeInfoActivity)activity;
            model.AVCL = (param["AVCL"] != null ? (ActivityVisibilityCheckLevel)param["AVCL"] : ActivityVisibilityCheckLevel.CheckBoth);
            model.IsActivityVisibleToUser = model.Activity.IsVisibleTo(HttpContext.Current.User.Identity.Name, model.AVCL, context);
            if (model.IsActivityVisibleToUser)
            {
                model.ActivityActor = activity.Member;
                //****** modules models:
                model.ActorThumbIdentityModel = UserIdentityModel.Create(model.ActivityActor, 40, UserIdentityType.Thumb, "actor", "", "", "", context);
                model.ActorNameIdentityModel = UserIdentityModel.Create(model.ActivityActor, null, UserIdentityType.FullName, "actor_name", "", "", "", context);
                model.ActivityInfoModel = ActivityInfoModel.Create(model.Activity.TimeOfAct, null, "", context);
                model.ActivitySettingsModel = ActivitySettingsModel.Create(model.Activity, model.ActivityActor, context);
                //******
            }
            return model;
        }
        #endregion
    }
}