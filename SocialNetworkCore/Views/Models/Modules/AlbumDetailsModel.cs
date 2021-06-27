using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class AlbumDetailsModel
    {
        public Album Album { get; set; }
        public int DefaultPhotoId { get; set; }
        public Member AlbumOwner { get; set; }
        public AlbumPhoto DefaultPhoto { get; set; }
        public bool IsDefaultPhotoVisibleToUser { get; set; }
        public List<AlbumPhoto> AlbumPhotos { get; set; }
        //******* modules:
        public UserIdentityModel AlbumOwnerIdentityModel { get; set; }
        public PhotoCollectionModel PhotoCollectionModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        //*******

        public static AlbumDetailsModel Create(Album album, int defPhotoId, System.Data.Objects.ObjectContext context)
        {
            var ar = new AlbumsRepository(context);
            context = ar.Context;
            //***********************
            var model = new AlbumDetailsModel();
            model.Album = album;
            model.AlbumPhotos = model.Album.Photos.ToList();
            model.DefaultPhotoId = defPhotoId;
            model.AlbumOwner = model.Album.Member;
            model.DefaultPhoto = ar.GetPhoto(model.DefaultPhotoId);
            model.IsDefaultPhotoVisibleToUser = model.DefaultPhoto.IsVisibleTo(HttpContext.Current.User.Identity.Name, context);
            //***** moduels:
            model.AlbumOwnerIdentityModel = UserIdentityModel.Create(model.AlbumOwner, 45, UserIdentityType.ThumbAndFullName, "", "obj_det_fullname", "", "", context);
            model.PhotoCollectionModel = PhotoCollectionModel.Create("GetSharedObjectDetails", "Objects", model.Album.Id, model.AlbumPhotos.Cast<Photo>().ToList(), model.DefaultPhotoId, "obj_details_parent", context);
            model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.DefaultPhoto, context);
            //*****
            return model;
        }
    }
}