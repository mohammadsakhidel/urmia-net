using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class AccountDeleteSchedule
    {
        public static AccountDeleteSchedule GetModelFromCollection(FormCollection form)
        {
            var model = new AccountDeleteSchedule();
            model.MemberId = form["MemberId"];
            model.Reason = form["Reason"];
            model.DateOfDelete = CoreHelpers.MyHelper.Now;
            model.Status = (byte)DeleteScheduleStatus.Scheduled;
            return model;
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
            //Reason:
            if (!String.IsNullOrEmpty(Reason))
            {
                if (Reason.Length > 1000)
                    ValidationErrors.Add("Reason", Resources.Messages.StringLengthGeneral);
            }
            _isValidated = true;
        }
        #endregion
    }
}