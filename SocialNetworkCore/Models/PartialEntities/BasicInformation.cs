using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CoreHelpers;
using System.Web.Mvc;

namespace SocialNetApp.Models
{
    public partial class BasicInformation
    {
        #region Private Variables:
        private ValidationErrorList _validationErrors = new ValidationErrorList();
        private bool _isValidated = false;
        #endregion

        #region Properties:
        public bool HasFashion
        {
            get
            {
                bool res = false;
                if (!String.IsNullOrEmpty(this.Fashion))
                {
                    foreach (var c in Fashion)
                    {
                        if (c == '1')
                            return true;
                    }
                }
                return res;
            }
        }
        public string FashionsText
        {
            get
            {
                string txt = "";
                if (HasFashion)
                {
                    var j = 0;
                    for (var i = 0; i < Fashion.Length; i++)
                    {
                        if (Fashion[i] == '1')
                        {
                            txt += (j > 0 ? " - " : "") + Dictionaries.Fashions[i];
                            j++;
                        }
                    }
                }
                return txt;
            }
        }
        public string MaritalStatusText
        {
            get
            {
                string txt = "";
                if (this.MaritalStatus == (byte)CoreHelpers.MaritalStatus.Single)
                    txt = Resources.Words.Single;
                if (this.MaritalStatus == (byte)CoreHelpers.MaritalStatus.Married)
                    txt = Resources.Words.Married;
                if (this.MaritalStatus == (byte)CoreHelpers.MaritalStatus.InRelationship)
                    txt = Resources.Words.InRelationship;
                if (this.MaritalStatus == (byte)CoreHelpers.MaritalStatus.Divorced)
                    txt = Resources.Words.Divorced;
                return txt;
            }
        }
        public string LivingRegionName
        {
            get
            {
                var txt = "";
                if (LivingRegion == (byte)CoreHelpers.LivingRegion.North)
                    txt = Resources.Words.LivingRegionNorth;
                else if (LivingRegion == (byte)CoreHelpers.LivingRegion.East)
                    txt = Resources.Words.LivingRegionEast;
                else if (LivingRegion == (byte)CoreHelpers.LivingRegion.South)
                    txt = Resources.Words.LivingRegionSouth;
                else if (LivingRegion == (byte)CoreHelpers.LivingRegion.West)
                    txt = Resources.Words.LivingRegionWest;
                else if (LivingRegion == (byte)CoreHelpers.LivingRegion.Abroad)
                    txt = Resources.Words.LivingRegionAbroad;
                return txt;
            }
        }
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
        #endregion

        #region Object Methods:
        public void Validate()
        {
            //MemberId:
            Regex rgx_email = new Regex(Patterns.Email);
            if (String.IsNullOrEmpty(MemberId))
                ValidationErrors.Add("Email", Resources.Messages.RequiredEmail);
            else
            {
                if (MemberId.Length > 50)
                    ValidationErrors.Add("Email", Resources.Messages.StringLengthEmail);
                if (!rgx_email.IsMatch(MemberId))
                    ValidationErrors.Add("Email", Resources.Messages.RegularExpressionEmail);
            }

            //MaritalStatus:
            if (MaritalStatus.HasValue)
            {
                if (MaritalStatus.Value <= 0 || MaritalStatus.Value > Enum.GetNames(typeof(MaritalStatus)).Length)
                    ValidationErrors.Add("MaritalStatus", Resources.Messages.RangeMaritalStatus);
            }

            //Fashion:
            if (!String.IsNullOrEmpty(Fashion))
            {
                if (Fashion.Length > 10)
                    ValidationErrors.Add("Fashion", Resources.Messages.StringLengthFashion);
            }

            //Behavior:
            if (!String.IsNullOrEmpty(Behavior))
            {
                if (Behavior.Length > 100)
                    ValidationErrors.Add("Behavior", Resources.Messages.StringLengthBehavior);
            }

            //AboutMe:
            if (!String.IsNullOrEmpty(AboutMe))
            {
                if (AboutMe.Length > 1000)
                    ValidationErrors.Add("AboutMe", Resources.Messages.StringLengthAboutMe);
            }

            _isValidated = true;
        }
        #endregion

        #region Static Methods:
        public static BasicInformation GetModelFromCollection(FormCollection form)
        {
            BasicInformation info = new BasicInformation();
            info.MemberId = form["Email"];
            info.MaritalStatus = (!String.IsNullOrEmpty(form["MaritalStatus"]) ? Byte.Parse(form["MaritalStatus"]) : (byte?)null);
            info.Height = (!String.IsNullOrEmpty(form["Height"]) ? Byte.Parse(form["Height"]) : (byte?)null);
            info.Weight = (!String.IsNullOrEmpty(form["Weight"]) ? Byte.Parse(form["Weight"]) : (byte?)null);
            info.Fashion =
                ((!String.IsNullOrEmpty(form["Comfortable"]) && form["Comfortable"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Jean"]) && form["Jean"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Suit"]) && form["Suit"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Usual"]) && form["Usual"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Modern"]) && form["Modern"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["UpToDate"]) && form["UpToDate"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Variable"]) && form["Variable"] == "on") ? "1" : "0");
            info.Behavior = form["Behavior"];
            info.AboutMe = form["AboutMe"];
            info.LivingRegion = (!String.IsNullOrEmpty(form["LivingRegion"]) ? (Convert.ToByte(form["LivingRegion"]) > 0 ? Convert.ToByte(form["LivingRegion"]) : (byte?)null) : (byte?)null);
            info.LivingCity = form["LivingCity"];
            return info;
        }
        #endregion
    }
}