using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class EditCommentDialogModel
    {
        public Comment Comment { get; set; }
        public Member CommentOwner { get; set; }
        public UserIdentityModel CommentOwnerIdentityModel { get; set; }

        public static EditCommentDialogModel Create(Comment comment, System.Data.Objects.ObjectContext context)
        {
            var model = new EditCommentDialogModel();
            model.Comment = comment;
            model.CommentOwner = model.Comment.Member;
            model.CommentOwnerIdentityModel = UserIdentityModel.Create(model.CommentOwner, 50, UserIdentityType.Thumb, "dialog_user_id", "", "", "", context);
            return model;
        }
    }
}