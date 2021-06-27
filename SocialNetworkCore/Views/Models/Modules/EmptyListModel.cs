using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Modules
{
    public class EmptyListModel
    {
        public static EmptyListModel Create(System.Data.Objects.ObjectContext context)
        {
            //nothing
            return new EmptyListModel();
        }
    }
}