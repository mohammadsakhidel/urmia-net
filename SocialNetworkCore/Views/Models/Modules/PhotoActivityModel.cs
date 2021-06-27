using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PhotoActivityModel
    {
        #region Properties:
        public PhotoActivity Activity { get; set; }
        public ActivityVisibilityCheckLevel AVCL { get; set; }
        public bool IsActivityVisibleToUser { get; set; }
        public Member ActivityActor { get; set; }
        public Photo AssociatedPhoto { get; set; }
        public string AssociatedPhotoDesc { get; set; }
        public string AssociatedPhotoDetailsUrl { get; set; }
        //****** modules models:
        public UserIdentityModel ActorThumbIdentityModel { get; set; }
        public UserIdentityModel ActorNameIdentityModel { get; set; }
        public ActivityInfoModel ActivityInfoModel { get; set; }
        public ActivitySettingsModel ActivitySettingsModel { get; set; }
        public ActivityDetailsSectionModel ActivityDetailsSectionModel { get; set; }
        public SharedObjectActionsModel SharedObjectActionsModel { get; set; }
        //******
        #endregion

        #region Methods:
        public static PhotoActivityModel Create(Activity activity, Dictionary<string, object> param, System.Data.Objects.ObjectContext context)
        {
            var model = new PhotoActivityModel();
            model.Activity = (PhotoActivity)activity;
            model.AVCL = (param["AVCL"] != null ? (ActivityVisibilityCheckLevel)param["AVCL"] : ActivityVisibilityCheckLevel.CheckBoth);
            model.IsActivityVisibleToUser = model.Activity.IsVisibleTo(HttpContext.Current.User.Identity.Name, model.AVCL, context);
            if (model.IsActivityVisibleToUser)
            {
                model.ActivityActor = model.Activity.Member;
                model.AssociatedPhoto = model.Activity.Photo;
                model.AssociatedPhotoDesc = (model.AssociatedPhoto is AlbumPhoto ? ((AlbumPhoto)model.AssociatedPhoto).Description : "");
                model.AssociatedPhotoDetailsUrl = (model.AssociatedPhoto is AlbumPhoto ? ((AlbumPhoto)model.AssociatedPhoto).UrlOfAlbumDetails : model.AssociatedPhoto.UrlOfDetails);
                //****** modules models:
                model.ActorThumbIdentityModel = UserIdentityModel.Create(model.ActivityActor, 40, UserIdentityType.Thumb, "actor", "", "", "", context);
                model.ActorNameIdentityModel = UserIdentityModel.Create(model.ActivityActor, null, UserIdentityType.FullName, "actor_name", "", "", "", context);
                model.ActivityInfoModel = ActivityInfoModel.Create(model.Activity.TimeOfAct, model.AssociatedPhoto.VisibleTo, model.AssociatedPhoto.UrlOfDetails, context);
                model.ActivitySettingsModel = ActivitySettingsModel.Create(model.Activity, model.ActivityActor, context);
                model.SharedObjectActionsModel = SharedObjectActionsModel.Create(model.AssociatedPhoto, context);
                //******
            }
            return model;
        }
        #endregion
    }
}