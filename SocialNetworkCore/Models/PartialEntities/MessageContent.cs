using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class MessageContent
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
            if (String.IsNullOrEmpty(this.Text))
            {
                ValidationErrors.Add("Text", Resources.Messages.RequiredMessageText);
            }
            if (this.Text.Length > Digits.MaxMessageLength)
            {
                ValidationErrors.Add("Text", Resources.Messages.StringLengthGeneral);
            }
        }
        #endregion
    }
}