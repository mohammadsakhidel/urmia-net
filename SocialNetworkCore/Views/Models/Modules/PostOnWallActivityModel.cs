using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PostOnWallActivityModel
    {
        #region Properties:
        public PostOnWallActivity Activity { get; set; }
        public ActivityVisibilityCheckLevel AVCL { get; set; }
        public bool IsActivityVisibleToUser { get; set; }
        public Member ActivityActor { get; set; }
        public Post AssociatedPost { get; set; }
        public List<PostPhoto> AssociatedPostPhotos { get; set; }
        public bool HasVideo { get; set; }
        public string AssociatedPostOutputText { get; set; }
        public IEnumerable<SharedObjectTag> AssociatedPostTags { get; set; }
        //****** modules models:
        public VideoPlayerModel VideoPlayerModel { get; set; }
        public UserIdentityModel ActorThumbIdentityModel { get; set; }
        public UserIdentityModel ActorNameIdentityModel { get; set; }
        public ActivityInfoModel ActivityInfoModel { get; set; }
        public ActivitySettingsModel ActivitySettingsModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        //******
        #endregion

        #region Methods:
        public static PostOnWallActivityModel Create(Activity activity, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            var model = new PostOnWallActivityModel();
            model.Activity = (PostOnWallActivity)activity;
            model.AVCL = (param["AVCL"] != null ? (ActivityVisibilityCheckLevel)param["AVCL"] : ActivityVisibilityCheckLevel.CheckBoth);
            model.IsActivityVisibleToUser = model.Activity.IsVisibleTo(HttpContext.Current.User.Identity.Name, model.AVCL, context) || HttpContext.Current.User.Identity.Name == model.Activity.WallOwner;
            if (model.IsActivityVisibleToUser)
            {
                model.ActivityActor = model.Activity.Member;
                model.AssociatedPost = model.Activity.Post;
                model.AssociatedPostPhotos = model.AssociatedPost.Photos.ToList();
                model.AssociatedPostOutputText = !String.IsNullOrEmpty(model.AssociatedPost.Text) ? TextsProcessor.RenderText(model.AssociatedPost.Text) : "";
                model.HasVideo = model.AssociatedPost.PostVideos.Any();
                model.AssociatedPostTags = model.AssociatedPost.SharedObjectTags.ToList();
                // output text:
                var hasAnyAttachment = model.AssociatedPost.HasAnyAttachments;
                model.AssociatedPostOutputText =
                    !String.IsNullOrEmpty(model.AssociatedPost.Text) ?
                    (hasAnyAttachment ?
                    TextsProcessor.RenderText(model.AssociatedPost.Text) :
                    TextsProcessor.RenderText(model.AssociatedPost.Text, true, Digits.VideosDisplayWidth, (int)(Digits.VideosDisplayWidth * 9 / 16))) :
                    "";
                if (model.HasVideo)
                {
                    var vid = model.AssociatedPost.PostVideos.First();
                    model.VideoPlayerModel = VideoPlayerModel.Create(vid, null, context);
                }
                //****** modules models:
                model.ActorThumbIdentityModel = UserIdentityModel.Create(model.ActivityActor, 40, UserIdentityType.Thumb, "actor", "", "", "", context);
                model.ActorNameIdentityModel = UserIdentityModel.Create(model.ActivityActor, null, UserIdentityType.FullName, "actor_name", "", "", "", context);
                model.ActivityInfoModel = ActivityInfoModel.Create(model.Activity.TimeOfAct, model.AssociatedPost.VisibleTo, model.AssociatedPost.UrlOfDetails, context);
                model.ActivitySettingsModel = ActivitySettingsModel.Create(model.Activity, model.ActivityActor, context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.AssociatedPost, context);
                //******
            }
            return model;
        }
        #endregion
    }
}