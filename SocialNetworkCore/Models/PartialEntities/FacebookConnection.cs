using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public partial class FacebookConnection
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
            var rgxEmail = new System.Text.RegularExpressions.Regex(CoreHelpers.Patterns.Email);
            //MemberId:
            if (String.IsNullOrEmpty(MemberId))
            {
                ValidationErrors.Add("MemberId", Resources.Messages.NotValidFacebookEmail);
            }
            else if (!rgxEmail.IsMatch(MemberId))
            {
                ValidationErrors.Add("MemberId", Resources.Messages.NotValidFacebookEmail);
            }
            //fb Email:
            if (!String.IsNullOrEmpty(Email))
            {
                if (!rgxEmail.IsMatch(Email))
                    ValidationErrors.Add("Email", Resources.Messages.NotValidFacebookEmail);
            }
            //UserId
            if (String.IsNullOrEmpty(UserId))
            {
                ValidationErrors.Add("UserId", Resources.Messages.NotValidFacebookInformation);
            }
            //Name
            if (String.IsNullOrEmpty(Name))
            {
                ValidationErrors.Add("Name", Resources.Messages.NotValidFacebookInformation);
            }
            //LastName
            if (String.IsNullOrEmpty(LastName))
            {
                ValidationErrors.Add("LastName", Resources.Messages.NotValidFacebookInformation);
            }
            _isValidated = true;
        }
        #endregion
    }
}