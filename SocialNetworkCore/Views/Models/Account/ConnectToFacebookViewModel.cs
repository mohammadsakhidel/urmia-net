using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;
using CoreHelpers;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Account
{
    public class ConnectToFacebookViewModel
    {
        public FacebookConnectModel FacebookConnectModel { get; set; }

        public static ConnectToFacebookViewModel Create(System.Collections.Specialized.NameValueCollection qs, HttpSessionStateBase session, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            var model = new ConnectToFacebookViewModel();
            //--- facebook connect module model:
            var isAuth = HttpContext.Current.User.Identity.IsAuthenticated;
            var step = 
                isAuth ? FacebookConnectStep.ConnectAuthenticated : 
                (!String.IsNullOrEmpty(qs["step"]) && 
                MyHelper.IsByte(qs["step"]) ? 
                (FacebookConnectStep)Convert.ToByte(qs["step"]) : 
                FacebookConnectStep.Begin);
            var hasAccount = (bool?)session["has_account"];
            var authMember = isAuth ? mr.Get(HttpContext.Current.User.Identity.Name) : null;
            var fbcModule = FacebookConnectModel.Create(step, hasAccount, isAuth, authMember, context);
            //---
            model.FacebookConnectModel = fbcModule;
            return model;
        }
    }
}