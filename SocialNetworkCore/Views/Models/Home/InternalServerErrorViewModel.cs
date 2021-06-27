using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Home
{
    public class InternalServerErrorViewModel
    {
        public static InternalServerErrorViewModel Create(System.Data.Objects.ObjectContext context)
        {
            return new InternalServerErrorViewModel();
        }
    }
}