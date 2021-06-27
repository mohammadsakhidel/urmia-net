using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using System.Web.Routing;

namespace SocialNetApp.Views.Models.Modules
{
    public class PhotoCollectionModel
    {
        public List<Photo> Photos { get; set; }
        public int DefaultPhotoId { get; set; }
        public int DefaultPhotoIndex { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public int SharedObjectId { get; set; }
        public string UpdateTargetId { get; set; }
        public int MaxPhotoWidth { get; set; }
        public Photo DefaultPhoto { get; set; }
        public bool IsDefaultPhotoVisibleForUser { get; set; }
        public AccessDeniedModel AccessDeniedModel { get; set; }

        public static PhotoCollectionModel Create(string actionName, string controllerName, int sharedObjectId, List<Photo> photos, int defPhotoId, string updateTargetId, System.Data.Objects.ObjectContext context)
        {
            var model = new PhotoCollectionModel();
            model.Photos = photos;
            if (model.Photos.Any())
            {
                model.ActionName = actionName;
                model.ControllerName = controllerName;
                model.SharedObjectId = sharedObjectId;
                model.DefaultPhotoId = defPhotoId;
                model.DefaultPhotoIndex = model.Photos.Select(p => p.Id).ToList().IndexOf(model.DefaultPhotoId);
                model.DefaultPhotoIndex = model.DefaultPhotoIndex < 0 ? 0 : model.DefaultPhotoIndex;
                model.DefaultPhoto = model.Photos[model.DefaultPhotoIndex];
                model.UpdateTargetId = updateTargetId;
                model.MaxPhotoWidth = 700;
                model.IsDefaultPhotoVisibleForUser = model.DefaultPhoto.IsVisibleTo(HttpContext.Current.User.Identity.Name, context);
                model.AccessDeniedModel = AccessDeniedModel.Create(context);
            }
            return model;
        }
    }
}