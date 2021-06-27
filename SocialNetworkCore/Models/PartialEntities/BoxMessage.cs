using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using System.Web.Mvc;

namespace SocialNetApp.Models
{
    public partial class BoxMessage
    {
        #region Properties:
        public string DateOfSendText
        {
            get
            {
                return DateHelper.DateTimeToShortText(this.DateOfSend);
            }
        }

        public string SeenAtText
        {
            get
            {
                if (this is OutboxMessage && !String.IsNullOrEmpty(this.StatusInfo) && DateHelper.IsValidDateTimeText(this.StatusInfo))
                {
                    return DateHelper.DateTimeToShortText(DateTime.Parse(this.StatusInfo));
                }
                else
                {
                    return "";
                }
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
            throw new NotImplementedException();
        }
        #endregion
    }
}