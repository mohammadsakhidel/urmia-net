using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ConsiderationsPPSectionModel
    {
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public bool IsUserAdmin { get; set; }

        public static ConsiderationsPPSectionModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var model = new ConsiderationsPPSectionModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = model.Viewer != null ? model.Viewer.Email : "";
            model.IsUserAdmin = !String.IsNullOrEmpty(model.ViewerUserName) && Member.IsInAdminGroup(model.ViewerUserName, ManagerAccessLevels.core_user_mngr);
            return model;
        }
    }
}