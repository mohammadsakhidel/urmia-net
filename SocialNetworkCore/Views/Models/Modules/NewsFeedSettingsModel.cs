using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class NewsFeedSettingsModel
    {
        #region Properties:
        public NewsFeedInforming NewsFeedSettings { get; set; }
        public List<Member> UnwantedMembers { get; set; }
        public MemberBoxModel MemberBoxModel { get; set; }
        #endregion

        #region Methods:
        public static NewsFeedSettingsModel Create(Member member, System.Data.Objects.ObjectContext cntx)
        {
            var mr = new MembersRepository(cntx);
            cntx = mr.Context;
            var activitySettings = member.ActivitySetting != null ? member.ActivitySetting : ActivitySetting.Default;
            ////////////////////////////////////////
            var model = new NewsFeedSettingsModel();
            model.NewsFeedSettings = activitySettings.NewsFeedInformingObj;
            model.UnwantedMembers = mr.Get(activitySettings.UnwantedActorIds).ToList();
            model.MemberBoxModel = MemberBoxModel.Create("UnwantedActors",
                "FindInAssociatedMembers", "Members", null,
                model.UnwantedMembers, "", "", cntx);
            return model;
        }
        #endregion
    }
}