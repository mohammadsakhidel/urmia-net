using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using System.Web.Mvc;

namespace SocialNetApp.Models
{
    public partial class AlbumPhoto
    {
        #region Validation:
        private ValidationErrorList _validationErrors = new ValidationErrorList();
        private bool _isValidated = false;

        public ValidationErrorList ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
            set
            {
                _validationErrors = value;
            }
        }
        public bool IsValid
        {
            get
            {
                if (!_isValidated)
                    Validate();
                return ValidationErrors.Count() == 0;
            }
        }

        public void Validate()
        {
            //Description:
            if (!String.IsNullOrEmpty(Description))
            {
                if (Description.Length > 500)
                    ValidationErrors.Add("Description", Resources.Messages.StringLengthGeneral);
            }

            _isValidated = true;
        }
        #endregion
        #region Public Properties:
        public string LargeThumbUrl
        {
            get
            {
                return Urls.AlbumLargeThumbnails + this.FileName;
            }
        }
        public string SmallThumbUrl
        {
            get
            {
                return Urls.AlbumSmallThumbnails + this.FileName;
            }
        }
        new public string Url
        {
            get
            {
                return Urls.AlbumPhotos + this.FileName;
            }
        }
        public string UrlOfAlbumDetails
        {
            get
            {
                return "~/ObjectDetails/" + this.AlbumId + "?def=" + this.Id;
            }
        }
        #endregion
        #region Static Methods:
        
        #endregion
    }
}