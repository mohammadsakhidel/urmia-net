using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using System.Web.Routing;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class MessagesModel
    {
        #region Properties:
        public List<Tuple<Conversation, UserIdentityModel, UserIdentityModel>> ConversationRecords { get; set; }
        public string SelectedConversationId { get; set; }
        public Conversation SelectedConversation { get; set; }
        public bool IsAConversationSelected { get; set; }
        public bool HasSelectedConversationAnyMessages { get; set; }
        public bool HasCurrentUserParticipated { get; set; }
        public List<MessageModel> SelectedConversationMessageModels { get; set; }
        public MessageSenderModel MessageSenderModel { get; set; }
        public int UpdatedUnreadsCount { get; set; }
        #endregion

        #region Methods:
        public static MessagesModel Create(Member member, RouteValueDictionary rData, System.Data.Objects.ObjectContext context)
        {
            var msgr = new MessagesRepository(context);
            context = msgr.Context;
            //**** conversation records:
            var convRecords = new List<Tuple<Conversation, UserIdentityModel, UserIdentityModel>>();
            var conversations = member.Conversations(context);
            foreach (var c in conversations)
            {
                var uidWithThumb = UserIdentityModel.Create(c.ThatMember, 50, UserIdentityType.Thumb, "", "", "", "", context);
                var uidWithName = UserIdentityModel.Create(c.ThatMember, null, UserIdentityType.FullName, "", "", "", "", context);
                convRecords.Add(new Tuple<Conversation, UserIdentityModel, UserIdentityModel>(c, uidWithThumb, uidWithName));
            }
            //***************************************
            MessagesModel model = new MessagesModel();
            model.SelectedConversationId = rData["id"] != null ? rData["id"].ToString() : "";
            model.ConversationRecords = convRecords;
            model.SelectedConversation = !String.IsNullOrEmpty(model.SelectedConversationId) ? msgr.FindConversation(HttpContext.Current.User.Identity.Name, model.SelectedConversationId) : null;
            model.IsAConversationSelected = model.SelectedConversation != null;
            model.HasSelectedConversationAnyMessages = model.IsAConversationSelected && model.SelectedConversation.Messages != null && model.SelectedConversation.Messages.Any();
            model.HasCurrentUserParticipated = model.IsAConversationSelected && model.SelectedConversation.HasParticipated(HttpContext.Current.User.Identity.Name);
            //**** message models:
            var msgModels = new List<MessageModel>();
            if (model.HasSelectedConversationAnyMessages)
            {
                foreach (var msg in model.SelectedConversation.Messages)
                {
                    if (msg is InboxMessage)
                    {
                        // set inbox msg as read:
                        msg.Status = (byte)InboxMessageStatus.Read;
                        msg.StatusInfo = MyHelper.Now.ToString();
                        // set associated outbox msg as delivered:
                        var associated_outbox_msg = msgr.GetOutboxMessage(((InboxMessage)msg).AssociatedOutboxMsgId.Value);
                        if (associated_outbox_msg != null)
                        {
                            associated_outbox_msg.Status = (byte)OutboxMessageStatus.Delivered;
                            associated_outbox_msg.StatusInfo = MyHelper.Now.ToString();
                        }
                        msgr.Save();
                    }
                    var msgModel = MessageModel.Create(msg, null);
                    msgModels.Add(msgModel);
                }
            }
            //****
            model.SelectedConversationMessageModels = msgModels;
            model.MessageSenderModel = MessageSenderModel.Create(model.SelectedConversationId, null, null, "conv_reply_msg", null);
            //**** updated unreads count: 
            model.UpdatedUnreadsCount = msgr.GetUnreadMessagesCount(HttpContext.Current.User.Identity.Name);
            return model;
        }
        #endregion
    }
}