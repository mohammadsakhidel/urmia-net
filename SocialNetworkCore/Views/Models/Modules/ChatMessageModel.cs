using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository
;using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ChatMessageModel
    {
        public ChatMessage ChatMessage { get; set; }
        public Member FromMember { get; set; }
        public string CssExtension { get; set; }
        public UserIdentityModel UserIdentityModel { get; set; }

        public static ChatMessageModel Create(ChatMessage msg, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            //**********************************
            var model = new ChatMessageModel();
            model.ChatMessage = msg;
            model.FromMember = mr.Get(msg.From);
            model.CssExtension = (!String.IsNullOrEmpty(model.ChatMessage.CssExtension) ? model.ChatMessage.CssExtension : (HttpContext.Current.User.Identity.Name != model.ChatMessage.From ? "_otherside" : ""));
            model.UserIdentityModel = UserIdentityModel.Create(model.FromMember, 35, UserIdentityType.Thumb, "", "", "", "", context);
            return model;
        }
    }
}