using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PhotoDetailsModel
    {
        public Photo Photo { get; set; }
        public Member PhotoOwner { get; set; }
        public bool HasPhoto { get; set; }
        //**** modules
        public UserIdentityModel PhotoOwnerIdentityModel { get; set; }
        public PhotoCollectionModel PhotoCollectionModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        //*****

        public static PhotoDetailsModel Create(Photo photo, System.Data.Objects.ObjectContext context)
        {
            var model = new PhotoDetailsModel();
            model.Photo = photo;
            model.HasPhoto = model.Photo != null;
            model.PhotoOwner = model.HasPhoto ? model.Photo.Member : null;
            if (model.HasPhoto)
            {
                //**** modules:
                model.PhotoOwnerIdentityModel = UserIdentityModel.Create(model.PhotoOwner, 45, UserIdentityType.ThumbAndFullName, "", "obj_det_fullname", "", "", context);
                model.PhotoCollectionModel = PhotoCollectionModel.Create("GetSharedObjectDetails", "Objects", model.Photo.Id, new List<Photo>() { photo }, model.Photo.Id, "obj_details_parent", context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.Photo, context);
                //****
            }
            return model;
        }
    }
}