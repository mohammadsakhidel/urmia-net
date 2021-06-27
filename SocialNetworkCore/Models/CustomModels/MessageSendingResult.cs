using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public class MessageSendingResult
    {
        public Member Receiver { get; set; }
        public bool SendStatus { get; set; }
        public string Message { get; set; }
        public BoxMessage SentMessage { get; set; }
    }
}