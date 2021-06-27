using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class SharedObjectDetailsModel
    {
        public SharedObject SharedObject { get; set; }
        public int DefaultPhotoId { get; set; }
        public string UrlOfPostDetails { get; set; }
        public string UrlOfAlbumDetails { get; set; }
        //********* modules:
        public PostDetailsModel PostDetailsModel { get; set; }
        public AlbumDetailsModel AlbumDetailsModel { get; set; }
        public SharingDetailsModel SharingDetailsModel { get; set; }
        public PhotoDetailsModel PhotoDetailsModel { get; set; }
        //*********

        public static SharedObjectDetailsModel Create(SharedObject obj, int defaultPhotoId, System.Data.Objects.ObjectContext context)
        {
            var or = new SharedObjectsRepository(context);
            context = or.Context;
            //***********************************************
            var model = new SharedObjectDetailsModel();
            model.SharedObject = obj;
            model.DefaultPhotoId = defaultPhotoId;
            //****** modules:
            if (model.SharedObject is Post)
            {
                model.PostDetailsModel = PostDetailsModel.Create((Post)model.SharedObject, model.DefaultPhotoId, context);
                //increase def photo visits, because the post's counter will be increased in sharedobjectactions view.
                or.IncreaseVisitsCounter(model.DefaultPhotoId);
            }
            else if (model.SharedObject is Album)
            {
                model.AlbumDetailsModel = AlbumDetailsModel.Create((Album)model.SharedObject, model.DefaultPhotoId, context);
                //increase album visits, because the photo's counter will be increased in sharedobjectactions view.
                or.IncreaseVisitsCounter(model.SharedObject.Id);
            }
            else if (model.SharedObject is Sharing)
            {
                model.SharingDetailsModel = SharingDetailsModel.Create((Sharing)model.SharedObject, model.DefaultPhotoId, context);
            }
            else if (model.SharedObject is Photo)
            {
                if (model.SharedObject is PostPhoto)
                {
                    model.UrlOfPostDetails = ((PostPhoto)model.SharedObject).UrlOfPostDetails;
                }
                else if (model.SharedObject is AlbumPhoto)
                {
                    model.UrlOfAlbumDetails = ((AlbumPhoto)model.SharedObject).UrlOfAlbumDetails;
                }
                else
                {
                    model.PhotoDetailsModel = PhotoDetailsModel.Create((Photo)model.SharedObject, context);
                    //increase photo visits:
                    or.IncreaseVisitsCounter(model.SharedObject.Id);
                }
            }
            //******
            return model;
        }
    }
}