using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;
using System.Web.Security;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfilePageAdminActionsModel
    {
        public Member ProfileOwner { get; set; }
        public bool HasProfileCover { get; set; }
        public bool CanUserSetManager { get; set; }
        public bool IsProfileOwnerManager { get; set; }
        public static ProfilePageAdminActionsModel Create(Member profOwner, bool hasCover ,System.Data.Objects.ObjectContext context)
        {
            var model = new ProfilePageAdminActionsModel();
            model.ProfileOwner = profOwner;
            model.HasProfileCover = hasCover;
            model.CanUserSetManager = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_super_admin) &&
                !Roles.IsUserInRole(model.ProfileOwner.Email, MyRoles.Administrator);
            if (model.CanUserSetManager)
            {
                model.IsProfileOwnerManager = Roles.IsUserInRole(model.ProfileOwner.Email, CoreHelpers.MyRoles.Manager);

            }
            return model;
        }
    }
}