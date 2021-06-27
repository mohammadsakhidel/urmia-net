using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class Comment
    {
        public string DateOfCommentText
        {
            get
            {
                string txt = CoreHelpers.MyHelper.DateToText(this.DateOfComment);
                return txt;
            }
        }
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
            if (String.IsNullOrEmpty(Text))
                ValidationErrors.Add("Text", Resources.Messages.RequiredCommentText);
            if (!String.IsNullOrEmpty(Text))
            {
                if (Text.Length > Digits.MaxCommentTextLength)
                    ValidationErrors.Add("Text", Resources.Messages.StringLengthCommentText);
            }

            _isValidated = true;
        }
        #endregion
    }
}