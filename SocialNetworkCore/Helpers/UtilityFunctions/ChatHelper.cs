using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace CoreHelpers
{
    public static class ChatHelper
    {
        #region Public Methods:
        public static List<ChatSession> GetChatSessions(string memberId)
        {
            if (HttpContext.Current.Application["chat_sessions"] != null)
            {
                var all_sessions = GetChatSessions().Select(s => s.Value);
                return all_sessions.Where(s => s.Participants.Contains(memberId)).ToList();
            }
            else
            {
                return new List<ChatSession>();
            }
        }
        public static ChatSession BeginChat(List<string> participants)
        {
            if (ChatSessionExists(participants))
            {
                return GetChatSession(participants);
            }
            else
            {
                var session = new ChatSession();
                session.Id = GetNewRandomSessionId();
                session.Participants = participants;
                session.BeginTime = MyHelper.Now;
                session.Messages = new List<ChatMessage>();
                AddChatSession(session);
                return session;
            }
        }
        public static ChatSession GetChatSession(string id)
        {
            try
            {
                var sessions = GetChatSessions();
                return sessions[id];
            }
            catch
            {
                return null;
            }
        }
        public static List<ChatMessage> GetSessionMessages(string sessionId)
        {
            return GetChatSession(sessionId).Messages.OrderByDescending(m => m.DateOfSend).Take(Digits.MaxChatSessionMessages).OrderBy(m => m.DateOfSend).ToList();
        }
        public static List<ChatMessage> GetSessionMessages(string sessionId, DateTime olderThanDate)
        {
            return GetChatSession(sessionId).Messages
                .Where(m => m.DateOfSend < olderThanDate)
                .OrderByDescending(m => m.DateOfSend)
                .Take(Digits.MaxChatSessionMessages)
                .OrderBy(m => m.DateOfSend).ToList();
        }
        public static int GetSessionMessagesTotalCount(string sessionId)
        {
            return GetChatSession(sessionId).Messages.Count();
        }
        public static int GetNewSessionMessagesCount(string sessionId, string notFromMemberId)
        {
            return GetChatSession(sessionId).Messages.Where(m => m.From != notFromMemberId && m.Status == (byte)ChatMessageStatus.New).Count();
        }
        public static void RefreshChatSessions()
        {
            var sessions = GetChatSessions().Select(s => s.Value);
            foreach (var session in sessions)
            {
                if ((session.LastMessageDate.HasValue && session.LastMessageDate.Value.AddMinutes(Digits.ChatSessionWindowMinutes) < MyHelper.Now) ||
                    (!session.LastMessageDate.HasValue && session.BeginTime.AddMinutes(Digits.ChatSessionWindowMinutes) < MyHelper.Now))
                {
                    DeleteChatSession(session.Id);
                }
            }
        }
        public static ChatMessage AddChatMessage(ChatMessage msg)
        {
            msg.Id = GetNewRandomMessageId();
            var session = GetChatSession(msg.ChatSessionId);
            var msgs = session.Messages;
            msgs.Add(msg);
            session.Messages = msgs;
            SaveChatSession(session);
            return msg;
        }
        public static void SetSessionAsRead(string sessionId, string notFromMemberId)
        {
            var session = GetChatSession(sessionId);
            foreach (var msg in session.Messages)
            {
                if (msg.From != notFromMemberId)
                    msg.Status = (byte)ChatMessageStatus.ShowedToReciever;
            }
            SaveChatSession(session);
        }
        public static void SetMessageAsRead(ChatMessage message)
        {
            var session = GetChatSession(message.ChatSessionId);
            foreach (var msg in session.Messages)
            {
                if (msg.Id == message.Id)
                    msg.Status = (byte)ChatMessageStatus.ShowedToReciever;
            }
            SaveChatSession(session);
        }
        public static bool HasNewMessage(string memberId)
        {
            var sessions = GetChatSessions(memberId);
            foreach (var s in sessions)
            {
                if (IsNewMessageInSession(s.Id))
                    return true;
            }
            return false;
        }
        public static bool IsMoreMessages(string sessionId, DateTime olderThanDate)
        {
            return GetChatSession(sessionId).Messages.Where(m => m.DateOfSend < olderThanDate).Any();
        }
        #endregion

        #region Private Methods:
        private static string GetNewRandomSessionId()
        {
            var randomId = MyHelper.GetRandomString(10, true);
            if (!ChatSessionExists(randomId))
            {
                return randomId;
            }
            else
            {
                return GetNewRandomSessionId();
            }
        }
        private static string GetNewRandomMessageId()
        {
            var randomId = MyHelper.GetRandomString(25, true);
            return randomId;
        }
        private static bool ChatSessionExists(List<string> participants)
        {
            var sessions = GetChatSessions().Select(s => s.Value);
            return sessions.Where(s => !participants.Except(s.Participants).Any()).Any();
        }
        private static bool ChatSessionExists(string id)
        {
            var sessions = GetChatSessions().Select(s => s.Value);
            return sessions.Where(s => s.Id == id).Any();
        }
        private static void AddChatSession(ChatSession session)
        {
            var sessions = GetChatSessions();
            sessions.Add(session.Id, session);
            SaveChatSessions(sessions);
        }
        private static void SaveChatSessions(Dictionary<string, ChatSession> sessions)
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["chat_sessions"] = sessions;
            HttpContext.Current.Application.UnLock();
        }
        private static void SaveChatSession(ChatSession session)
        {
            var sessions = GetChatSessions();
            sessions[session.Id] = session;
            SaveChatSessions(sessions);
        }
        private static ChatSession GetChatSession(List<string> participants)
        {
            try
            {
                var sessions = GetChatSessions().Select(s => s.Value);
                return sessions.Single(s => !participants.Except(s.Participants).Any());
            }
            catch
            {
                return null;
            }
        }
        private static Dictionary<string, ChatSession> GetChatSessions()
        {
            if (HttpContext.Current.Application["chat_sessions"] != null)
            {
                return (Dictionary<string, ChatSession>)HttpContext.Current.Application["chat_sessions"];
            }
            else
            {
                return new Dictionary<string, ChatSession>();
            }
        }
        private static void DeleteChatSession(string sessionId)
        {
            
        }
        private static bool IsNewMessageInSession(string sessionId)
        {
            var msgs = GetSessionMessages(sessionId);
            foreach (var m in msgs)
            {
                if (m.Status == (byte)ChatMessageStatus.New)
                    return true;
            }
            return false;
        }
        #endregion
    }
}