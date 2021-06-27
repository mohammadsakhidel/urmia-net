using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models.Repository;
using System.IO;
using System.Web.Mvc;

namespace SocialNetApp.Models
{
    public partial class Activity
    {
        public string TimeOfActText
        {
            get
            {
                string txt = CoreHelpers.MyHelper.DateToText(this.TimeOfAct);
                return txt;
            }
        }
        public ActivityType ActivityType
        {
            get
            {
                try
                {
                    var typeString = this.GetType().Name;
                    return (ActivityType)Enum.Parse(typeof(ActivityType), typeString);
                }
                catch
                {
                    return ActivityType.None;
                }
            }
        }
        public string PartialViewUrl
        {
            get
            {
                if (this.ActivityType == ActivityType.PostActivity)
                {
                    return Urls.ModuleViews + "_PostActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.PhotoActivity)
                {
                    return Urls.ModuleViews + "_PhotoActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.SharingActivity)
                {
                    return Urls.ModuleViews + "_SharingActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.CommentActivity)
                {
                    return Urls.ModuleViews + "_CommentActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.LikeActivity)
                {
                    return Urls.ModuleViews + "_LikeActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.ChangeInfoActivity)
                {
                    return Urls.ModuleViews + "_ChangeInfoActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.ChangeCoverActivity)
                {
                    return Urls.ModuleViews + "_ChangeCoverActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.EducationActivity)
                {
                    return Urls.ModuleViews + "_EducationActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.SkillActivity)
                {
                    return Urls.ModuleViews + "_SkillActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.PostOnWallActivity)
                {
                    return Urls.ModuleViews + "_PostOnWallActivity.cshtml";
                }
                else if (this.ActivityType == ActivityType.ChangePPActivity)
                {
                    return Urls.ModuleViews + "_ChangePPActivity.cshtml";
                }
                else return "";
            }
        }
        public bool IsVisibleTo(string username, ActivityVisibilityCheckLevel checklevel, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            // check level:
            var check_privacy = checklevel == ActivityVisibilityCheckLevel.CheckPrivacy || checklevel == ActivityVisibilityCheckLevel.CheckBoth ? true : false;
            var check_obj = checklevel == ActivityVisibilityCheckLevel.CheckObject || checklevel == ActivityVisibilityCheckLevel.CheckBoth ? true : false;
            if (checklevel == ActivityVisibilityCheckLevel.None)
                return true;
            // is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            // if user is not authenthicated username = "" is passed in.
            var res = false;
            var actor_privacy = PrivacySetting.Default;
            if (check_privacy)
            {
                actor_privacy = this.Member.PrivacySetting ?? PrivacySetting.Default;
            }
            if (this.ActivityType == ActivityType.LikeActivity)
            {
                if (!check_privacy ||
                    (actor_privacy.WhoSeeLikeActivities == (byte)VisibleTo.EveryOne) ||
                    (actor_privacy.WhoSeeLikeActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (actor_privacy.WhoSeeLikeActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (actor_privacy.WhoSeeLikeActivities == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    if (!check_obj) return true;
                    var like = ((LikeActivity)this).Like;
                    var obj = (like is CommentLike ? ((CommentLike)like).Comment.SharedObject : ((SharedObjectLike)like).SharedObject);
                    if (obj.IsVisibleTo(username, context))
                    {
                        res = true;
                    }
                }
            }
            else if (this.ActivityType == ActivityType.CommentActivity)
            {
                if (!check_privacy ||
                    (actor_privacy.WhoSeeCommentActivities == (byte)VisibleTo.EveryOne) ||
                    (actor_privacy.WhoSeeCommentActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (actor_privacy.WhoSeeCommentActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (actor_privacy.WhoSeeCommentActivities == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    if (!check_obj) return true;
                    var comment = ((CommentActivity)this).Comment;
                    var obj = comment.SharedObject;
                    if (obj.IsVisibleTo(username, context))
                    {
                        res = true;
                    }
                }
            }
            else if (this.ActivityType == ActivityType.ChangeInfoActivity)
            {
                if (!check_privacy ||
                    (actor_privacy.WhoSeeChangeInfoActivities == (byte)VisibleTo.EveryOne) ||
                    (actor_privacy.WhoSeeChangeInfoActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (actor_privacy.WhoSeeChangeInfoActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (actor_privacy.WhoSeeChangeInfoActivities == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    res = true;
                }
            }
            else if (this.ActivityType == ActivityType.ChangeCoverActivity)
            {
                if (!check_privacy ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.EveryOne) ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    res = true;
                }
            }
            else if (this.ActivityType == ActivityType.ChangePPActivity)
            {
                if (!check_privacy ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.EveryOne) ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (actor_privacy.WhoSeeChangeCoverActivities == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    if (!check_obj) return true;
                    var obj = ((ChangePPActivity)this).Photo;
                    if (obj.IsVisibleTo(username, context))
                    {
                        res = true;
                    }
                }
            }
            else if (this.ActivityType == ActivityType.EducationActivity)
            {
                if (!check_privacy ||
                    (actor_privacy.WhoSeeEducationActivities == (byte)VisibleTo.EveryOne) ||
                    (actor_privacy.WhoSeeEducationActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (actor_privacy.WhoSeeEducationActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (actor_privacy.WhoSeeEducationActivities == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    res = true;
                }
            }
            else if (this.ActivityType == ActivityType.SkillActivity)
            {
                if (!check_privacy ||
                    (actor_privacy.WhoSeeSkillActivities == (byte)VisibleTo.EveryOne) ||
                    (actor_privacy.WhoSeeSkillActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (actor_privacy.WhoSeeSkillActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (actor_privacy.WhoSeeSkillActivities == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    res = true;
                }
            }
            else if (this.ActivityType == ActivityType.PostActivity)
            {
                if (!check_obj) return true;
                var post = ((PostActivity)this).Post;
                if (post.IsVisibleTo(username, context))
                {
                    res = true;
                }
            }
            else if (this.ActivityType == ActivityType.PostOnWallActivity)
            {
                if (!check_obj) return true;
                var post = ((PostOnWallActivity)this).Post;
                if (post.IsVisibleTo(username, context))
                {
                    res = true;
                }
            }
            else if (this.ActivityType == ActivityType.PhotoActivity)
            {
                if (!check_obj) return true;
                var photo = ((PhotoActivity)this).Photo;
                if (photo.IsVisibleTo(username, context))
                {
                    res = true;
                }
            }
            else if (this.ActivityType == ActivityType.SharingActivity)
            {
                var sharing = ((SharingActivity)this).Sharing;
                if ((sharing.VisibleTo == (byte)VisibleTo.EveryOne) ||
                    (sharing.VisibleTo == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                    (sharing.VisibleTo == (byte)VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                    (sharing.VisibleTo == (byte)VisibleTo.OnlyMe && username == this.MemberId))
                {
                    if (!check_obj) return true;
                    var obj = sharing.SharedObject;
                    if (obj.IsVisibleTo(username, context))
                    {
                        res = true;
                    }
                }
            }
            return res;
        }
    }
}