using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfilePageFriendsModel
    {
        #region Properties:
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public bool AreFriendsVisibleToUser { get; set; }
        public List<UserIdentityModel> FriendModels { get; set; }
        public int TotalFriendsCount { get; set; }
        public bool IsThereMoreFriends { get; set; }
        //***** modules:
        public AccessDeniedModel AccessDeniedModel { get; set; }
        public ViewMoreModel ViewMoreModel { get; set; }
        //*****
        #endregion

        #region Methods:
        public static ProfilePageFriendsModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //********************************
            var model = new ProfilePageFriendsModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = (viewer != null ? viewer.Email : "");
            model.AreFriendsVisibleToUser = model.ProfileOwner.IsFriendsVisibleTo(model.ViewerUserName, context);
            if (model.AreFriendsVisibleToUser)
            {
                //--- friend models:
                var friends = mr.FindPagedFriends(model.ProfileOwner.Email, 0).ToList();
                var fModels = new List<UserIdentityModel>();
                foreach (var f in friends)
                {
                    var fModel = UserIdentityModel.Create(f, 70, UserIdentityType.ThumbAndFullName, "", "pp_friends_frnd_fullname", "", "", context);
                    fModels.Add(fModel);
                }
                //---
                model.FriendModels = fModels;
                model.TotalFriendsCount = mr.GetFriendsCount(model.ProfileOwner.Email);
                model.IsThereMoreFriends = model.TotalFriendsCount > Digits.ListingItemsPageSize;
                //**** modules:
                model.ViewMoreModel = ViewMoreModel.Create("GetMoreFriends", "Members", "pp_friends_list", "", model.IsThereMoreFriends, model.ProfileOwner.Email, context);
                //****
            }
            else
            {
                model.AccessDeniedModel = AccessDeniedModel.Create(context);
            }
            return model;
        }
        #endregion
    }
}