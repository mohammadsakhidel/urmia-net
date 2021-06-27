using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class AccountSettingsModel
    {
        #region Properties:
        public Member AssociatedMember { get; set; }
        public AccountSetting AccountSetting { get; set; }
        public List<SupportedLanguage> SupportedLanguages { get; set; }
        #endregion

        #region Methods:
        public static AccountSettingsModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new AccountSettingsModel();
            model.AssociatedMember = member;
            model.AccountSetting = (member.AccountSetting != null ? member.AccountSetting : AccountSetting.Default);
            model.SupportedLanguages = Configs.SupportedLanguages.ToList();
            return model;
        }
        #endregion
    }
}