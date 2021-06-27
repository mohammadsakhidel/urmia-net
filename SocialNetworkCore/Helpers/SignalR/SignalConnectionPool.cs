using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    [Serializable]
    public class SignalConnectionPool : Dictionary<string, SignalConnection>
    {
        public SignalConnectionPool()
            : base()
        {
        }

        public SignalConnectionPool(int capacity)
            : base(capacity)
        {
        }
    }
}