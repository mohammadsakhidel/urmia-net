using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class SharingDetailsModel
    {
        public Sharing Sharing { get; set; }
        public Member SharingOwner { get; set; }
        public int DefaultPhotoId { get; set; }
        public SharedObject SharingRootObject { get; set; }
        public bool IsSharingVisibleToUser { get; set; }
        public List<Photo> Photos { get; set; }
        public string OutputText { get; set; }
        public bool HasVideo { get; set; }
        //***** modules
        public UserIdentityModel SharingOwnerIdentityModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        public AccessDeniedModel AccessDeniedModel { get; set; }
        public PhotoCollectionModel PhotoCollectionModel { get; set; }
        public VideoPlayerModel VideoPlayerModel { get; set; }
        //****

        public static SharingDetailsModel Create(Sharing sharing, int defPhotoId, System.Data.Objects.ObjectContext context)
        {
            var model = new SharingDetailsModel();
            model.Sharing = sharing;
            model.IsSharingVisibleToUser = model.Sharing.IsVisibleTo(HttpContext.Current.User.Identity.Name, context);
            if (model.IsSharingVisibleToUser)
            {
                model.SharingOwner = model.Sharing.Member;
                model.SharingRootObject = sharing.SharedObject;
                model.DefaultPhotoId = defPhotoId;
                model.Photos = (model.SharingRootObject is Post ?
                    ((Post)model.SharingRootObject).Photos.Cast<Photo>() :
                    (model.SharingRootObject is Photo ? new List<Photo> { (Photo)model.SharingRootObject } : new List<Photo>())).ToList();
                //--- output text:
                var outputText = "";
                if (model.SharingRootObject is Post)
                {
                    var post = (Post)model.SharingRootObject;
                    outputText = (post is SpecialPost ? 
                        post.Text : 
                        (post.HasAnyAttachments ? 
                            TextsProcessor.RenderText(post.Text) :
                            TextsProcessor.RenderText(post.Text, true, Digits.VideosFixedWidth, (int)(Digits.VideosFixedWidth / 16 * 9))));
                    model.HasVideo = post.PostVideos.Any();
                    if (model.HasVideo)
                    {
                        var vid = post.PostVideos.First();
                        model.VideoPlayerModel = VideoPlayerModel.Create(vid, vid.Width, context);
                    }
                }
                else if (model.SharingRootObject is AlbumPhoto)
                {
                    var ph = (AlbumPhoto)model.SharingRootObject;
                    outputText = !String.IsNullOrEmpty(ph.Description) ? TextsProcessor.RenderText(ph.Description) : "";
                    model.HasVideo = false;
                }
                //---
                model.OutputText = outputText;
                //****** modules
                model.SharingOwnerIdentityModel = UserIdentityModel.Create(model.SharingOwner, 45, UserIdentityType.ThumbAndFullName, "", "obj_det_fullname", "", "", context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.Sharing, context);
                model.PhotoCollectionModel = PhotoCollectionModel.Create("GetSharedObjectDetails", "Objects", model.Sharing.Id, model.Photos, model.DefaultPhotoId, "obj_details_parent", context);
                //******
            }
            else
            {
                //****** modules
                model.AccessDeniedModel = AccessDeniedModel.Create(context);
                //******
            }
            return model;
        }
    }
}