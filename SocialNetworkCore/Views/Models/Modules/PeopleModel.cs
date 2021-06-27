using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class PeopleModel
    {
        public int PageIndex { get; set; }
        public List<MemberBriefModel> MemberBriefModels { get; set; }
        public int MembersCount { get; set; }
        public int PagesCount { get; set; }
        public bool NextVisible { get; set; }
        public bool PreviousVisible { get; set; }

        public static PeopleModel Create(System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            //***************************************
            var model = new PeopleModel();
            model.PageIndex = HttpContext.Current.Request.QueryString["page"] != null ? Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]) : 0;
            //---- member brief models:
            var members = mr.GetAllActiveMembers(model.PageIndex).ToList();
            var briefModels = new List<MemberBriefModel>();
            for (var i = 0; i < members.Count(); i++ )
            {
                var m = members[i];
                var briefModel = MemberBriefModel.Create(m, i, 3, context);
                briefModels.Add(briefModel);
            }
            //----
            model.MemberBriefModels = briefModels;
            model.MembersCount = mr.GetAllActiveMembersCount();
            model.PagesCount = (int)(model.MembersCount / Digits.PeoplePageSize) + (model.MembersCount % Digits.PeoplePageSize == 0 ? 0 : 1);
            model.NextVisible = model.PageIndex < model.PagesCount - 1;
            model.PreviousVisible = model.PageIndex > 0;
            return model;
        }
    }
}