using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace SocialNetApp.Models
{
    public partial class SocialNetDbEntities : ObjectContext
    {
        public override int SaveChanges(SaveOptions options)
        {
            // cascade deletes:
            var deleted_entities = ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Deleted).Select(e => e.Entity);
            foreach (var entity in deleted_entities)
            {
                if (entity is SharedObject)
                {
                    var obj = (SharedObject)entity;
                    CascadeDeleteSharedObject(obj);
                }
                else if (entity is Comment)
                {
                    var c = (Comment)entity;
                    CascadeDeleteComment(c);
                }
                //... cascade delete all activities and notifications
            }
            //********************************
            return base.SaveChanges(options);
        }
        #region Cascades:
        private void CascadeDeleteSharedObject(SharedObject obj)
        {
            // ********************************************************************* cascade delete sharings:
            var sharings = this.SharedObjects.OfType<Sharing>().Where(s => s.SharedObjectId == obj.Id);
            foreach (var sharing in sharings)
            {
                // delete sharing activities:
                var share_acts = this.Activities.OfType<SharingActivity>().Where(sa => sa.SharingId == sharing.Id);
                foreach (var share_act in share_acts)
                {
                    CascadeDeleteActivity(share_act);
                }
                // delete sharing notifications:
                var share_nots = this.Notifications.OfType<ShareNotification>().Where(sn => sn.SharingId == sharing.Id);
                foreach (var share_not in share_nots)
                {
                    CascadeDeleteNotification(share_not);
                }
                // delete object:
                CascadeDeleteSharedObject(sharing);
            }
            // ********************************************************************* cascade delete comments:
            var comments = this.Comments.Where(c => c.SharedObjectId == obj.Id);
            foreach (var c in comments)
            {
                CascadeDeleteComment(c);
            }
            // ********************************************************************* cascade delete sharedobject likes:
            var slikes = this.Likes.OfType<SharedObjectLike>().Where(sl => sl.SharedObjectId == obj.Id);
            foreach (var slike in slikes)
            {
                CascadeDeleteLike(slike);
            }
            //********************************************************************** cascade delete activities:
            var acts = GetAllRelatedActivities(obj);
            foreach (var act in acts)
            {
                CascadeDeleteActivity(act);
            }
            //********************************************************************** cascade delete notifications:
            var nots = GetAllRelatedNotifications(obj);
            foreach (var not in nots)
            {
                CascadeDeleteNotification(not);
            }
            // delete object:
            this.SharedObjects.DeleteObject(obj);
        }
        private void CascadeDeleteComment(Comment c)
        {
            // cascade delete comment likes:
            var clikes = this.Likes.OfType<CommentLike>().Where(cl => cl.CommentId == c.Id);
            foreach (var clike in clikes)
            {
                CascadeDeleteLike(clike);
            }
            // delete comment activities:
            var comm_acts = this.Activities.OfType<CommentActivity>().Where(ca => ca.CommentId == c.Id);
            foreach (var comm_act in comm_acts)
            {
                CascadeDeleteActivity(comm_act);
            }
            // delete comment notifictaions:
            var also_nots = this.Notifications.OfType<AlsoCommentNotification>().Where(n => n.CommentId == c.Id).Select(n => n as Notification);
            var comm_nots = this.Notifications.OfType<CommentNotification>().Where(n => n.CommentId == c.Id).Select(n => n as Notification);
            var tot_nots = also_nots.Concat(comm_nots);
            foreach (var comm_not in tot_nots)
            {
                CascadeDeleteNotification(comm_not);
            }
            // delete comment:
            this.Comments.DeleteObject(c);
        }
        private void CascadeDeleteLike(Like like)
        {
            // delete like activity:
            var like_acts = this.Activities.OfType<LikeActivity>().Where(la => la.LikeId == like.Id);
            foreach (var like_act in like_acts)
            {
                CascadeDeleteActivity(like_act);
            }
            // delete like notifications:
            var like_nots = this.Notifications.OfType<LikeNotification>().Where(ln => ln.LikeId == like.Id);
            foreach (var like_not in like_nots)
            {
                CascadeDeleteNotification(like_not);
            }
            // delete like data:
            this.Likes.DeleteObject(like);
        }
        private void CascadeDeleteActivity(Activity act)
        {
            this.Activities.DeleteObject(act);
        }
        private void CascadeDeleteNotification(Notification not)
        {
            this.Notifications.DeleteObject(not);
        }
        #endregion
        private IEnumerable<Activity> GetAllRelatedActivities(SharedObject obj)
        {
            var post_acts = this.Activities.OfType<PostActivity>().Where(pa => pa.PostId == obj.Id).AsEnumerable<Activity>();
            var postonwall_acts = this.Activities.OfType<PostOnWallActivity>().Where(pa => pa.PostId == obj.Id).AsEnumerable<Activity>();
            var photo_acts = this.Activities.OfType<PhotoActivity>().Where(pha => pha.PhotoId == obj.Id).AsEnumerable<Activity>();
            var acts = post_acts.Concat(postonwall_acts).Concat(photo_acts);
            return acts;
        }
        private IEnumerable<Notification> GetAllRelatedNotifications(SharedObject obj)
        {
            var nots = this.Notifications.OfType<PostOnWallNotification>().Where(not => not.PostId == obj.Id);
            return nots;
        }
    }
}