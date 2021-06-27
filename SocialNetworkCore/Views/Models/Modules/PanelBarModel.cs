using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PanelBarModel
    {
        #region properties
        public Member Member { get; set; }
        public string PPUrl { get; set; }
        public bool IsUserAdmin { get; set; }
        public bool UserHasAccessToSpecialPosts { get; set; }
        public bool UserHasAccessToObjectChecker { get; set; }
        public bool UserHasAccessToPages { get; set; }
        public bool UserHasAccessToAdvs { get; set; }
        #endregion

        #region Methods
        public static PanelBarModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var pModel = new PanelBarModel();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                pModel.Member = member;
                pModel.PPUrl = (!String.IsNullOrEmpty(pModel.Member.ProfilePhoto) ?
                    Urls.AlbumLargeThumbnails + pModel.Member.ProfilePhoto :
                    (pModel.Member.Gender.Value ? Defaults.UrlForMaleProfileImage : Defaults.UrlForFemaleProfileImage));
                // admin access:
                pModel.IsUserAdmin = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, string.Empty);
                if (pModel.IsUserAdmin)
                {
                    pModel.UserHasAccessToSpecialPosts = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_spec_posts);
                    pModel.UserHasAccessToObjectChecker = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_obj_check);
                    pModel.UserHasAccessToPages = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_pages);
                    pModel.UserHasAccessToAdvs = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_advs);
                }
            }
            return pModel;
        }
        #endregion
    }
}