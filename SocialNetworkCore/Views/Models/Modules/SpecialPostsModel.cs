using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class SpecialPostsModel
    {
        public List<SpecialPostModel> SpecialPostModels { get; set; }

        public static SpecialPostsModel Create(System.Data.Objects.ObjectContext context)
        {
            var pr = new PostsRepository(context);
            context = pr.Context;
            var spPosts = pr.GetSpecialPosts().ToList();
            var spPostModels = new List<SpecialPostModel>();
            foreach (var sp in spPosts)
            {
                var spModel = SpecialPostModel.Create(sp, context);
                spPostModels.Add(spModel);
            }
            ///////////////////////////////
            var model = new SpecialPostsModel();
            model.SpecialPostModels = spPostModels;
            return model;
        }
    }
}