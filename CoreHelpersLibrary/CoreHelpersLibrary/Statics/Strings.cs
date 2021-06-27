using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class Strings
    {
        public static string TitleSeparator
        {
            get
            {
                return Statics.GetValue("strings", "TitleSeparator");
            }
        }
    }
}