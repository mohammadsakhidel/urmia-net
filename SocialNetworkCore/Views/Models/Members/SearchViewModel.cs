using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Views.Models.Members
{
    public class SearchViewModel
    {
        public SearchMembersModel SearchMembersModel { get; set; }

        public static SearchViewModel Create(System.Data.Objects.ObjectContext context)
        {
            var model = new SearchViewModel();
            model.SearchMembersModel = SearchMembersModel.Create(HttpContext.Current.Request.QueryString, context);
            return model;
        }
    }
}