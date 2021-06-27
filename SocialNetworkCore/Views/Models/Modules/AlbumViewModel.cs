using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class AlbumViewModel
    {
        #region Properties:
        public Album Album { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public List<Tuple<AlbumPhoto, bool>> AlbumPhotoRecords { get; set; }
        public string AlbumDesc { get; set; }
        public bool IsAnyVisiblePhotoForUser { get; set; }
        #endregion

        #region Methods:
        public static AlbumViewModel Create(Album album, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var model = new AlbumViewModel();
            model.Album = album;
            model.Viewer = viewer;
            model.ViewerUserName = (model.Viewer != null ? model.Viewer.Email : "");
            //--- album photo records:
            var albumPhotos = model.Album.Photos.ToList();
            var albumPhotoRecords = new List<Tuple<AlbumPhoto, bool>>();
            foreach (var ph in albumPhotos)
            {
                albumPhotoRecords.Add(new Tuple<AlbumPhoto, bool>(ph, ph.IsVisibleTo(model.ViewerUserName, context)));
            }
            //---
            model.AlbumPhotoRecords = albumPhotoRecords;
            model.AlbumDesc = !String.IsNullOrEmpty(model.Album.Description) ? model.Album.Description : "";
            model.IsAnyVisiblePhotoForUser = albumPhotos.Where(p => p.IsVisibleTo(model.ViewerUserName, context)).Any();
            return model;
        }
        #endregion
    }
}