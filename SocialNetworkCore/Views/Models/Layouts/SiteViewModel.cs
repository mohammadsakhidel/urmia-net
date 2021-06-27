using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using SocialNetApp.Views.Models.Modules;
using CoreHelpers;
using System.Web.Security;

namespace SocialNetApp.Views.Models.Layouts
{
    public class SiteViewModel
    {
        public static string Theme { get; set; }
        public static string Language { get; set; }
        public static string StyleSheetsUrl { get; set; }
        //***** modules:
        public HeaderModel HeaderModel { get; set; }
        public FooterModel FooterModel { get; set; }
        public ChatSessionsModel ChatSessionsModel { get; set; }
        //*****

        public static SiteViewModel Create()
        {
            var membershipUser = Membership.GetUser(); // this if for updating last activity time of user.
            ////////
            var model = new SiteViewModel();
            //***** modules:
            model.HeaderModel = HeaderModel.Create(null);
            model.FooterModel = FooterModel.Create(null);
            model.ChatSessionsModel = ChatSessionsModel.Create(ChatHelper.GetChatSessions(HttpContext.Current.User.Identity.Name), "", "", null);
            //*****
            return model;
        }
    }
}