using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PostEditorModel
    {
        #region Properties:
        public string WaterMark { get; set; }
        public string UpdatePanelId { get; set; }
        public string WallOwner { get; set; }
        public string SupportedVideoFormats { get; set; }
        public string SupportedPhotoFormats { get; set; }
        #endregion

        #region Methods:
        public static PostEditorModel Create(string waterMark, string updatePanelId, string wallOwner, System.Data.Objects.ObjectContext context)
        {
            var model = new PostEditorModel();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                model.WaterMark = !String.IsNullOrEmpty(waterMark) ? waterMark : Resources.Words.WhatsOnYourMind;
                model.UpdatePanelId = !String.IsNullOrEmpty(updatePanelId) ? updatePanelId : "";
                model.WallOwner = !String.IsNullOrEmpty(wallOwner) ? wallOwner : "";
                var supportedVideoFormatsTxt = "";
                Configs.SupportedVideoFormats.ForEach(f => supportedVideoFormatsTxt += string.Format(".{0},", f));
                model.SupportedVideoFormats = supportedVideoFormatsTxt;
                var supportedPhotoFormatsTxt = "";
                Configs.SupportedPhotoFormats.ForEach(f => supportedPhotoFormatsTxt += string.Format(".{0},", f));
                model.SupportedPhotoFormats = supportedPhotoFormatsTxt;
            }
            return model;
        }
        #endregion
    }
}