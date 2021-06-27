using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class AccountSetting
    {
        public static AccountSetting Default
        {
            get
            {
                var def = new AccountSetting();
                def.Language = Configs.SupportedLanguages.First().Code;
                def.MobNumber = "";
                def.AlternativeEmail = "";
                def.AlternativeEmailApproved = false;
                def.AlternativeEmailActivationCode = "";
                return def;
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
            if (!String.IsNullOrEmpty(MobNumber))
            {
                if (MobNumber.Length > 20)
                    ValidationErrors.Add("MobNumber", Resources.Messages.StringLengthGeneral);
                System.Text.RegularExpressions.Regex rgx_mob = new System.Text.RegularExpressions.Regex(Patterns.MobNumber);
                if (!rgx_mob.IsMatch(MobNumber))
                {
                    ValidationErrors.Add("MobNumber", Resources.Messages.RegularExpressionMobNumber);
                }
            }
            //AlternativeEmail:
            if (!String.IsNullOrEmpty(AlternativeEmail))
            {
                var rgx_email = new System.Text.RegularExpressions.Regex(Patterns.Email);
                if (!rgx_email.IsMatch(AlternativeEmail))
                {
                    ValidationErrors.Add("AlternativeEmail", Resources.Messages.RegularExpressionEmail);
                }
            }
            _isValidated = true;
        }
        #endregion
    }
}