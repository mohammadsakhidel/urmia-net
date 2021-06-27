using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfilePagePhotosModel
    {
        #region Properties:
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        //****** modules:
        public List<AlbumViewModel> AlbumModels { get; set; }
        public EmptyListModel EmptyListModel { get; set; }
        //******
        #endregion

        #region Methods
        public static ProfilePagePhotosModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var model = new ProfilePagePhotosModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = (viewer != null ? viewer.Email : "");
            //--- album models:
            var albums = model.ProfileOwner.Albums.ToList();
            var albumModels = new List<AlbumViewModel>();
            foreach (var album in albums)
            {
                //new AlbumViewModel { Album = album, Viewer = viewer }
                var albumModel = AlbumViewModel.Create(album, model.Viewer, context);
                albumModels.Add(albumModel);
            }
            //---
            model.AlbumModels = albumModels;
            model.EmptyListModel = EmptyListModel.Create(context);
            return model;
        }
        #endregion
    }
}