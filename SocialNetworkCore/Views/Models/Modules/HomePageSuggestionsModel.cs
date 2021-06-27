using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class HomePageSuggestionsModel
    {
        #region Properties:
        public List<Tuple<Member, UserIdentityModel>> RecommendedPeople { get; set; }
        public List<Tuple<Member, UserIdentityModel>> ProfileViewers { get; set; }
        public List<Tuple<Member, UserIdentityModel>> SomeRandomOnlines { get; set; }
        //************** module models:
        public HomePagePixelAdvModel HomePagePixelAdvModel { get; set; }
        //**************
        #endregion

        #region Methods:
        public static HomePageSuggestionsModel Create(System.Data.Objects.ObjectContext context)
        {
            var sr = new SuggestionsRepository(context);
            context = sr.Context;
            ///////////////////////////////////////////////////////
            var model = new HomePageSuggestionsModel();
            model.HomePagePixelAdvModel = HomePagePixelAdvModel.Create(context);
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //recommendations:
                var recommendedPeopleRecords = new List<Tuple<Member, UserIdentityModel>>();
                var recommendedPeople = sr.FindRecommendedPeople(HttpContext.Current.User.Identity.Name);
                foreach (var m in recommendedPeople)
                {
                    var uidModel = UserIdentityModel.Create(m, 44, UserIdentityType.Thumb, "sugg_box_user", "", "", "", context);
                    recommendedPeopleRecords.Add(new Tuple<Member, UserIdentityModel>(m, uidModel));
                }
                //random onlines:
                var someRandomOnlinesRecords = new List<Tuple<Member, UserIdentityModel>>();
                var someRandomOnlines = sr.FindRecommendedOnlines(HttpContext.Current.User.Identity.Name);
                foreach (var m in someRandomOnlines)
                {
                    var uidModel = UserIdentityModel.Create(m, 44, UserIdentityType.Thumb, "sugg_box_user", "", "", "", context);
                    someRandomOnlinesRecords.Add(new Tuple<Member, UserIdentityModel>(m, uidModel));
                }
                //profile views:
                var profileViewersRecords = new List<Tuple<Member, UserIdentityModel>>();
                var profileViewers = sr.FindProfileViewers(HttpContext.Current.User.Identity.Name);
                foreach (var m in profileViewers)
                {
                    var uidModel = UserIdentityModel.Create(m, 44, UserIdentityType.Thumb, "sugg_box_user", "", "", "", context);
                    profileViewersRecords.Add(new Tuple<Member, UserIdentityModel>(m, uidModel));
                }
                ////////////////////////////////////////////
                model.RecommendedPeople = recommendedPeopleRecords;
                model.SomeRandomOnlines = someRandomOnlinesRecords;
                model.ProfileViewers = profileViewersRecords;
            }
            return model;
        }
        #endregion
    }
}