using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class PrivacyModel
    {
        #region Properties:
        public PrivacySetting PrivacySetting { get; set; }
        public MemberBoxModel MemberBoxModel { get; set; }
        #endregion

        #region Methods:
        public static PrivacyModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new PrivacyModel();
            model.PrivacySetting = (member.PrivacySetting != null ? member.PrivacySetting : PrivacySetting.Default);
            model.MemberBoxModel = MemberBoxModel.Create("BlockedUsers", 
                "FindInAssociatedMembers", "Members", null, 
                model.PrivacySetting.BlockedMembers(context).ToList(), "", "", context);
            return model;
        }
        #endregion
    }
}