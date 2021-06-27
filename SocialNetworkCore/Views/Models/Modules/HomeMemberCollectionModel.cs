using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class HomeMemberCollectionModel
    {
        public List<Member> Members { get; set; }
        public int LargeThumbIndex { get; set; }

        public static HomeMemberCollectionModel Create(System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            ////////////////////////////////////////////
            var model = new HomeMemberCollectionModel();
            model.Members = mr.GetMembersForHomeCollection().ToList();
            model.LargeThumbIndex = 12;
            return model;
        }
    }
}