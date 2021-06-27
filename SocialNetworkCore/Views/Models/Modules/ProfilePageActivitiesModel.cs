using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfilePageActivitiesModel
    {
        #region Properties:
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public List<Tuple<Activity, object>> ActivityRecords { get; set; }
        public string LastActivityDateString { get; set; }
        public bool IsThereMoreActivities { get; set; }
        public ViewMoreModel ViewMoreModel { get; set; }
        #endregion

        #region Methods:
        public static ProfilePageActivitiesModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var ar = new ActivityRepository(context);
            context = ar.Context;
            //****************************************
            var model = new ProfilePageActivitiesModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = (viewer != null ? viewer.Email : "");
            //--- activity records:
            var param = new Dictionary<string, object>();
            param.Add("AVCL", ActivityVisibilityCheckLevel.None);
            var acts = ar.FindProfilePageActivitis(model.ProfileOwner.Email, model.ViewerUserName);
            var actRecords = new List<Tuple<Activity, object>>();
            foreach (var act in acts)
            {
                var actRec = new Tuple<Activity, object>(act, ActivityModel.Create(act, param, context));
                actRecords.Add(actRec);
            }
            //---
            model.ActivityRecords = actRecords;
            model.LastActivityDateString = (acts.Any() ? acts.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
            model.IsThereMoreActivities = (acts.Any() ? ar.IsThereMoreProfilePageActivitis(model.ProfileOwner.Email, model.ViewerUserName, acts.Last().TimeOfAct) : false);
            model.ViewMoreModel = ViewMoreModel.Create("GetMoreProfilePageActivities", "Members", "pp_activities_acts", model.LastActivityDateString, model.IsThereMoreActivities, model.ProfileOwner.Email, context);
            return model;
        }
        #endregion
    }
}