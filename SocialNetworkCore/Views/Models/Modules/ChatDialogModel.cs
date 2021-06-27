using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ChatDialogModel
    {
        public string SessionId { get; set; }
        public ChatSession Session { get; set; }
        public List<ChatMessageModel> SessionMessageModels { get; set; }
        public int TotalMessagesCount { get; set; }
        public Member FromMember { get; set; }
        public string OlderThanDateString { get; set; }

        public static ChatDialogModel Create(string sessionId, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //*********************
            var model = new ChatDialogModel();
            model.SessionId = sessionId;
            model.Session = ChatHelper.GetChatSession(model.SessionId);
            //---- message models:
            var chatMessages = ChatHelper.GetSessionMessages(model.SessionId);
            var chatMessageModels = new List<ChatMessageModel>();
            foreach (var msg in chatMessages)
            {
                var msgModel = ChatMessageModel.Create(msg, mr.Context);
                chatMessageModels.Add(msgModel);
                //--- mark as read:
                SocialNetHub hub = new SocialNetHub();
                hub.SetMessageAsRead(msg, true);
            }
            //ChatHelper.SetSessionAsRead(Model.SessionId, User.Identity.Name);
            //----
            model.SessionMessageModels = chatMessageModels;
            model.TotalMessagesCount = ChatHelper.GetSessionMessagesTotalCount(model.SessionId);
            model.FromMember = mr.Get(HttpContext.Current.User.Identity.Name);
            model.OlderThanDateString = model.SessionMessageModels.Any() ? model.SessionMessageModels.First().ChatMessage.DateOfSend.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            return model;
        }
    }
}