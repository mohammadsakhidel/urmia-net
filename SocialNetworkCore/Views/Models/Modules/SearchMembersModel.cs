using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;
using System.Collections.Specialized;

namespace SocialNetApp.Views.Models.Modules
{
    public class SearchMembersModel
    {
        public List<MemberBriefModel> RandomMemberBriefModels { get; set; }
        public byte? LivingRegion { get; set; }
        public string LivingCity { get; set; }
        public bool Invoke { get; set; }

        public static SearchMembersModel Create(NameValueCollection qs, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            //****************************************
            var model = new SearchMembersModel();
            //--- initial random member brief modules:
            var randomMembers = mr.GetRandomActiveMembers(Digits.SearchMembersDefaultSize, false, new List<string> { HttpContext.Current.User.Identity.Name }).ToList();
            var briefModels = new List<MemberBriefModel>();
            for (var i = 0; i < randomMembers.Count(); i++)
            {
                var briefModel = MemberBriefModel.Create(randomMembers[i], i, 2, context);
                briefModels.Add(briefModel);
            }
            //---
            model.RandomMemberBriefModels = briefModels;
            model.LivingRegion = (!String.IsNullOrEmpty(qs["region"]) && 
                MyHelper.IsByte(qs["region"].ToString()) && 
                Convert.ToByte(qs["region"]) > 0) ? 
                Convert.ToByte(qs["region"]) : 
                (byte?)null;
            model.LivingCity = (!String.IsNullOrEmpty(qs["city"]) ? qs["city"].ToString() : "");
            model.Invoke = (!String.IsNullOrEmpty(qs["invoke"]) ? qs["invoke"].ToLower() == "true" : false);
            return model;
        }
    }
}