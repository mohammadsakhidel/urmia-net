using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class AlbumPresenterModel
    {
        #region Properties:
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string AlbumCoverThumbUrl { get; set; }
        public int AlbumPhotosCount { get; set; }
        public string AlbumDescription { get; set; }
        #endregion

        #region Methods
        public static AlbumPresenterModel Create(Album album, System.Data.Objects.ObjectContext context)
        {
            var model = new AlbumPresenterModel();
            model.AlbumId = album.Id;
            model.AlbumName = album.Name;
            model.AlbumCoverThumbUrl = album.CoverThumbUrl;
            model.AlbumPhotosCount = album.Photos.Count();
            model.AlbumDescription = album.Description;
            return model;
        }
        #endregion
    }
}