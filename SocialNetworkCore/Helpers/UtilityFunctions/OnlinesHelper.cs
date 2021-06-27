using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models.Repository;

namespace CoreHelpers
{
    public class OnlinesHelper
    {
        #region Publics:
        public static bool IsOnline(string memberId)
        {
            Refresh(HttpContext.Current);
            var cr = new ConnectionsRepository();
            return cr.IsOnline(memberId);
        }
        public static void Refresh(HttpContext context)
        {
            var cr = new ConnectionsRepository();
            cr.Refresh(context);
        }
        public static void SignOut(string username)
        {
            var cr = new ConnectionsRepository();
            cr.DeleteByMemberId(username);
            System.Web.Security.FormsAuthentication.SignOut();
        }
        #endregion
    }
}