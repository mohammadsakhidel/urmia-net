using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Models
{
    public class ENotification
    {
        public ENotification()
        {
        }

        public ENotification(string memberId, System.Data.Objects.ObjectContext context)
        {
            using (var nr = new NotificationRepository(context))
            {
                var enot = nr.GetMemberENotification(memberId);
                ///////////////
                this.MemberId = memberId;
                this.NewMessagesCount = enot.NewMessagesCount;
                this.FriendshipRequestsCount = enot.FriendshipRequestsCount;
                this.WallPostsCount = enot.WallPostsCount;
                this.CommentsCount = enot.CommentsCount;
                this.SharesCount = enot.SharesCount;
                this.LikesCount = enot.LikesCount;
            }
        }
        public string MemberId { get; set; }
        public int NewMessagesCount { get; set; }
        public int FriendshipRequestsCount { get; set; }
        public int WallPostsCount { get; set; }
        public int CommentsCount { get; set; }
        public int SharesCount { get; set; }
        public int LikesCount { get; set; }
        //////////////////////////////////////////
        public bool IsAllowed(System.Data.Objects.ObjectContext context)
        {
            using (var mr = new MembersRepository(context))
            {
                var res = false;
                var setting = mr.GetNotificationSettings(MemberId);
                if ((setting.OnNewMessage && NewMessagesCount > 0) ||
                    (setting.OnFriendshipRequest && FriendshipRequestsCount > 0) ||
                    (setting.OnWallPost && WallPostsCount > 0) ||
                    (setting.OnComment && CommentsCount > 0) ||
                    (setting.OnShare && SharesCount > 0) ||
                    (setting.OnLike && LikesCount > 0))
                {
                    res = true;
                }
                return res;
            }
        }
    }
}