using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PublicSharesModel
    {
        #region Properties:
        public List<Tuple<Activity, object>> ActivityRecords { get; set; }
        public string LastActivityDateString { get; set; }
        public bool IsThereMoreActivities { get; set; }
        public List<SpecialPostModel> PinnedPosts { get; set; }
        //*************** modules models
        public PostEditorModel PostEditorModel { get; set; }
        public ViewMoreModel ViewMoreModel { get; set; }
        public HomePageSuggestionsModel HomePageSuggestionsModel { get; set; }
        //***************
        #endregion

        #region Methods:
        public static PublicSharesModel Create(Member member, string tag, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            var ar = new ActivityRepository(context);
            var pr = new PostsRepository(context);
            //activities:
            var activities = ar.FindPublicActivities(HttpContext.Current.User.Identity.Name, tag);
            var actRecs = new List<Tuple<Activity, object>>();
            var param = new Dictionary<string, object>();
            param.Add("AVCL", ActivityVisibilityCheckLevel.None);
            foreach (var act in activities)
            {
                var actModel = ActivityModel.Create(act, param, context);
                var actRec = new Tuple<Activity, object>(act, actModel);
                actRecs.Add(actRec);
            }
            //***********************************
            var model = new PublicSharesModel();
            model.ActivityRecords = actRecs;
            model.LastActivityDateString = (activities.Any() ? activities.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
            model.IsThereMoreActivities = (activities.Any() ? ar.IsThereMorePublicActivities(HttpContext.Current.User.Identity.Name, tag, activities.Last().TimeOfAct) : false);
            //---- pinned post models:
            var pinnedPosts = pr.FindSpecialPosts(SpecialPostShowMethod.Pinned).ToList();
            var ppModels = new List<SpecialPostModel>();
            foreach (var pp in pinnedPosts)
            {
                var ppModel = SpecialPostModel.Create(pp, context);
                ppModels.Add(ppModel);
            }
            //----
            model.PinnedPosts = ppModels;
            //********** modules models:
            model.PostEditorModel = PostEditorModel.Create("", "public_activities", "", context);
            model.ViewMoreModel = ViewMoreModel.Create("GetMorePublicActivities", "HomePage", "public_activities", model.LastActivityDateString, model.IsThereMoreActivities, (!string.IsNullOrEmpty(tag) ? string.Format("tag={0}", tag) : ""), context);
            model.HomePageSuggestionsModel = HomePageSuggestionsModel.Create(context);
            //**********
            return model;
        }
        #endregion
    }
}