using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class NotificationModel
    {
        #region Properties:
        public Notification Notification { get; set; }
        public Member Actor { get; set; }
        public UserIdentityModel ActorIdentityModel { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public bool HasThumb { get; set; }
        public string NotificationThumbImageUrl { get; set; }
        public string NotificationThumbHrefUrl { get; set; }
        // for like notifications:
        public Like AssociatedLike { get; set; }
        public object AssociatedLikedItem { get; set; } //can be sharedobject or comment
        public SharedObject AssociatedLikedItemObject { get; set; } // for comment like this is the object commented on.
        // for comment and also comment notifications:
        public Comment AssociatedComment { get; set; }
        public SharedObject AssociatedCommentedObject { get; set; }
        // for sharing notifications:
        public Sharing AssociatedSharing { get; set; }
        // for post on wall notifications:
        public Post AssociatedPost { get; set; }
        #endregion

        public static NotificationModel Create(Notification not, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            SharedObject thumbObject = null;
            var notModel = new NotificationModel();
            if (not is LikeNotification)
            {
                notModel.AssociatedLike = ((LikeNotification)not).Like;
                notModel.Actor = notModel.AssociatedLike.Member;
                notModel.AssociatedLikedItem = (notModel.AssociatedLike is SharedObjectLike ? (object)(((SharedObjectLike)notModel.AssociatedLike).SharedObject) : (notModel.AssociatedLike is CommentLike ? (object)(((CommentLike)notModel.AssociatedLike).Comment) : null));
                if (notModel.AssociatedLike is CommentLike)
                {
                    notModel.AssociatedLikedItemObject = ((Comment)notModel.AssociatedLikedItem).SharedObject;
                    thumbObject = notModel.AssociatedLikedItemObject;
                }
                else if (notModel.AssociatedLike is SharedObjectLike)
                {
                    thumbObject = (SharedObject)notModel.AssociatedLikedItem;
                }
            }
            else if (not is CommentNotification)
            {
                notModel.AssociatedComment = ((CommentNotification)not).Comment;
                notModel.AssociatedCommentedObject = notModel.AssociatedComment.SharedObject;
                notModel.Actor = notModel.AssociatedComment.Member;
                thumbObject = notModel.AssociatedCommentedObject;
            }
            else if (not is AlsoCommentNotification)
            {
                notModel.AssociatedComment = ((AlsoCommentNotification)not).Comment;
                notModel.AssociatedCommentedObject = notModel.AssociatedComment.SharedObject;
                notModel.Actor = notModel.AssociatedComment.Member;
                thumbObject = notModel.AssociatedCommentedObject;
            }
            else if (not is ShareNotification)
            {
                notModel.AssociatedSharing = ((ShareNotification)not).Sharing;
                notModel.Actor = notModel.AssociatedSharing.Member;
                thumbObject = notModel.AssociatedSharing;
            }
            else if (not is PostOnWallNotification)
            {
                notModel.AssociatedPost = ((PostOnWallNotification)not).Post;
                notModel.Actor = notModel.AssociatedPost.Member;
                thumbObject = notModel.AssociatedPost;
            }
            else if (not is AcceptRequestNotification)
            {
                notModel.Actor = mr.Get(((AcceptRequestNotification)not).AcceptedBy);
            }
            // common fields:
            notModel.Notification = not;
            notModel.NotificationStatus = (NotificationStatus)not.Status;
            notModel.ActorIdentityModel = UserIdentityModel
                .Create(notModel.Actor, 40,
                UserIdentityType.Thumb,
                "", "", "", "", context);
            notModel.NotificationThumbHrefUrl = thumbObject != null ? thumbObject.UrlOfDetails : "";
            notModel.NotificationThumbImageUrl = thumbObject != null ? thumbObject.GetSmallThumbUrl(context) : "";
            notModel.HasThumb = thumbObject != null && !String.IsNullOrEmpty(notModel.NotificationThumbImageUrl);
            /////////////////
            return notModel;
        }
    }
}