using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Views.Models.Objects
{
    public class ObjectDetailsViewModel
    {
        public SharedObject SharedObject { get; set; }
        public int DefaultPhotoId { get; set; }
        public SharedObjectDetailsModel SharedObjectDetailsModel { get; set; }

        public static ObjectDetailsViewModel Create(SharedObject obj, System.Data.Objects.ObjectContext context)
        {
            var model = new ObjectDetailsViewModel();
            model.SharedObject = obj;
            model.DefaultPhotoId = (HttpContext.Current.Request.QueryString["def"] != null ? Convert.ToInt32(HttpContext.Current.Request.QueryString["def"]) : 0);
            model.SharedObjectDetailsModel = SharedObjectDetailsModel.Create(model.SharedObject, model.DefaultPhotoId, context);
            return model;
        }
    }
}