using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Views.Models.Members
{
    public class IndexViewModel
    {
        public PeopleModel PeopleModel { get; set; }

        public static IndexViewModel Create( System.Data.Objects.ObjectContext context)
        {
            var model = new IndexViewModel();
            model.PeopleModel = PeopleModel.Create(context);
            return model;
        }
    }
}