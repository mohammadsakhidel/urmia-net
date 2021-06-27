using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class NotificationRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public NotificationRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public NotificationRepository(System.Data.Objects.ObjectContext context)
        {
            db = context != null ? (SocialNetDbEntities)context : new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }
        #endregion

        #region Properties:
        public System.Data.Objects.ObjectContext Context
        {
            get
            {
                return db;
            }
        }

        public IEnumerable<Notification> GetAllReadNotifications(string memberId)
        {
            return db.Notifications.Where(m => m.MemberId == memberId && m.Status == (byte)NotificationStatus.Read);
        }
        #endregion
        //**********************************************************************************
        #region SELECT:
        public IEnumerable<Notification> GetNotifications(string memberId, int count)
        {
            return db.Notifications.Where(n => n.MemberId == memberId).OrderByDescending(n => n.CreateTime).Take(count);
        }
        public IEnumerable<Notification> GetNotifications(string memberId)
        {
            return db.Notifications.Where(n => n.MemberId == memberId).OrderByDescending(n => n.CreateTime);
        }
        public int GetUnreadNotificationsCount(string memberId)
        {
            return db.Notifications.Where(n => n.MemberId == memberId && n.Status == (byte)NotificationStatus.Unread).Count();
        }
        public bool IsThereMoreNotifications(string memberId, int pgIndex)
        {
            var pgSize = Digits.ListingItemsPageSize;
            return db.Notifications.Where(n => n.MemberId == memberId).Count() > (pgIndex + 1) * pgSize;
        }
        public IEnumerable<Notification> GetPagedNotifications(string memberId, int pgIndex)
        {
            return db.Notifications.Where(n => n.MemberId == memberId).OrderByDescending(n => n.CreateTime).Skip(pgIndex * Digits.ListingItemsPageSize).Take(Digits.ListingItemsPageSize);
        }
        public ENotification GetMemberENotification(string memberId)
        {
            ENotification enot = new ENotification();
            var mr = new MembersRepository(db);
            var msg_rep = new MessagesRepository(mr.Context);
            var member = mr.Get(memberId);
            var lst_not_date = member.LastEmailNotificationsDate;
            // fetch info:
            var msgs_count = msg_rep.GetUnreadMessagesCountGTDate(memberId, lst_not_date);
            var reqs_count = mr.FindNewFriendshipRequestsCountGTDate(memberId, lst_not_date);
            var wallposts_count = (from not in db.Notifications.OfType<PostOnWallNotification>()
                                   where not.MemberId == memberId && not.Status == (byte)NotificationStatus.Unread && not.CreateTime > lst_not_date
                                   select not).Count();
            var comments_count = (from not in db.Notifications.OfType<CommentNotification>()
                                  where not.MemberId == memberId && not.Status == (byte)NotificationStatus.Unread && not.CreateTime > lst_not_date
                                  select not).Count();
            var shares_count = (from not in db.Notifications.OfType<ShareNotification>()
                                where not.MemberId == memberId && not.Status == (byte)NotificationStatus.Unread && not.CreateTime > lst_not_date
                                select not).Count();
            var likes_count = (from not in db.Notifications.OfType<LikeNotification>()
                                where not.MemberId == memberId && not.Status == (byte)NotificationStatus.Unread && not.CreateTime > lst_not_date
                                select not).Count();
            // create obj:
            enot.MemberId = memberId;
            enot.NewMessagesCount = msgs_count;
            enot.FriendshipRequestsCount = reqs_count;
            enot.WallPostsCount = wallposts_count;
            enot.CommentsCount = comments_count;
            enot.SharesCount = shares_count;
            enot.LikesCount = likes_count;
            return enot;
        }
        #endregion

        #region Insert:
        #endregion

        #region Updtate:
        public void MarkAllAsRead(string memberId)
        {
            var nots = db.Notifications.Where(n => n.MemberId == memberId && n.Status != (byte)NotificationStatus.Read);
            foreach (var n in nots)
            {
                n.Status = (byte)NotificationStatus.Read;
            }
            Save();
        }
        #endregion

        #region Delete:
        public void Remove(Notification notification)
        {
            db.Notifications.DeleteObject(notification);
        }
        #endregion

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}