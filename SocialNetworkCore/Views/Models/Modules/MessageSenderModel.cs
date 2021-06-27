using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class MessageSenderModel
    {
        #region Properties:
        public string RandomId { get; set; }
        public string ConversationId { get; set; }
        public bool IsEnabled { get; set; }
        public int Width { get; set; }
        public int TextAreaHeight { get; set; }
        public string PositioningCssClass { get; set; }
        #endregion

        #region Methods:
        public static MessageSenderModel Create(string convId, int? width, int? textAreaHeight, string cssStyle, System.Data.Objects.ObjectContext context)
        {
            var model = new MessageSenderModel();
            model.RandomId = MyHelper.GetRandomString(5, true);
            model.ConversationId = convId;
            model.IsEnabled = !String.IsNullOrEmpty(model.ConversationId);
            model.Width = width.HasValue ? width.Value : 460;
            model.TextAreaHeight = textAreaHeight.HasValue ? textAreaHeight.Value : 75;
            model.PositioningCssClass = cssStyle;
            return model;
        }
        #endregion
    }
}