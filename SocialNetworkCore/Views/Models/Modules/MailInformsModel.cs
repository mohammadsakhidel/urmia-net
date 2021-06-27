using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class MailInformsModel
    {
        #region Properties:
        public NotificationSetting NotificationSetting { get; set; }
        #endregion

        #region Methods:
        public static MailInformsModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new MailInformsModel();
            model.NotificationSetting = (member.NotificationSetting != null ? member.NotificationSetting : NotificationSetting.Default);
            return model;
        }
        #endregion
    }
}