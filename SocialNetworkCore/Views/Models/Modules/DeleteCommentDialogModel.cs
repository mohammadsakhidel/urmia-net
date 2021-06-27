using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class DeleteCommentDialogModel
    {
        public Comment Comment { get; set; }
        public Member CommentOwner { get; set; }
        public UserIdentityModel CommentOwnerIdentityModel { get; set; }

        public static DeleteCommentDialogModel Create(Comment comment, System.Data.Objects.ObjectContext context)
        {
            var model = new DeleteCommentDialogModel();
            model.Comment = comment;
            model.CommentOwner = model.Comment.Member;
            model.CommentOwnerIdentityModel = UserIdentityModel.Create(model.CommentOwner, 50, UserIdentityType.Thumb, "dialog_user_identity", "", "", "", context);
            return model;
        }
    }
}