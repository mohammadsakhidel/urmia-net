using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class SharedObjectsRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public SharedObjectsRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public SharedObjectsRepository(System.Data.Objects.ObjectContext context)
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
        public SharedObject Get(int objId)
        {
            try
            {
                return db.SharedObjects.Single(obj => obj.Id == objId);
            }
            catch
            {
                return null;
            }
        }

        public Member GetOwner(int objId)
        {
            var q = from obj in db.SharedObjects
                    join mem in db.Members
                    on obj.MemberId equals mem.Email
                    where obj.Id == objId
                    select mem;
            return q.FirstOrDefault();
        }

        public bool HasLiked(int objId, string memberId)
        {
            return db.Likes.OfType<SharedObjectLike>().Where(l => l.MemberId == memberId && l.SharedObjectId == objId).Any();
        }

        public SharedObjectLike GetLike(int objId, string memberId)
        {
            try
            {
                return db.Likes.OfType<SharedObjectLike>().Single(l => l.MemberId == memberId && l.SharedObjectId == objId);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<SharedObjectLike> GetLikes(int objId)
        {
            return db.Likes.OfType<SharedObjectLike>().Where(l => l.SharedObjectId == objId);
        }

        public bool HasShared(string memberId, int objId)
        {
            return db.SharedObjects.OfType<Sharing>().Where(s => s.MemberId == memberId && s.SharedObjectId == objId).Any();
        }

        public int GetSharesCount(int objId)
        {
            return db.SharedObjects.OfType<Sharing>().Where(s => s.SharedObjectId == objId).Count();
        }

        public IEnumerable<Sharing> GetSharings(int objId)
        {
            return db.SharedObjects.OfType<Sharing>().Where(s => s.SharedObjectId == objId);
        }

        public Photo GetPhoto(int id)
        {
            try
            {
                return db.SharedObjects.OfType<Photo>().Single(p => p.Id == id);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region UPDATE:
        public void IncreaseVisitsCounter(int objId)
        {
            db.IncreaseObjectCounter(objId);
        }
        #endregion

        #region DELETE:
        public void DeleteSharing(int sharingId)
        {
            var sharing = (Sharing)Get(sharingId);
            db.SharedObjects.DeleteObject(sharing);
            Save();
        }
        #endregion

        #region INSERT:
        public void Like(int objId, string memberId)
        {
            var hasLiked = HasLiked(objId, memberId);
            if (!hasLiked)
            {
                var like = new SharedObjectLike();
                like.MemberId = memberId;
                like.SharedObjectId = objId;
                like.DateOfLike = MyHelper.Now;
                db.Likes.AddObject(like);
                // add like activity:
                AddLikeActivity(like);
                // add like notificartion:
                AddLikeNotification(like);
                // save:
                Save();
            }
            else
            {
                var like = GetLike(objId, memberId);
                if (like != null)
                {
                    db.Likes.DeleteObject(like);
                    Save();
                }
            }
        }
        public void Insert(Sharing sharing)
        {
            db.SharedObjects.AddObject(sharing);
            // add sharing activity:
            AddSharingActivity(sharing);
            // add share notification:
            AddSharingNotification(sharing);
            Save();
        }
        #endregion

        #region
        private void AddSharingActivity(Sharing sharing)
        {
            var act = new SharingActivity();
            act.MemberId = sharing.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.SharingId = sharing.Id;
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
            var obj = Get(((SharedObjectLike)like).SharedObjectId);
            if (obj.MemberId != like.MemberId)
            {
                var not = new LikeNotification();
                not.MemberId = obj.MemberId;
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
                    var reciever = obj.Member;
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
        private void AddSharingNotification(Sharing sharing)
        {
            var obj = Get(sharing.SharedObjectId);
            var not = new ShareNotification();
            not.MemberId = obj.MemberId;
            not.CreateTime = MyHelper.Now;
            not.Status = (byte)NotificationStatus.Unread;
            not.SharingId = sharing.Id;
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
                if (sett.OnShare && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                {
                    EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                    reciever.LastEmailNotificationsDate = MyHelper.Now;
                    Save();
                }
            }
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