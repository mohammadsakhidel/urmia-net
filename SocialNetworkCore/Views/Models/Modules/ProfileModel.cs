using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfileModel
    {
        #region Properties:
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public string Content { get; set; }
        public AlbumPhoto ProfileOwnerPP { get; set; }
        public bool HasPP { get; set; }
        public bool IsProfileOwnerOnline { get; set; }
        public bool IsProfileOwnerActive { get; set; }
        public bool ShowAdminActions { get; set; }
        public bool IsUserAllowedToBeginChat { get; set; }
        public bool IsUserAllowedToSendMessage { get; set; }
        public bool IsUserAllowedToSendFriendshipRequest { get; set; }
        public bool AreViewerAndProfileOwnerFriends { get; set; }
        public bool HasPendingFriendshipRequest { get; set; }
        public FriendshipRequest PendingFriendshipRequest { get; set; }
        public string LastActivityTimeText { get; set; }
        public bool HasProfileCover { get; set; }
        public string UrlOfCover { get; set; }
        public bool IsFollowingAllowed { get; set; }
        public bool IsCurrentlyFallowing { get; set; }
        public int FollowersCount { get; set; }
        //************ modules
        public ProfilePageAdminActionsModel ProfilePageAdminActionsModel { get; set; }//OK
        public ConsiderationsPPSectionModel ConsiderationsPPSectionModel { get; set; }//OK
        public ProfilePagePixelAdvModel ProfilePagePixelAdvModel { get; set; }//OK
        public InfoPPSectionModel InfoPPSectionModel { get; set; }//OK
        public FriendsPPSectionModel FriendsPPSectionModel { get; set; }//OK
        public ProfilePageTimeLineModel ProfilePageTimeLineModel { get; set; }//OK
        public ProfilePageFriendsModel ProfilePageFriendsModel { get; set; }//OK
        public ProfilePageInfoModel ProfilePageInfoModel { get; set; }//OK
        public ProfilePagePhotosModel ProfilePagePhotosModel { get; set; }//OK
        public ProfilePageActivitiesModel ProfilePageActivitiesModel { get; set; }//OK
        //************
        #endregion

        #region Methods:
        public static ProfileModel Create(Member profileOwner, string content, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            //**************************************
            var model = new ProfileModel();
            model.ProfileOwner = profileOwner;
            model.HasProfileCover = model.ProfileOwner.HasProfileCover(context);
            model.UrlOfCover = model.HasProfileCover ? model.ProfileOwner.UrlOfCover(context) : "";
            model.Content = content.ToLower();
            model.Viewer = (HttpContext.Current.User.Identity.IsAuthenticated ? mr.Get(HttpContext.Current.User.Identity.Name) : null);
            model.ViewerUserName = model.Viewer != null ? model.Viewer.Email : "";
            model.HasPP = !String.IsNullOrEmpty(model.ProfileOwner.ProfilePhoto);
            model.ProfileOwnerPP = (model.HasPP ? mr.FindProfilePhoto(model.ProfileOwner.Email) : null);
            model.IsProfileOwnerOnline = OnlinesHelper.IsOnline(model.ProfileOwner.Email);
            model.IsProfileOwnerActive = model.ProfileOwner.Status == MemberStatus.Active;
            model.ShowAdminActions = Member.IsInAdminGroup(model.ViewerUserName, ManagerAccessLevels.core_user_mngr) && 
                !System.Web.Security.Roles.IsUserInRole(model.ProfileOwner.Email, MyRoles.Administrator) &&
                model.ProfileOwner.Email != model.ViewerUserName;
            model.IsUserAllowedToBeginChat = model.ProfileOwner.IsAllowedToBeginChat(model.ViewerUserName, context);
            model.IsUserAllowedToSendMessage = model.ProfileOwner.IsAllowedToSendMessage(model.ViewerUserName, context);
            model.IsUserAllowedToSendFriendshipRequest = model.ProfileOwner.IsAllowedToSendFriendshipRequest(model.ViewerUserName, context);

            model.AreViewerAndProfileOwnerFriends = mr.AreFriends(model.ViewerUserName, model.ProfileOwner.Email);
            if (!model.AreViewerAndProfileOwnerFriends && model.IsUserAllowedToSendFriendshipRequest)
            {
                var pendingReq = mr.GetPendingFriendshipRequest(model.ViewerUserName, model.ProfileOwner.Email);
                model.HasPendingFriendshipRequest = pendingReq != null;
                model.PendingFriendshipRequest = pendingReq;
            }

            model.LastActivityTimeText = DateHelper.DateToShortText(model.ProfileOwner.GetLastActivationDate());
            //---- following:
            model.IsFollowingAllowed = mr.IsFollowingAllowed(model.Viewer, model.ProfileOwner);
            model.IsCurrentlyFallowing = mr.IsFollowing(model.ViewerUserName, model.ProfileOwner.Email);
            model.FollowersCount = mr.GetFollowersCount(model.ProfileOwner.Email);
            //----
            //************ modules
            //-- common models:
            model.ProfilePageAdminActionsModel = ProfilePageAdminActionsModel.Create(model.ProfileOwner, model.HasProfileCover, context);
            model.ConsiderationsPPSectionModel = ConsiderationsPPSectionModel.Create(model.ProfileOwner, model.Viewer, context);
            model.ProfilePagePixelAdvModel = ProfilePagePixelAdvModel.Create(context);
            model.InfoPPSectionModel = InfoPPSectionModel.Create(model.ProfileOwner, model.Viewer, context);
            model.FriendsPPSectionModel = FriendsPPSectionModel.Create(model.ProfileOwner, model.Viewer, context);
            //--
            switch (model.Content)
            {
                case "timeline":
                    model.ProfilePageTimeLineModel = ProfilePageTimeLineModel.Create(model.ProfileOwner, model.Viewer, context);
                    break;
                case "friends":
                    model.ProfilePageFriendsModel = ProfilePageFriendsModel.Create(model.ProfileOwner, model.Viewer, context);
                    break;
                case "info":
                    model.ProfilePageInfoModel = ProfilePageInfoModel.Create(model.ProfileOwner, model.Viewer, context);
                    break;
                case "photos":
                    model.ProfilePagePhotosModel = ProfilePagePhotosModel.Create(model.ProfileOwner, model.Viewer, context);
                    break;
                case "activities":
                    model.ProfilePageActivitiesModel = ProfilePageActivitiesModel.Create(model.ProfileOwner, model.Viewer, context);
                    break;
                default:
                    model.ProfilePageTimeLineModel = ProfilePageTimeLineModel.Create(model.ProfileOwner, model.Viewer, context);
                    break;
            }
            return model;
        }
        #endregion
    }
}