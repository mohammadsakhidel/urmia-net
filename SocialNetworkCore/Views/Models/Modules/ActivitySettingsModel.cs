using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ActivitySettingsModel
    {
        #region Properties:
        public Activity Activity { get; set; }
        public Member Actor { get; set; }
        public bool IsDeleteEnabled { get; set; }
        #endregion

        #region Methods:
        public static ActivitySettingsModel Create(Activity act, Member actor, System.Data.Objects.ObjectContext context)
        {
            var model = new ActivitySettingsModel();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                model.Activity = act;
                model.Actor = actor;
                model.IsDeleteEnabled = HttpContext.Current.User.Identity.Name == model.Activity.MemberId || Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_acts) || (model.Activity.ActivityType == ActivityType.PostOnWallActivity && ((PostOnWallActivity)model.Activity).WallOwner == HttpContext.Current.User.Identity.Name);
            }
            return model;
        }
        #endregion
    }
}