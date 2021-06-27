using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Modules
{
    public class CustomErrorModel
    {
        public string Message { get; set; }

        public static CustomErrorModel Create(string message, System.Data.Objects.ObjectContext context)
        {
            var model = new CustomErrorModel();
            model.Message = message;
            return model;
        }
    }
}