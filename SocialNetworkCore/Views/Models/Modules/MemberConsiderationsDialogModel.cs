using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class MemberConsiderationsDialogModel
    {
        #region Properties:
        public Member Member { get; set; }
        public bool IsUserAdmin { get; set; }
        #endregion

        #region Methods:
        public static MemberConsiderationsDialogModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new MemberConsiderationsDialogModel();
            model.Member = member;
            model.IsUserAdmin = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_user_mngr);
            return model;
        }
        #endregion
    }
}