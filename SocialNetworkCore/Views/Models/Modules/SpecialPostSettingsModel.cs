using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class SpecialPostSettingsModel
    {
        public bool IsUserAdmin { get; set; }
        public SpecialPost SpecialPost { get; set; }

        public static SpecialPostSettingsModel Create(SpecialPost post, System.Data.Objects.ObjectContext context)
        {
            var model = new SpecialPostSettingsModel();
            model.IsUserAdmin = Member.IsInAdminGroup(HttpContext.Current.User.Identity.Name, ManagerAccessLevels.core_spec_posts);
            if (model.IsUserAdmin)
            {
                model.SpecialPost = post;
            }
            return model;
        }
    }
}