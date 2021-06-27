using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class CommentModel
    {
        #region Properties:
        public Comment Comment { get; set; }
        public Member Commenter { get; set; }
        public SharedObject AssociatedSharedObject { get; set; }
        public int LikesCount { get; set; }
        public bool HasUserLikedComment { get; set; }
        public bool CanUserDeleteComment { get; set; }
        //******** modules:
        public UserIdentityModel CommenterThumbUserIdentityModel { get; set; }
        public UserIdentityModel CommenterNameUserIdentityModel { get; set; }
        //********
        #endregion

        #region Methods:
        public static CommentModel Create(Comment comment, System.Data.Objects.ObjectContext context)
        {
            var cr = new CommentsRepository(context);
            context = cr.Context;
            ////////////////////////////////
            var model = new CommentModel();
            model.Comment = comment;
            model.Commenter = comment.Member;
            model.CommenterThumbUserIdentityModel = UserIdentityModel.Create(model.Commenter, 33, UserIdentityType.Thumb, "com_user_iden", "", "", "", context);
            model.CommenterNameUserIdentityModel = UserIdentityModel.Create(model.Commenter, null, UserIdentityType.FullName, "", "", "", "", context);
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                model.AssociatedSharedObject = comment.SharedObject;
                model.LikesCount = model.Comment.Likes.Count();
                model.HasUserLikedComment = cr.HasLiked(model.Comment.Id, HttpContext.Current.User.Identity.Name);
                model.CanUserDeleteComment = HttpContext.Current.User.Identity.Name == model.Comment.MemberId ||
                    HttpContext.Current.User.Identity.Name == model.AssociatedSharedObject.MemberId ||
                    Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_comments);
            }
            return model;
        }
        #endregion
    }
}