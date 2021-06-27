using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class PhotosModel
    {
        #region Properties:
        public int AlbumsCount { get; set; }
        public int TotalPhotosCount { get; set; }
        public List<AlbumPresenterModel> AlbumPresenterModels { get; set; }
        #endregion

        #region Methods:
        public static PhotosModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var albums = member.Albums.ToList();
            List<AlbumPresenterModel> apModels = new List<AlbumPresenterModel>();
            foreach (var a in albums)
            {
                apModels.Add(AlbumPresenterModel.Create(a, null));
            }
            ////////////////
            var pModel = new PhotosModel();
            pModel.AlbumsCount = albums.Count();
            pModel.TotalPhotosCount = albums.SelectMany(a => a.Photos).Count();
            pModel.AlbumPresenterModels = apModels;
            return pModel;
        }
        #endregion
    }
}