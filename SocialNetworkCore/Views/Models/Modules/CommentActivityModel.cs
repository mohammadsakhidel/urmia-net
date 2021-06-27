using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class CommentActivityModel
    {
        #region Properties:
        public CommentActivity Activity { get; set; }
        public Member ActivityActor { get; set; }
        public bool IsActivityVisibleToUser { get; set; }
        public Comment AddedComment { get; set; }
        public SharedObject CommentedSharedbject { get; set; }
        public SharedObject CommentedSharedObjectRoot { get; set; }
        public Member CommentedSharedbjectOwner { get; set; }
        public ActivityVisibilityCheckLevel AVCL { get; set; }
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
        public static CommentActivityModel Create(Activity activity, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            var model = new CommentActivityModel();
            model.Activity = (CommentActivity)activity;
            model.AVCL = (param["AVCL"] != null ? (ActivityVisibilityCheckLevel)param["AVCL"] : ActivityVisibilityCheckLevel.CheckBoth);
            model.IsActivityVisibleToUser = model.Activity.IsVisibleTo(HttpContext.Current.User.Identity.Name, model.AVCL, context);
            if (model.IsActivityVisibleToUser)
            {
                model.ActivityActor = model.Activity.Member;
                model.AddedComment = model.Activity.Comment;
                model.CommentedSharedbject = model.AddedComment.SharedObject;
                model.CommentedSharedbjectOwner = model.CommentedSharedbject.Member;
                model.CommentedSharedObjectRoot = model.CommentedSharedbject is Sharing ? ((Sharing)model.CommentedSharedbject).SharedObject : model.CommentedSharedbject;
                //****** modules models:
                model.ActorThumbIdentityModel = UserIdentityModel.Create(model.ActivityActor, 40, UserIdentityType.Thumb, "actor", "", "", "", context);
                model.ActorNameIdentityModel = UserIdentityModel.Create(model.ActivityActor, null, UserIdentityType.FullName, "actor_name", "", "", "", context);
                model.ActivityInfoModel = ActivityInfoModel.Create(model.Activity.TimeOfAct, model.CommentedSharedbject.VisibleTo, model.CommentedSharedbject.UrlOfDetails, context);
                model.ActivitySettingsModel = ActivitySettingsModel.Create(model.Activity, model.ActivityActor, context);
                model.ActivityDetailsSectionModel = ActivityDetailsSectionModel.Create(model.CommentedSharedObjectRoot, model.CommentedSharedbject, false, context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.CommentedSharedbject, context);
                //******
            }
            return model;
        }
        #endregion
    }
}