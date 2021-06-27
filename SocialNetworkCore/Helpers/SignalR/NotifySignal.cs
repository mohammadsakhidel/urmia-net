using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace CoreHelpers
{
    public class NotifySignal
    {
        public NotifySignalType Type { get; set; }
        public string ConnectionId { get; set; }
    }
}