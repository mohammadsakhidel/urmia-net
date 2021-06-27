using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public class Conversation
    {
        #region Properties:
        public IEnumerable<BoxMessage> Messages { get; set; }
        public string ThisMemberId { get; set; }
        public string ThatMemberId { get; set; }
        public DateTime LastMessageDate { get; set; }
        public string BriefText { get; set; }
        public int UnreadsCount { get; set; }
        public Member ThisMember { get; set; }
        public Member ThatMember { get; set; }

        // ReadOnly Properties:
        public string Id
        {
            get
            {
                return ThatMember.Alias;
            }
        }
        #endregion

        #region Methods:
        public bool HasParticipated(string userName)
        {
            return userName == ThisMember.Email || userName == ThatMember.Email;
        }
        #endregion
    }
}