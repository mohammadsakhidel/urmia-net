using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class RemoveFriendDialogModel
    {
        #region Properties:
        public Member ToBeRemovedFriend { get; set; }
        public UserIdentityModel ToBeRemovedUserIdentityModel { get; set; }
        #endregion

        #region Methods:
        public static RemoveFriendDialogModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new RemoveFriendDialogModel();
            model.ToBeRemovedFriend = member;
            model.ToBeRemovedUserIdentityModel =
                UserIdentityModel
                .Create(member, 55,
                UserIdentityType.ThumbAndFullName,
                "dialog_user_identity",
                "dialog_user_fullname",
                "", "", context);
            return model;
        }
        #endregion
    }
}