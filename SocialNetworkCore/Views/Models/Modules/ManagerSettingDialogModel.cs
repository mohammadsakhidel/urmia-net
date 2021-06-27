using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class ManagerSettingDialogModel
    {
        public bool IsUserManager { get; set; }
        public Member AssociatedMember { get; set; }
        public string Access { get; set; }

        public static ManagerSettingDialogModel Create(Member associatedMember, System.Data.Objects.ObjectContext context)
        {
            var model = new ManagerSettingDialogModel();
            model.AssociatedMember = associatedMember;
            model.IsUserManager = model.AssociatedMember.Manager != null;
            model.Access = model.IsUserManager ? model.AssociatedMember.Manager.AccessLevel : "";
            return model;
        }
    }
}