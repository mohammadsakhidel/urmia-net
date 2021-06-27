using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class CommentsRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public CommentsRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public CommentsRepository(System.Data.Objects.ObjectContext context)
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
        #endregion
        //**********************************************************************************
        #region SELECT:
        public Comment Get(int id)
        {
            try
            {
                return db.Comments.Single(c => c.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public List<Comment> FindLastComments(int objId)
        {
            var list = new List<Comment>();
            var size = Digits.MaxCommentsToShow;
            var count = db.Comments.Where(c => c.SharedObjectId == objId).Count();
            if (count > size)
                list = db.Comments.Include("Member")
                    .Where(c => c.SharedObjectId == objId && c.Member.StatusCode == (byte)MemberStatus.Active)
                    .OrderBy(c => c.DateOfComment)
                    .Skip(count - size)
                    .Take(size).ToList();
            else
                list = db.Comments.Include("Member")
                    .Where(c => c.SharedObjectId == objId && c.Member.StatusCode == (byte)MemberStatus.Active)
                    .OrderBy(c => c.DateOfComment).ToList();
            return list;
        }
        public List<Comment> FindLastComments(int objId, int comments_count)
        {
            var list = new List<Comment>();
            var size = Digits.MaxCommentsToShow;
            if (comments_count > size)
                list = db.Comments.Include("Member")
                    .Where(c => c.SharedObjectId == objId && c.Member.StatusCode == (byte)MemberStatus.Active)
                    .OrderBy(c => c.DateOfComment)
                    .Skip(comments_count - size)
                    .Take(size).ToList();
            else
                list = db.Comments.Include("Member")
                    .Where(c => c.SharedObjectId == objId && c.Member.StatusCode == (byte)MemberStatus.Active)
                    .OrderBy(c => c.DateOfComment).ToList();
            return list;
        }
        public int GetCommentsCount(int objId)
        {
            var q = (from c in db.Comments
                     join m in db.Members
                     on c.MemberId equals m.Email
                     where c.SharedObjectId == objId && m.StatusCode == (byte)MemberStatus.Active
                     select c).Count();
            return q;
        }
        public List<Comment> GetComments(int objId, DateTime dt)
        {
            var list = db.Comments.Include("Member")
                .Where(c => c.SharedObjectId == objId && c.DateOfComment < dt && c.Member.StatusCode == (byte)MemberStatus.Active)
                .OrderByDescending(c => c.DateOfComment)
                .Take(Digits.CommentsPageSize).ToList();
            return list.OrderBy(c => c.DateOfComment).ToList();
        }
        public bool HasLiked(int commentId, string memberId)
        {
            return db.Likes.OfType<CommentLike>().Where(l => l.CommentId == commentId && l.MemberId == memberId).Any();
        }
        public CommentLike GetLike(int commentId, string memberId)
        {
            try
            {
                return db.Likes.OfType<CommentLike>().Single(l => l.MemberId == memberId && l.CommentId == commentId);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<CommentLike> GetLikes(int commentId)
        {
            return db.Likes.OfType<CommentLike>().Where(l => l.CommentId == commentId);
        }
        public IEnumerable<Member> GetAlsoCommentNotifiedPeople(Comment comm)
        {
            var or = new SharedObjectsRepository(db);
            var obj = or.Get(comm.SharedObjectId);
            var q = from mId in
                        ((from c in db.Comments
                          where c.SharedObjectId == comm.SharedObjectId && c.MemberId != comm.MemberId && c.MemberId != obj.MemberId
                          orderby c.DateOfComment descending
                          select c.MemberId).Distinct().Take(Digits.MaximumAlsoCommentReceivers))
                    join m in db.Members
                    on mId equals m.Email
                    select m;
            return q;
        }
        #endregion

        #region INSERT:
        public void Insert(Comment comment)
        {
            db.Comments.AddObject(comment);
            // add comment activity:
            AddCommentActivity(comment);
            // add comment notification:
            AddCommentNotification(comment);
            // add also comment notifications:
            AddAlsoCommentNotifications(comment);
            Save();
        }

        public void Like(int commentId, string memberId)
        {
            var hasLiked = HasLiked(commentId, memberId);
            if (!hasLiked)
            {
                var like = new CommentLike();
                like.MemberId = memberId;
                like.CommentId = commentId;
                like.DateOfLike = MyHelper.Now;
                db.Likes.AddObject(like);
                // add like activity:
                AddLikeActivity(like);
                // add like notification:
                AddLikeNotification(like);
                // save:
                Save();
            }
            else
            {
                var like = GetLike(commentId, memberId);
                if (like != null)
                {
                    db.Likes.DeleteObject(like);
                    Save();
                }
            }
        }
        #endregion

        #region DELETE:
        public void Delete(int commentId)
        {
            db.Comments.DeleteObject(Get(commentId));
            Save();
        }
        #endregion

        #region Private Methods:
        public void AddCommentActivity(Comment comment)
        {
            var act = new CommentActivity();
            act.MemberId = comment.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.CommentId = comment.Id;
            db.Activities.AddObject(act);
        }

        private void AddLikeActivity(Like like)
        {
            var act = new LikeActivity();
            act.MemberId = like.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.LikeId = like.Id;
            db.Activities.AddObject(act);
        }

        private void AddLikeNotification(Like like)
        {
            var comment = Get(((CommentLike)like).CommentId);
            if (comment.MemberId != like.MemberId)
            {
                var not = new LikeNotification();
                not.MemberId = comment.MemberId;
                not.CreateTime = MyHelper.Now;
                not.Status = (byte)NotificationStatus.Unread;
                not.LikeId = like.Id;
                db.Notifications.AddObject(not);
                // notify if online:
                if (OnlinesHelper.IsOnline(not.MemberId))
                {
                    var cr = new ConnectionsRepository();
                    var connections = cr.FindConnections(not.MemberId).ToList();
                    var signals = connections.Select(con => new NotifySignal { Type = NotifySignalType.NewNotification, ConnectionId = con.Id });
                    if (connections.Any())
                    {
                        SocialNetHub.SendNotifySignals(signals);
                    }
                }
                else //send email notification if needed:
                {
                    var reciever = comment.Member;
                    var sett = reciever.NotificationSetting ?? NotificationSetting.Default;
                    if (sett.OnLike && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                    {
                        EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                        reciever.LastEmailNotificationsDate = MyHelper.Now;
                        Save();
                    }
                }
            }
        }

        private void AddCommentNotification(Comment comment)
        {
            var or = new SharedObjectsRepository(db);
            var obj = or.Get(comment.SharedObjectId);
            if (comment.MemberId != obj.MemberId)
            {
                var not = new CommentNotification();
                not.MemberId = obj.MemberId;
                not.CreateTime = MyHelper.Now;
                not.Status = (byte)NotificationStatus.Unread;
                not.CommentId = comment.Id;
                db.Notifications.AddObject(not);
                // notify if online:
                if (OnlinesHelper.IsOnline(not.MemberId))
                {
                    var cr = new ConnectionsRepository();
                    var connections = cr.FindConnections(not.MemberId).ToList();
                    var signals = connections.Select(con => new NotifySignal { Type = NotifySignalType.NewNotification, ConnectionId = con.Id });
                    if (connections.Any())
                    {
                        SocialNetHub.SendNotifySignals(signals);
                    }
                }
                else //send email notification if needed:
                {
                    var reciever = obj.Member;
                    var sett = reciever.NotificationSetting ?? NotificationSetting.Default;
                    if (sett.OnComment && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                    {
                        EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                        reciever.LastEmailNotificationsDate = MyHelper.Now;
                        Save();
                    }
                }
            }
        }

        private void AddAlsoCommentNotifications(Comment comment)
        {
            var notified_people = GetAlsoCommentNotifiedPeople(comment);
            foreach (var mem in notified_people)
            {
                var not = new AlsoCommentNotification();
                not.MemberId = mem.Email;
                not.CreateTime = MyHelper.Now;
                not.Status = (byte)NotificationStatus.Unread;
                not.CommentId = comment.Id;
                db.Notifications.AddObject(not);
                // notify if online:
                if (OnlinesHelper.IsOnline(not.MemberId))
                {
                    var cr = new ConnectionsRepository();
                    var connections = cr.FindConnections(not.MemberId).ToList();
                    var signals = connections.Select(con => new NotifySignal { Type = NotifySignalType.NewNotification, ConnectionId = con.Id });
                    if (connections.Any())
                    {
                        SocialNetHub.SendNotifySignals(signals);
                    }
                }
                else //send email notification if needed:
                {
                    var reciever = mem;
                    var sett = reciever.NotificationSetting ?? NotificationSetting.Default;
                    if (sett.OnComment && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                    {
                        EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                        reciever.LastEmailNotificationsDate = MyHelper.Now;
                    }
                }
            }
            Save();
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