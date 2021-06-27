using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class HeaderModel
    {
        public HeaderType Type
        {
            get
            {
                return HasLogin ? HeaderType.Slim : HeaderType.Thick;
            }
        }
        protected bool HasLogin
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
        public bool ShowLogin
        {
            get
            {
                return !HttpContext.Current.Request.FilePath.ToLower().EndsWith("login");
            }
        }
        public int NewRequestsCount { get; set; }
        public int NewMessagesCount { get; set; }
        public int NewNotificationsCount { get; set; }
        //***** modules:
        public LoginModel LoginModel { get; set; }
        //*****

        public static HeaderModel Create(System.Data.Objects.ObjectContext context)
        {
            using (var mr = new MembersRepository())
            using (var msgr = new MessagesRepository(mr.Context))
            using (var nr = new NotificationRepository(mr.Context))
            {
                context = mr.Context;
                var model = new HeaderModel();
                if (model.Type == HeaderType.Thick)
                {
                    model.LoginModel = LoginModel.Create(LoginType.OnHeader, "", null, false, "", true, context);
                }
                else if (model.Type == HeaderType.Slim)
                {
                    model.NewRequestsCount = mr.FindNewFriendshipRequestsCount(HttpContext.Current.User.Identity.Name);
                    model.NewMessagesCount = msgr.GetUnreadMessagesCount(HttpContext.Current.User.Identity.Name);
                    model.NewNotificationsCount = nr.GetUnreadNotificationsCount(HttpContext.Current.User.Identity.Name);
                }
                return model;
            }
        }
    }
}