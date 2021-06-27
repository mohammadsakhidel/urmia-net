using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class FriendsPPSectionModel
    {
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public bool IsFriendsSectionVisibleToUser { get; set; }
        public List<Member> SomeFriends { get; set; }
        public int FriendsCount { get; set; }

        public static FriendsPPSectionModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //********************
            var model = new FriendsPPSectionModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = (viewer != null ? viewer.Email : "");
            model.IsFriendsSectionVisibleToUser = model.ProfileOwner.IsFriendsVisibleTo(model.ViewerUserName, context);
            if (model.IsFriendsSectionVisibleToUser)
            {
                model.SomeFriends = mr.RetrieveSomeFriends(model.ProfileOwner.Email, Digits.ProfilePageFriendsCount).OrderBy(f => Guid.NewGuid()).ToList();
                model.FriendsCount = mr.GetFriendsCount(model.ProfileOwner.Email);
            }
            return model;
        }
    }
}