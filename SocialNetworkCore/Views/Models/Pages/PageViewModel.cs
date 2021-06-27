using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;
using SocialNetApp.Views.Models.Layouts;

namespace SocialNetApp.Views.Models.Pages
{
    public class PageViewModel
    {
        public Page Page { get; set; }
        public PageContent PageContent { get; set; }

        public static PageViewModel Create(Page pg, System.Data.Objects.ObjectContext context)
        {
            var model = new PageViewModel();
            model.Page = pg;
            model.PageContent = pg.PageContents.FirstOrDefault(pgc => pgc.Language == SiteViewModel.Language);
            return model;
        }
    }
}