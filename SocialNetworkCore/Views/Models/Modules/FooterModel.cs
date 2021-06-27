using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Modules
{
    public class FooterModel
    {
        public bool HasLogedin
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public static FooterModel Create(System.Data.Objects.ObjectContext context)
        {
            return new FooterModel();
        }
    }
}