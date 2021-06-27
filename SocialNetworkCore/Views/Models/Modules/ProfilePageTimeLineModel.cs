using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfilePageTimeLineModel
    {
        #region Properties:
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public List<Tuple<Activity, object>> ActivityRecords { get; set; }
        public string LastActivityDateString { get; set; }
        public bool IsThereMoreActivities { get; set; }
        public bool IsUserAllowedToPostOnWall { get; set; }
        //***** modules:
        public ViewMoreModel ViewMoreModel { get; set; }
        public AccessDeniedModel AccessDeniedModel { get; set; }
        public PostEditorModel PostEditorModel { get; set; }
        //*****
        #endregion

        #region Methods:
        public static ProfilePageTimeLineModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var ar = new ActivityRepository(context);
            context = ar.Context;
            //*****************************************
            var model = new ProfilePageTimeLineModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = (viewer != null ? viewer.Email : "");
            //--- activity records:
            var acts = ar.FindMemberActivitiesForHisWall(model.ProfileOwner.Email, viewer != null ? viewer.Email : "");
            var actRecs = new List<Tuple<Activity, object>>();
            var parameters = new Dictionary<string, object>();
            parameters.Add("AVCL", ActivityVisibilityCheckLevel.None);
            foreach (var act in acts)
            {
                var actRec = new Tuple<Activity, object>(act, ActivityModel.Create(act, parameters, context));
                actRecs.Add(actRec);
            }
            //---
            model.ActivityRecords = actRecs;
            model.LastActivityDateString = (acts.Any() ? acts.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
            model.IsThereMoreActivities = (acts.Any() ? ar.IsThereMoreWallActivities(model.ProfileOwner.Email, model.ViewerUserName, acts.Last().TimeOfAct) : false);
            model.IsUserAllowedToPostOnWall = model.ProfileOwner.IsPostingOnWallAvailableTo(model.ViewerUserName, context);
            //***** modules:
            model.ViewMoreModel = ViewMoreModel.Create("GetMoreTimelineActivities", "Members", "pp_timeline_activities", model.LastActivityDateString, model.IsThereMoreActivities, model.ProfileOwner.Email, context);
            model.AccessDeniedModel = AccessDeniedModel.Create(context);
            model.PostEditorModel = PostEditorModel.Create(Resources.Words.WriteOnHisWall, "pp_timeline_activities", model.ProfileOwner.Email, context);
            //*****
            return model;
        }
        #endregion
    }
}