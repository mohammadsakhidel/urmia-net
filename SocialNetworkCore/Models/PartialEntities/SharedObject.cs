using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Models
{
    public partial class SharedObject
    {
        public string UrlOfDetails
        {
            get
            {
                return "~/ObjectDetails/" + this.Id;
            }
        }

        public string GetSmallThumbUrl(System.Data.Objects.ObjectContext context)
        {
            var pr = new PostsRepository(context);
            var url = "";
            var obj = this is Sharing ? ((Sharing)this).SharedObject : this;
            url = obj is AlbumPhoto ?
                ((AlbumPhoto)obj).SmallThumbUrl :
                (obj is Post ? pr.GetSmallThumbUrl((Post)obj) : "");
            return url;
        }

        public string DateOfAddText
        {
            get
            {
                string txt = CoreHelpers.MyHelper.DateToText(this.DateOfAdd);
                return txt;
            }
        }

        public bool IsVisibleTo(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            // is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            if ((this.VisibleTo == (byte)CoreHelpers.VisibleTo.EveryOne) ||
                (this.VisibleTo == (byte)CoreHelpers.VisibleTo.Members && username.Trim().Length > 0) ||
                (this.VisibleTo == (byte)CoreHelpers.VisibleTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                (this.VisibleTo == (byte)CoreHelpers.VisibleTo.OnlyMe && username == this.MemberId))
            {
                res = true;
            }
            return res;
        }

        public bool IsAllowedToComment(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            // is admin?
            if (Member.IsInAdminGroup(username, ManagerAccessLevels.core_comments))
                return true;
            //
            var res = false;
            if ((this.AllowCommentTo == (byte)CoreHelpers.AllowCommentTo.Members && username.Trim().Length > 0) ||
                (this.AllowCommentTo == (byte)CoreHelpers.AllowCommentTo.Friends && mr.AreFriends(this.MemberId, username)) ||
                (this.AllowCommentTo == (byte)CoreHelpers.AllowCommentTo.OnlyMe && username == this.MemberId))
            {
                var privacy = this.Member.PrivacySetting;
                if ((privacy == null) || (privacy != null && !privacy.BlockedUserNames.Contains(username)))
                {
                    res = true;
                }
            }
            return res;
        }
    }
}