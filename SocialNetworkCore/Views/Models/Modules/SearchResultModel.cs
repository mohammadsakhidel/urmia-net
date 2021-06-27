using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class SearchResultModel
    {
        public List<MemberBriefModel> MemberBriefModels { get; set; }
        public EmptyListModel EmptyListModel { get; set; }

        public static SearchResultModel Create(List<Member> members, System.Data.Objects.ObjectContext context)
        {
            var model = new SearchResultModel();
            //--- member brief models:
            var briefModels = new List<MemberBriefModel>();
            for (var i = 0; i < members.Count(); i++)
            {
                var briefModel = MemberBriefModel.Create(members[i], i, 2, context);
                briefModels.Add(briefModel);
            }
            //---
            model.MemberBriefModels = briefModels;
            model.EmptyListModel = EmptyListModel.Create(context);
            return model;
        }
    }
}