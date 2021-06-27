using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Home
{
    public class PageNotFoundErrorViewModel
    {
        public static PageNotFoundErrorViewModel Create(System.Data.Objects.ObjectContext context)
        {
            return new PageNotFoundErrorViewModel();
        }
    }
}