using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class AlbumsRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public AlbumsRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public AlbumsRepository(System.Data.Objects.ObjectContext context)
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
        public int GetAlbumsCount(string email)
        {
            return db.SharedObjects.OfType<Album>().Where(a => a.MemberId == email).Count();
        }

        public AlbumPhoto GetPhoto(int id)
        {
            try
            {
                return db.SharedObjects.OfType<AlbumPhoto>().Single(a => a.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public Album GetAlbum(int albumId)
        {
            try
            {
                return db.SharedObjects.OfType<Album>().Single(a => a.Id == albumId);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region INSERT:
        public void Insert(Album album)
        {
            db.SharedObjects.AddObject(album);
            Save();
        }

        public void Insert(AlbumPhoto aphoto)
        {
            db.SharedObjects.AddObject(aphoto);
            // add photo activity:
            AddPhotoActivity(aphoto);
            Save();
        }
        #endregion

        #region DELETE:
        public void DeletePhoto(int id)
        {
            // get photo:
            var photo = GetPhoto(id);
            // delete files:
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.Url));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.SmallThumbUrl));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.LargeThumbUrl));
            // clear profile photo data:
            if (photo.IsProfilePhoto)
            {
                var m = photo.Member;
                m.ProfilePhoto = "";
            }
            // delete object:
            db.SharedObjects.DeleteObject(photo);
            Save();
        }
        public void DeleteAlbum(int id)
        {
            var album = GetAlbum(id);
            // delete photos:
            var photos = (from p in album.Photos select p.Id).ToList();
            foreach (var p in photos)
            {
                DeletePhoto(p);
            }
            // delete album:
            db.SharedObjects.DeleteObject(album);
            // save:
            Save();
        }
        #endregion

        #region Update
        public void ClearProfilePhoto(string user)
        {
            var mr = new MembersRepository(db);
            var member = mr.Get(user);
            foreach (var ph in member.Albums.SelectMany(a => a.Photos))
            {
                ph.IsProfilePhoto = false;
            }
            mr.Save();
        }

        public void ClearAlbumCover(int albumId)
        {
            db.ClearAlbumCover(albumId);
        }
        #endregion

        #region Private Methods:
        private void AddPhotoActivity(Photo photo)
        {
            var act = new PhotoActivity();
            act.MemberId = photo.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.PhotoId = photo.Id;
            db.Activities.AddObject(act);
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