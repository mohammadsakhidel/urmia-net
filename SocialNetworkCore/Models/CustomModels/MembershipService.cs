using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SocialNetApp.Models
{
    public class MembershipService
    {
        public static bool ValidateUser(string userName, string password)
        {
            MembershipProvider _provider = Membership.Provider;
            return _provider.ValidateUser(userName, password);
        }

        public static void SetCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
    }
}