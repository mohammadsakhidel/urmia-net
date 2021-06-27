using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class HmiSettingsModel
    {
        #region Properties:
        public Member AssociatedMember { get; set; }
        #endregion

        #region Methods:
        public static HmiSettingsModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new HmiSettingsModel();
            model.AssociatedMember = member;
            return model;
        }
        #endregion
    }
}