using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PostDetailsModel
    {
        public Post Post { get; set; }
        public Member PostOwner { get; set; }
        public bool IsPostVisibleForUser { get; set; }
        public int DefaultPhotoId { get; set; }
        public string OutputText { get; set; }
        public List<PostPhoto> PostPhotos { get; set; }
        public bool HasVideo { get; set; }
        public IEnumerable<SharedObjectTag> PostTags { get; set; }
        //**** modules:
        public VideoPlayerModel VideoPlayerModel { get; set; }
        public UserIdentityModel PostOwnerIdentityModel { get; set; }
        public PhotoCollectionModel PhotoCollectionModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        public AccessDeniedModel AccessDeniedModel { get; set; }
        //****

        public static PostDetailsModel Create(Post post, int defPhotoId, System.Data.Objects.ObjectContext context)
        {
            var model = new PostDetailsModel();
            model.Post = post;
            model.IsPostVisibleForUser = model.Post.IsVisibleTo(HttpContext.Current.User.Identity.Name, context);
            if (model.IsPostVisibleForUser)
            {
                model.PostOwner = model.Post.Member;
                model.PostTags = model.Post.SharedObjectTags.ToList();
                model.DefaultPhotoId = defPhotoId;
                model.PostPhotos = model.Post.Photos.ToList();
                model.HasVideo = model.Post.PostVideos.Any();
                var hasAnyAttachment = model.Post.HasAnyAttachments;
                model.OutputText = (model.Post is SpecialPost ? 
                    model.Post.Text :
                    (hasAnyAttachment ? 
                        TextsProcessor.RenderText(model.Post.Text) : 
                        TextsProcessor.RenderText(model.Post.Text, true, Digits.VideosFixedWidth, (int)(Digits.VideosFixedWidth * 9 / 16))));
                //***** modules:
                model.PostOwnerIdentityModel = UserIdentityModel.Create(model.PostOwner, 45, UserIdentityType.ThumbAndFullName, "", "obj_det_fullname", "", "", context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.Post, context);
                if (model.PostPhotos.Any())
                {
                    model.PhotoCollectionModel = PhotoCollectionModel.Create("GetSharedObjectDetails", "Objects", model.Post.Id, model.PostPhotos.Cast<Photo>().ToList(), model.DefaultPhotoId, "obj_details_parent", context);
                }
                else if (model.HasVideo)
                {
                    var vid = model.Post.PostVideos.First();
                    model.VideoPlayerModel = VideoPlayerModel.Create(vid, vid.Width, context);
                }
                //*****
            }
            else
            {
                model.AccessDeniedModel = AccessDeniedModel.Create(context);
            }
            return model;
        }
    }
}