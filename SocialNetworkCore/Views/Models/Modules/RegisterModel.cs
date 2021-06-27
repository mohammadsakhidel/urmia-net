using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class RegisterModel
    {
        public string PositioningCssClass { get; set; }
        public RegisterType Type { get; set; }

        public static RegisterModel Create(RegisterType regType, string posCssClass, System.Data.Objects.ObjectContext context)
        {
            var model = new RegisterModel();
            model.Type = regType;
            model.PositioningCssClass = posCssClass;
            return model;
        }
    }
}