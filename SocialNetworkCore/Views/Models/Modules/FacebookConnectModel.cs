using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class FacebookConnectModel
    {
        #region Properties:
        public FacebookConnectStep Step { get; set; }
        public bool? HasAccount { get; set; }
        public bool IsAuthenticated { get; set; }
        public Member AuthenticatedMember { get; set; }
        //--- Modules:
        public LoginModel LoginModel { get; set; }
        public UserIdentityModel AuthenticatedMemberIdentityModel { get; set; }
        //---
        #endregion

        public static FacebookConnectModel Create(FacebookConnectStep step, bool? hasAccount, bool isAuth, Member authenticatedMember, System.Data.Objects.ObjectContext context)
        {
            var model = new FacebookConnectModel();
            model.Step = step;
            model.HasAccount = hasAccount;
            model.IsAuthenticated = isAuth;
            model.AuthenticatedMember = authenticatedMember;
            //--- modules:
            model.LoginModel = LoginModel.Create(LoginType.InContent, "", null, false, MyHelper.ToAbsolutePath("~/Account/ConnectToFacebook?step=" + (byte)FacebookConnectStep.ConnectAuthenticated), false, context);
            model.AuthenticatedMemberIdentityModel = UserIdentityModel.Create(model.AuthenticatedMember, 65, UserIdentityType.ThumbAndFullName, "", "authMemberFullName", "", "", context);
            //---
            return model;
        }
    }
}