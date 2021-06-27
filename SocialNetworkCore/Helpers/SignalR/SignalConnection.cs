using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    [Serializable]
    public class SignalConnection
    {
        public string Id { get; set; }
        public string MemberId { get; set; }
        public DateTime DateOfEstablish { get; set; }
    }
}