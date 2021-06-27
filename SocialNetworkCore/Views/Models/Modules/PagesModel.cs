using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PagesModel
    {
        public List<SocialNetApp.Models.Page> Pages { get; set; }
        public List<SupportedLanguage> Languages { get; set; }

        public static PagesModel Create(System.Data.Objects.ObjectContext context)
        {
            var pr = new PagesRepository(context);
            ////////////////////////////////
            var model = new PagesModel();
            model.Pages = pr.GetAll().ToList();
            model.Languages = Configs.SupportedLanguages.ToList();
            return model;
        }
    }
}