using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class LikeActivityModel
    {
        #region Properties:
        public LikeActivity Activity { get; set; }
        public ActivityVisibilityCheckLevel AVCL { get; set; }
        public bool IsActivityVisibleToUser { get; set; }
        public Member ActivityActor { get; set; }
        public Like AddedLike { get; set; }
        public object LikedItem { get; set; }
        public Member LikedItemOwner { get; set; }
        public SharedObject LikeRelatedSharedObject { get; set; }
        public SharedObject LikeRelatedSharedObjectRoot { get; set; }
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
        public static LikeActivityModel Create(Activity activity, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            var model = new LikeActivityModel();
            model.Activity = (LikeActivity)activity;
            model.AVCL = (param["AVCL"] != null ? (ActivityVisibilityCheckLevel)param["AVCL"] : ActivityVisibilityCheckLevel.CheckBoth);
            model.IsActivityVisibleToUser = model.Activity.IsVisibleTo(HttpContext.Current.User.Identity.Name, model.AVCL, context);
            if (model.IsActivityVisibleToUser)
            {
                model.ActivityActor = model.Activity.Member;
                model.AddedLike = model.Activity.Like;
                model.LikedItem = model.AddedLike is SharedObjectLike ? 
                    (object)(((SharedObjectLike)model.AddedLike).SharedObject) :
                    (object)(model.AddedLike is CommentLike ? ((CommentLike)model.AddedLike).Comment : null);
                model.LikedItemOwner = (model.LikedItem is Comment ? ((Comment)model.LikedItem).Member : ((SharedObject)model.LikedItem).Member);
                model.LikeRelatedSharedObject = model.LikedItem is SharedObject ? 
                    (SharedObject)model.LikedItem :
                    (model.LikedItem is Comment ? ((Comment)model.LikedItem).SharedObject : null);
                model.LikeRelatedSharedObjectRoot = model.LikeRelatedSharedObject is Sharing ? 
                    ((Sharing)model.LikeRelatedSharedObject).SharedObject : 
                    model.LikeRelatedSharedObject;
                //****** modules models:
                model.ActorThumbIdentityModel = UserIdentityModel.Create(model.ActivityActor, 40, UserIdentityType.Thumb, "actor", "", "", "", context);
                model.ActorNameIdentityModel = UserIdentityModel.Create(model.ActivityActor, null, UserIdentityType.FullName, "actor_name", "", "", "", context);
                model.ActivityInfoModel = ActivityInfoModel.Create(model.Activity.TimeOfAct, model.LikeRelatedSharedObject.VisibleTo, model.LikeRelatedSharedObject.UrlOfDetails, context);
                model.ActivitySettingsModel = ActivitySettingsModel.Create(model.Activity, model.ActivityActor, context);
                model.ActivityDetailsSectionModel = ActivityDetailsSectionModel.Create(model.LikeRelatedSharedObjectRoot, model.LikeRelatedSharedObject, false, context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.LikeRelatedSharedObject, context);
                //******
            }
            return model;
        }
        #endregion
    }
}