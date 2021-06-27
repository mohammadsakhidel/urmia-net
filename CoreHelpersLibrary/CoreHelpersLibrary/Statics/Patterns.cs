using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class Patterns
    {
        public static string Email
        {
            get
            {
                return Statics.GetValue("patterns", "Email");
            }
        }
        public static string Password
        {
            get
            {
                return Statics.GetValue("patterns", "Password");
            }
        }
        public static string RegisterationCode
        {
            get
            {
                return Statics.GetValue("patterns", "RegisterationCode");
            }
        }
        public static string Alias
        {
            get
            {
                return Statics.GetValue("patterns", "Alias");
            }
        }
        public static string AliasNonValidCharsRemover
        {
            get
            {
                return Statics.GetValue("patterns", "AliasNonValidCharsRemover");
            }
        }
        public static string MobNumber
        {
            get
            {
                return Statics.GetValue("patterns", "MobNumber");
            }
        }
        public static string PageName
        {
            get
            {
                return Statics.GetValue("patterns", "PageName");
            }
        }
        public static string FullName
        {
            get
            {
                return Statics.GetValue("patterns", "FullName");
            }
        }
        public static string AlternativeEmailActivationCode
        {
            get
            {
                return Statics.GetValue("patterns", "AlternativeEmailActivationCode");
            }
        }
    }
}