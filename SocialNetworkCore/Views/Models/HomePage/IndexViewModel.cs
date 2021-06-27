using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.HomePage
{
    public class IndexViewModel
    {
        public string PageName { get; set; }
        public PanelBarModel PanelBarModel { get; set; }
        public object PageModel { get; set; }

        public static IndexViewModel Create(Member member, string pageName, object pageModel, System.Data.Objects.ObjectContext context)
        {
            var model = new IndexViewModel();
            model.PageName = pageName;
            model.PageModel = pageModel;
            model.PanelBarModel = PanelBarModel.Create(member, context);
            return model;
        }
    }
}