using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    [Serializable]
    public class ChatSession
    {
        public string Id { get; set; }
        public List<string> Participants { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? LastMessageDate
        {
            get
            {
                var msgs = ChatHelper.GetSessionMessages(this.Id).OrderBy(m => m.DateOfSend);
                return msgs.Any() ? msgs.Last().DateOfSend : (DateTime?)null;
            }
        }
        public List<ChatMessage> Messages { get; set; }
        //
        public int GetNewMessagesCount(string notFromMember)
        {
            var new_count = ChatHelper.GetNewSessionMessagesCount(this.Id, notFromMember);
            return new_count;
        }
    }
}