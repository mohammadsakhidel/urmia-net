using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetApp.Models
{
    public partial class NotificationSetting
    {
        #region Statics:
        public static NotificationSetting GetModelFromCollection(FormCollection form)
        {
            var model = new NotificationSetting();
            model.OnNewMessage = form["OnNewMessage"] != null && form["OnNewMessage"] == "on";
            model.OnFriendshipRequest = form["OnFriendshipRequest"] != null && form["OnFriendshipRequest"] == "on";
            model.OnWallPost = form["OnWallPost"] != null && form["OnWallPost"] == "on";
            model.OnComment = form["OnComment"] != null && form["OnComment"] == "on";
            model.OnShare = form["OnShare"] != null && form["OnShare"] == "on";
            model.OnLike = form["OnLike"] != null && form["OnLike"] == "on";
            return model;
        }

        public static NotificationSetting Default
        {
            get
            {
                var def = new NotificationSetting();
                def.OnNewMessage = true;
                def.OnFriendshipRequest = true;
                def.OnWallPost = true;
                def.OnComment = true;
                def.OnShare = true;
                def.OnLike = true;
                return def;
            }
        }
        #endregion
    }
}