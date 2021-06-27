using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ChatSessionsModel
    {
        public string ActiveSessionId { get; set; }
        public string Viewer { get; set; }
        public bool IsVisible { get; set; }
        public List<Tuple<ChatSession, UserIdentityModel, int>> SessionRecords { get; set; }

        public static ChatSessionsModel Create(List<ChatSession> sessions, string activeSessionId, string viewerId, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            //***********************
            var model = new ChatSessionsModel();
            model.IsVisible = sessions != null && sessions.Any();
            if (model.IsVisible)
            {
                model.ActiveSessionId = activeSessionId;
                model.Viewer = !String.IsNullOrEmpty(viewerId) ? viewerId : HttpContext.Current.User.Identity.Name;
                //----- session records:
                var sessionRecords = new List<Tuple<ChatSession, UserIdentityModel, int>>();
                foreach (var session in sessions)
                {
                    var member = mr.Get(session.Participants.Where(p => p != model.Viewer).First());
                    var memberIdentityModel = UserIdentityModel.Create(member, 35, UserIdentityType.Thumb, "", "", "", "", context);
                    var newmsgsCount = session.GetNewMessagesCount(model.Viewer);
                    var sessionRec = new Tuple<ChatSession, UserIdentityModel, int>(session, memberIdentityModel, newmsgsCount);
                    sessionRecords.Add(sessionRec);
                }
                //----
                model.SessionRecords = sessionRecords;
            }
            return model;
        }
    }
}