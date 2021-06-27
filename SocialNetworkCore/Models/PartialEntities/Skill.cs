using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public partial class Skill
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
            //SkillTitle:
            if (String.IsNullOrEmpty(SkillTitle))
                ValidationErrors.Add("SkillTitle", Resources.Messages.RequiredSkillTitle);
            if (!String.IsNullOrEmpty(SkillTitle))
            {
                if (SkillTitle.Length > 100)
                    ValidationErrors.Add("SkillTitle", Resources.Messages.StringLengthGeneral);
            }
            _isValidated = true;
        }
        #endregion
    }
}