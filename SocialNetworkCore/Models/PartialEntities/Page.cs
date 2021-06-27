using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class Page
    {
        public string Url
        {
            get
            {
                return "~/pages/" + this.Name;
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
            // Name:
            if (String.IsNullOrEmpty(Name))
                ValidationErrors.Add("Name", Resources.Messages.RequiredID);
            else
            {
                var rgx_name = new System.Text.RegularExpressions.Regex(Patterns.PageName);
                if (!rgx_name.IsMatch(Name))
                    ValidationErrors.Add("Name", Resources.Messages.RegularExpressionPageName);
                if (Name.Length > 50)
                    ValidationErrors.Add("Name", Resources.Messages.StringLengthGeneral);
            }
            _isValidated = true;
        }
        #endregion
    }
}