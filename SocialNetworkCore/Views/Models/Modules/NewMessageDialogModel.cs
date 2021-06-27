using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class NewMessageDialogModel
    {
        public List<Member> DefaultRecievers { get; set; }
        public MemberBoxModel MemberBoxModel { get; set; }

        public static NewMessageDialogModel Create(List<Member> defRecievers, System.Data.Objects.ObjectContext context)
        {
            var model = new NewMessageDialogModel();
            model.DefaultRecievers = defRecievers;
            model.MemberBoxModel = MemberBoxModel.Create("OpponentId", "FindInAssociatedMembers", "Members", 500, model.DefaultRecievers, "thumb", "Alias", context);
            return model;
        }
    }
}