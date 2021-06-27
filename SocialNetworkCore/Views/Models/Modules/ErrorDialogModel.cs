using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Views.Models.Modules
{
    public class ErrorDialogModel
    {
        public string Message { get; set; }

        public static ErrorDialogModel Create(string message, System.Data.Objects.ObjectContext context)
        {
            var model = new ErrorDialogModel();
            model.Message = message;
            return model;
        }
    }
}