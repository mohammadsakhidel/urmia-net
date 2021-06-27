using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ActivityDetailsSectionModel
    {
        #region Properties:
        public SharedObject ActedOnSharedObjectRoot { get; set; }
        public SharedObject ActedOnSharedObject { get; set; }
        public Dictionary<string, object> RootObjectInformation { get; set; }
        public bool Abbreviated { get; set; }
        #endregion

        #region Methods:
        public static ActivityDetailsSectionModel Create(SharedObject actedOnSharedObjectRoot, SharedObject actedOnSharedObject, bool abbreviated, System.Data.Objects.ObjectContext context)
        {
            var model = new ActivityDetailsSectionModel();
            model.ActedOnSharedObjectRoot = actedOnSharedObjectRoot;
            model.ActedOnSharedObject = actedOnSharedObject;
            model.Abbreviated = abbreviated;
            //--- needed info:
            var info = new Dictionary<string, object>();
            if (model.ActedOnSharedObjectRoot is Post)
            {
                var post = (Post)model.ActedOnSharedObjectRoot;
                var photos = post.Photos.ToList();
                var hasVideo = post.PostVideos.Any();
                var hasAnyAttachment = post.HasAnyAttachments;
                info["post"] = post;
                info["photoRecords"] = photos.Select(p => new Tuple<PostPhoto, string>(p, (model.ActedOnSharedObject is Sharing ? ((Sharing)model.ActedOnSharedObject).GetUrlOfDetails(p.Id) : p.UrlOfPostDetails))).ToList();
                info["photosCount"] = photos.Count;
                info["outputText"] = !String.IsNullOrEmpty(post.Text) ?
                    (post is SpecialPost ? post.Text : 
                        (hasAnyAttachment || model.Abbreviated ? 
                        TextsProcessor.RenderText(post.Text) : 
                        TextsProcessor.RenderText(post.Text, true, Digits.VideosDisplayWidth, (int)(Digits.VideosDisplayWidth * 9 / 16)))) : 
                    "";
                info["hasVideo"] = hasVideo;
                if (hasVideo)
                {
                    info["videoPlayerModel"] = VideoPlayerModel.Create(post.PostVideos.First(), model.Abbreviated ? (int?)180 : (int?)null, context);
                }
                info["tags"] = post.SharedObjectTags.ToList();
            }
            else if (model.ActedOnSharedObjectRoot is AlbumPhoto)
            {
                var alPhoto = (AlbumPhoto)model.ActedOnSharedObjectRoot;
                info["albumPhoto"] = alPhoto;
                info["urlOfDetails"] = (model.ActedOnSharedObject is Sharing ? model.ActedOnSharedObject.UrlOfDetails : alPhoto.UrlOfAlbumDetails);
            }
            else if (model.ActedOnSharedObjectRoot is Photo)
            {
                var photo = (Photo)model.ActedOnSharedObjectRoot;
                info["photo"] = photo;
                info["urlOfDetails"] = (model.ActedOnSharedObject is Sharing ? model.ActedOnSharedObject.UrlOfDetails : photo.UrlOfDetails);
            }
            //---
            model.RootObjectInformation = info;
            return model;
        }
        #endregion
    }
}