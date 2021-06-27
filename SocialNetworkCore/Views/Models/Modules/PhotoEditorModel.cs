using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PhotoEditorModel
    {
        #region Properties:
        public AlbumPhoto Photo { get; set; }
        #endregion

        #region Methods:
        public static PhotoEditorModel Create(AlbumPhoto photo, System.Data.Objects.ObjectContext context)
        {
            var model = new PhotoEditorModel();
            model.Photo = photo;
            return model;
        }
        #endregion
    }
}