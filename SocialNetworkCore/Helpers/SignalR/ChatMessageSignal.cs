using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;

namespace CoreHelpers
{
    public class ChatMessageSignal
    {
        public string ConnectionId { get; set; }
        public ChatMessage Message { get; set; }
        public string SessionsView { get; set; }
    }
}