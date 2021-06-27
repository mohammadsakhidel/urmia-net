using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class HomePageModel
    {
        #region Properties:
        public List<Tuple<Activity, object>> ActivityRecords { get; set; }
        public string LastActivityDateString { get; set; }
        public bool IsThereMoreActivities { get; set; }
        public bool IsDeservedForWelcome { get; set; }
        public List<SpecialPostModel> WelcomePosts { get; set; }
        public List<SpecialPostModel> PinnedPosts { get; set; }
        //*************** modules models
        public PostEditorModel PostEditorModel { get; set; }
        public ViewMoreModel ViewMoreModel { get; set; }
        public HomePageSuggestionsModel HomePageSuggestionsModel { get; set; }
        //***************
        #endregion

        #region Methods:
        public static HomePageModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            var ar = new ActivityRepository(context);
            var pr = new PostsRepository(context);
            //activities:
            var activities = ar.FindNewsFeedActivities(HttpContext.Current.User.Identity.Name).ToList();
            var actRecs = new List<Tuple<Activity, object>>();
            var param = new Dictionary<string, object>();
            param.Add("AVCL", ActivityVisibilityCheckLevel.None);
            foreach (var act in activities)
            {
                var actModel = ActivityModel.Create(act, param , context);
                var actRec = new Tuple<Activity, object>(act, actModel);
                actRecs.Add(actRec);
            }
            ///////////////////////////////////
            var model = new HomePageModel();
            model.ActivityRecords = actRecs;
            model.LastActivityDateString = (activities.Any() ? activities.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
            model.IsThereMoreActivities = (activities.Any() ? ar.IsThereMoreNewsFeedActivities(HttpContext.Current.User.Identity.Name, activities.Last().TimeOfAct) : false);
            model.IsDeservedForWelcome = mr.IsDeservedForWelcome(HttpContext.Current.User.Identity.Name) || !activities.Any();
            //---- welcome post models:
            var wpModels = new List<SpecialPostModel>();
            if (model.IsDeservedForWelcome)
            {
                var wPosts = pr.FindSpecialPosts(SpecialPostShowMethod.Welcome).ToList();
                foreach (var wp in wPosts)
                {
                    var wpModel = SpecialPostModel.Create(wp, context);
                    wpModels.Add(wpModel);
                }
            }
            //----
            model.WelcomePosts = wpModels;
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
            model.PostEditorModel = PostEditorModel.Create("", "hp_activities", "", context);
            model.ViewMoreModel = ViewMoreModel.Create("GetMoreActivities", "HomePage", "hp_activities", model.LastActivityDateString, model.IsThereMoreActivities, "", context);
            model.HomePageSuggestionsModel = HomePageSuggestionsModel.Create(context);
            //**********
            return model;
        }
        #endregion
    }
}