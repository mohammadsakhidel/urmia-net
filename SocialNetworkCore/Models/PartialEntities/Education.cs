using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public partial class Education
    {
        public string Text
        {
            get
            {
                string txt = 
                    EducationLevelText + " " +
                    Resources.Words.InFieldOf + " " +
                    EducationBranch;
                if (!String.IsNullOrEmpty(EducationLocation))
                {
                    txt += 
                    " " + Resources.Words.In + 
                    " " + EducationLocation;
                }
                if (FromYear.HasValue && ToYear.HasValue)
                {
                    txt +=
                    " " + Resources.Words.From +
                    " " + FromYear +
                    " " + Resources.Words.To +
                    " " + ToYear;
                }
                return txt;
            }
        }
        public string EducationLevelText
        {
            get
            {
                return CoreHelpers.Dictionaries.EducationLevels[this.EducationLevel];
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
            //EducationLevel:
            if (EducationLevel <= 0)
                ValidationErrors.Add("EducationLevel", Resources.Messages.RequiredEducationLevel);
            //EducationBranch:
            if (String.IsNullOrEmpty(EducationBranch))
                ValidationErrors.Add("EducationBranch", Resources.Messages.RequiredEducationBranch);
            if (!String.IsNullOrEmpty(EducationBranch))
            {
                if (EducationBranch.Length > 50)
                    ValidationErrors.Add("EducationBranch", Resources.Messages.StringLengthGeneral);
            }
            //EducationLocation:
            if (!String.IsNullOrEmpty(EducationLocation))
            {
                if (EducationLocation.Length > 50)
                    ValidationErrors.Add("EducationLocation", Resources.Messages.StringLengthGeneral);
            }

            _isValidated = true;
        }
        #endregion
    }
}