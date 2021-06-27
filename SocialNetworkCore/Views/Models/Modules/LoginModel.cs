using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class LoginModel
    {
        #region Properties:
        public LoginType Type { get; set; }
        public string UserName { get; set; }
        public LoginStatus? LoginStatus { get; set; }
        public bool ShowAccessDeniedMessage { get; set; }
        public string RedirectPath { get; set; }
        public bool ShowFacebookLoginButton { get; set; }
        #endregion

        #region Methods:
        public static LoginModel Create(LoginType loginType, 
            string userName, 
            LoginStatus? loginStatus, 
            bool showAccessDeniedMessage, 
            string redirectPath,
            bool showFacebookBtn,
            System.Data.Objects.ObjectContext context)
        {
            var model = new LoginModel();
            model.Type = loginType;
            model.UserName = userName;
            model.LoginStatus = loginStatus;
            model.ShowAccessDeniedMessage = showAccessDeniedMessage;
            model.RedirectPath = (!String.IsNullOrEmpty(redirectPath) ? redirectPath : "/homepage");
            model.ShowFacebookLoginButton = showFacebookBtn;
            return model;
        }
        #endregion
    }
}