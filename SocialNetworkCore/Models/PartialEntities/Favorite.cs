using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class Favorite
    {
        #region Private Variables:
        private ValidationErrorList _validationErrors = new ValidationErrorList();
        private bool _isValidated = false;
        #endregion

        #region Properties:
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

            //Hobbies:
            if (!String.IsNullOrEmpty(Hobbies))
            {
                if (Hobbies.Length > 500)
                    ValidationErrors.Add("Hobbies", Resources.Messages.StringLengthHobbies);
            }

            //FavoriteMusics:
            if (!String.IsNullOrEmpty(FavoriteMusics))
            {
                if (FavoriteMusics.Length > 500)
                    ValidationErrors.Add("FavoriteMusics", Resources.Messages.StringLengthFavoriteMusics);
            }

            //FavoriteMovies:
            if (!String.IsNullOrEmpty(FavoriteMovies))
            {
                if (FavoriteMovies.Length > 500)
                    ValidationErrors.Add("FavoriteMovies", Resources.Messages.StringLengthFavoriteMovies);
            }

            //FavoriteBooks:
            if (!String.IsNullOrEmpty(FavoriteBooks))
            {
                if (FavoriteBooks.Length > 500)
                    ValidationErrors.Add("FavoriteBooks", Resources.Messages.StringLengthFavoriteBooks);
            }

            //FavoriteQuotes:
            if (!String.IsNullOrEmpty(FavoriteQuotes))
            {
                if (FavoriteQuotes.Length > 500)
                    ValidationErrors.Add("FavoriteQuotes", Resources.Messages.StringLengthFavoriteQuotes);
            }

            _isValidated = true;
        }
        #endregion

        #region Static Methods:
        public static Favorite GetModelFromCollection(FormCollection form)
        {
            Favorite fav = new Favorite();
            fav.MemberId = form["Email"];
            fav.AgeFrom = (!String.IsNullOrEmpty(form["AgeFrom"]) ? Byte.Parse(form["AgeFrom"]) : (byte?)null);
            fav.AgeTo = (!String.IsNullOrEmpty(form["AgeTo"]) ? Byte.Parse(form["AgeTo"]) : (byte?)null);
            fav.MaritalStatus = (!String.IsNullOrEmpty(form["MaritalStatus"]) ? Byte.Parse(form["MaritalStatus"]) : (byte?)null);
            fav.Height = (!String.IsNullOrEmpty(form["Height"]) ? Byte.Parse(form["Height"]) : (byte?)null);
            fav.Weight = (!String.IsNullOrEmpty(form["Weight"]) ? Byte.Parse(form["Weight"]) : (byte?)null);
            fav.Behavior = form["Behavior"];
            fav.Fashion =
                ((!String.IsNullOrEmpty(form["Comfortable"]) && form["Comfortable"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Jean"]) && form["Jean"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Suit"]) && form["Suit"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Usual"]) && form["Usual"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Modern"]) && form["Modern"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["UpToDate"]) && form["UpToDate"] == "on") ? "1" : "0") +
                ((!String.IsNullOrEmpty(form["Variable"]) && form["Variable"] == "on") ? "1" : "0");
            fav.Hobbies = form["Hobbies"];
            fav.FavoriteMusics = form["FavoriteMusics"];
            fav.FavoriteMovies = form["FavoriteMovies"];
            fav.FavoriteBooks = form["FavoriteBooks"];
            fav.FavoriteQuotes = form["FavoriteQuotes"];
            return fav;
        }
        #endregion
    }
}