using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ActivityModel
    {
        public Activity Activity { get; set; }
        public ActivityVisibilityCheckLevel? VisibilityCheckLevel { get; set; }

        #region Methods:
        public static object Create(Activity act, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            object model = null;
            switch (act.ActivityType)
            {
                case ActivityType.ChangeCoverActivity:
                    model = ChangeCoverActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.ChangeInfoActivity:
                    model = ChangeInfoActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.ChangePPActivity:
                    model = ChangePPActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.CommentActivity:
                    model = CommentActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.EducationActivity:
                    model = EducationActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.LikeActivity:
                    model = LikeActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.PhotoActivity:
                    model = PhotoActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.PostActivity:
                    model = PostActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.PostOnWallActivity:
                    model = PostOnWallActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.SharingActivity:
                    model = SharingActivityModel.Create(act, param, context);//OK
                    break;
                case ActivityType.SkillActivity:
                    model = SkillActivityModel.Create(act, param, context);//OK
                    break;
            }
            return model;
        }
        #endregion
    }
}