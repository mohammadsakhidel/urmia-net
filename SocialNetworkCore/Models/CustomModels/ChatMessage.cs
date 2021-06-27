using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using System.Web.Mvc;
using System.IO;

namespace SocialNetApp.Models
{
    [Serializable]
    public class ChatMessage
    {
        #region private variebles
        #endregion
        public string Id { get; set; }
        public string ChatSessionId { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime DateOfSend { get; set; }
        public byte Status { get; set; }
        public string PartialView { get; set; }
        public List<string> Recievers
        {
            get
            {
                var session = ChatHelper.GetChatSession(this.ChatSessionId);
                var other_people_in_session = session.Participants.Except(new List<string>() { this.From });
                return other_people_in_session.ToList();
            }
        }
        public string CssExtension { get; set; }
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
            if (String.IsNullOrEmpty(Text))
                ValidationErrors.Add("Text", Resources.Messages.RequiredMessageText);
            else
            {
                if (Text.Length > Digits.MaxChatMessageLength)
                    ValidationErrors.Add("Text", Resources.Messages.StringLengthGeneral);
            }
            _isValidated = true;
        }
        #endregion
        #region PRIVATE METHODS:
        #endregion
    }
}