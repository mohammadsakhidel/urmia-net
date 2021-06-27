using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Views.Models.Members
{
    public class ProfilePageViewModel
    {
        public Member Member { get; set; }
        public string DefaultView { get; set; }
        public string ContentPage { get; set; }
        //***** modules
        public ProfileModel ProfileModel { get; set; }
        public CustomErrorModel CustomErrorModel { get; set; }
        //*****

        public static ProfilePageViewModel Create(Member member, System.Web.Routing.RouteValueDictionary routeData, System.Data.Objects.ObjectContext context)
        {
            var model = new ProfilePageViewModel();
            model.Member = member;
            model.DefaultView = "timeline";
            model.ContentPage = routeData["content"] != null && !String.IsNullOrEmpty(routeData["content"].ToString()) ? routeData["content"].ToString() : model.DefaultView;
            //***** modules:
            model.ProfileModel = ProfileModel.Create(model.Member, model.ContentPage, context);
            model.CustomErrorModel = CustomErrorModel.Create(Resources.Messages.PageNotFoundErrorText, context);
            //*****
            return model;
        }
    }
}