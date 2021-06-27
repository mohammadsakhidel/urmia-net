using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class HmiMessagesModel
    {
        #region Properties:
        public List<Tuple<BoxMessage, Member, UserIdentityModel, UserIdentityModel>> UnreadMessagesRecords { get; set; }
        #endregion

        #region Methods:
        public static HmiMessagesModel Create(System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            var msg_rep = new MessagesRepository(context);
            var messages = msg_rep.GetUnreadMessages(HttpContext.Current.User.Identity.Name, Digits.MaxHmiItems).ToList();
            var records = new List<Tuple<BoxMessage, Member, UserIdentityModel, UserIdentityModel>>();
            foreach (var msg in messages)
            {
                var sender = mr.Get(msg.OpponentId);
                var thumbUidModel = UserIdentityModel.Create(sender, 40, UserIdentityType.Thumb, "", "", "", "", context);
                var nameUidModel = UserIdentityModel.Create(sender, null, UserIdentityType.FullName, "", "", "", "", context);
                records.Add(new Tuple<BoxMessage, Member, UserIdentityModel, UserIdentityModel>(msg, sender, thumbUidModel, nameUidModel));
            }
            //*********************************
            var model = new HmiMessagesModel();
            model.UnreadMessagesRecords = records;
            return model;
        }
        #endregion
    }
}