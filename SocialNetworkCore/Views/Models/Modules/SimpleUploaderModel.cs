using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Modules
{
    public class SimpleUploaderModel
    {
        public static SimpleUploaderModel Create(System.Data.Objects.ObjectContext context)
        {
            return new SimpleUploaderModel();
        }
    }
}