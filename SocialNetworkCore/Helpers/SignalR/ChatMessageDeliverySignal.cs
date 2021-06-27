using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class ChatMessageDeliverySignal
    {
        public string ConnectionId { get; set; }
        public string SessionId { get; set; }
        public string MessageId { get; set; }
    }
}