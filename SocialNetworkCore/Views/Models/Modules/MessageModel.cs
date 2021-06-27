using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class MessageModel
    {
        #region Properties:
        public UserIdentityModel SenderThumbIdentityModel { get; set; }
        public UserIdentityModel SenderNameIdentityModel { get; set; }
        public BoxMessage Message { get; set; }
        public string MessageText { get; set; }
        #endregion

        #region Methods:
        public static MessageModel Create(BoxMessage msg, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            var boxMsgOwner = mr.Get(msg.MemberId);
            var boxMsgOpponent = mr.Get(msg.OpponentId);
            var from_member = msg is InboxMessage ? boxMsgOpponent : boxMsgOwner;
            ///////////////////////////////////
            var msgModel = new MessageModel();
            msgModel.SenderThumbIdentityModel = UserIdentityModel.Create(from_member, 35, UserIdentityType.Thumb, "", "", "", "", context);
            msgModel.SenderNameIdentityModel = UserIdentityModel.Create(from_member, null, UserIdentityType.FullName, "", "", "", "", context);
            msgModel.Message = msg;
            msgModel.MessageText = msgModel.Message.MessageContent.Text;
            return msgModel;
        }
        #endregion
    }
}