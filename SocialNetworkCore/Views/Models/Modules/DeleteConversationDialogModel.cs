using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class DeleteConversationDialogModel
    {
        public Conversation Conversation { get; set; }
        public Member ConversationOtherSide { get; set; }

        public static DeleteConversationDialogModel Create(Conversation conv, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //**********************************************
            var model = new DeleteConversationDialogModel();
            model.Conversation = conv;
            model.ConversationOtherSide = mr.Get(conv.ThatMemberId);
            return model;
        }
    }
}