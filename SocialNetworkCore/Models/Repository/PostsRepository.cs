using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class PostsRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public PostsRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public PostsRepository(System.Data.Objects.ObjectContext context)
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
        public Post Get(int id)
        {
            try
            {
                return db.SharedObjects.OfType<Post>().Single(p => p.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public PostPhoto GetPostPhoto(int id)
        {
            try
            {
                return db.SharedObjects.OfType<PostPhoto>().Single(p => p.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<SpecialPost> GetSpecialPosts()
        {
            return db.SharedObjects.OfType<SpecialPost>().OrderByDescending(sp => sp.DateOfAdd);
        }
        public IEnumerable<SpecialPost> FindSpecialPosts(SpecialPostShowMethod show_method)
        {
            var sps = from p in db.SharedObjects.OfType<SpecialPost>()
                      where p.ShowMethod == (byte)show_method && p.Status == (byte)PostStatus.Visible
                      orderby p.Priority descending, p.DateOfAdd descending
                      select p;
            return sps;
        }

        public string GetSmallThumbUrl(Post post)
        {
            if (post.Photos.Any())
            {
                PostPhoto pp = post.Photos.FirstOrDefault();
                if (pp != null)
                    return pp.SmallThumbUrl;
            }
            else if (post.PostVideos.Any())
            {
                PostVideo vid = post.PostVideos.FirstOrDefault();
                if (vid != null)
                    return vid.UrlOfSmallThumbnail;
            }
            return String.Empty;
        }
        #endregion

        #region INSERT:
        public void Insert(Post post)
        {
            // add post:
            db.SharedObjects.AddObject(post);
            // add activity:
            if (post.Type == (byte)PostType.NormalPost)
            {
                AddPostActivity(post);
            }
            if (post.Type == (byte)PostType.WallPost)
            {
                AddPostOnWallActivity(post);
                AddPostOnWallNotification(post);
            }
            // save:
            Save();
        }

        public void Add(Post post)
        {
            db.SharedObjects.AddObject(post);
            // add activity:
            if (post.Type == (byte)PostType.NormalPost)
            {
                AddPostActivity(post);
            }
            if (post.Type == (byte)PostType.WallPost)
            {
                AddPostOnWallActivity(post);
                AddPostOnWallNotification(post);
            }
        }

        public void Add(PostPhoto photo)
        {
            db.SharedObjects.AddObject(photo);
        }

        public void Add(PostVideo vid)
        {
            db.SharedObjects.AddObject(vid);
        }
        #endregion

        #region UPDATE:
        #endregion

        #region DELETE:
        public void Delete(int postId)
        {
            Post p = Get(postId);
            if (p != null)
            {
                // delete photos:
                foreach(var ph in p.Photos.Select(ph => ph.Id).ToList())
                {
                    DeletePostPhoto(ph);
                }
                // delete video:
                foreach (var v in p.PostVideos.Select(pv => pv.Id).ToList())
                {
                    DeletePostVideo(v);
                }
                // delete data:
                db.SharedObjects.DeleteObject(p);
                //
                Save();
            }
        }
        public void DeletePostPhoto(int photoId)
        {
            var photo = GetPostPhoto(photoId);
            // delete files:
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.Url));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.LargeThumbUrl));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.SmallThumbUrl));
            // deete data:
            db.SharedObjects.DeleteObject(photo);
            Save();
        }
        public void DeletePostVideo(int pVideoId)
        {
            var vr = new VideoRepository(db);
            var vid = (PostVideo)vr.Get(pVideoId);
            var fileNameBase = System.IO.Path.GetFileNameWithoutExtension(vid.FileName);
            // delete files:
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(vid.UrlOfSmallThumbnail));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(vid.UrlOfActualSizeThumbnail));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(vid.UrlOfVideoFile));
            // delete data:
            db.SharedObjects.DeleteObject(vid);
            Save();
        }
        #endregion

        #region Methods:
        public void Save()
        {
            db.SaveChanges();
        }

        private void AddPostActivity(Post post)
        {
            var act = new PostActivity();
            act.MemberId = post.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.PostId = post.Id;
            db.Activities.AddObject(act);
        }

        private void AddPostOnWallActivity(Post post)
        {
            var act = new PostOnWallActivity();
            act.MemberId = post.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.PostId = post.Id;
            act.WallOwner = post.Considerations;
            db.Activities.AddObject(act);
        }

        private void AddPostOnWallNotification(Post post)
        {
            if (!String.IsNullOrEmpty(post.Considerations) && post.Considerations != post.MemberId)
            {
                var not = new PostOnWallNotification();
                not.MemberId = post.Considerations;
                not.CreateTime = MyHelper.Now;
                not.Status = (byte)NotificationStatus.Unread;
                not.PostId = post.Id;
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
                    var mr = new MembersRepository(db);
                    var reciever = mr.Get(not.MemberId);
                    var sett = reciever.NotificationSetting ?? NotificationSetting.Default;
                    if (sett.OnWallPost && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                    {
                        EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                        reciever.LastEmailNotificationsDate = MyHelper.Now;
                        mr.Save();
                    }
                }
            }
        }
        #endregion

        public void Dispose()
        {
            db.Dispose();
        }
    }
}