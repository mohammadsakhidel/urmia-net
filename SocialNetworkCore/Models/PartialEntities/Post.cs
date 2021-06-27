using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using System.Text.RegularExpressions;

namespace SocialNetApp.Models
{
    public partial class Post
    {
        #region Properties:
        public string FileName
        {
            get
            {
                return this.Id.ToString() + "_" + MyHelper.GetRandomString(5, true);
            }
        }
        public List<string> TempFilesToBeAdded { get; set; }
        public bool HasAnyAttachments
        {
            get
            {
                return this.PostVideos.Any() || this.Photos.Any();
            }
        }
        #endregion

        #region Static Methods:
        #endregion

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
            //Text:
            if (String.IsNullOrEmpty(Text) && this.TempFilesToBeAdded.Count == 0)
                ValidationErrors.Add("Text", Resources.Messages.RequiredPostTextOrPhoto);
            if (!String.IsNullOrEmpty(Text))
            {
                if (Text.Length > Digits.MaxPostTextLength)
                    ValidationErrors.Add("Text", Resources.Messages.StringLengthPostText);
            }
            if (TempFilesToBeAdded.Count > Digits.MaxPostPhotos)
                ValidationErrors.Add("Photos", Resources.Messages.ExceedMaxPostPhotos);
            _isValidated = true;
        }
        #endregion
    }
}