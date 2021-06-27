using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Views.Models.Modules;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Account
{
    public class LoginViewModel
    {
        #region Properties:
        public LoginModel LoginModel { get; set; }
        #endregion

        #region Methods:
        public static LoginViewModel Create(LoginType loginType, System.Data.Objects.ObjectContext context)
        {
            var model = new LoginViewModel();
            var userName = HttpContext.Current.Request.QueryString["u"] != null ? HttpContext.Current.Request.QueryString["u"].ToString() : "";
            var hasStatusCode = HttpContext.Current.Request.QueryString["sc"] != null &&
                MyHelper.IsByte(HttpContext.Current.Request.QueryString["sc"]) &&
                Convert.ToByte(HttpContext.Current.Request.QueryString["sc"]) >= (byte)Enum.GetValues(typeof(LoginStatus)).Cast<LoginStatus>().Min() &&
                Convert.ToByte(HttpContext.Current.Request.QueryString["sc"]) <= (byte)Enum.GetValues(typeof(LoginStatus)).Cast<LoginStatus>().Max();
            var loginStatus = hasStatusCode ? (LoginStatus?)Enum.Parse(typeof(LoginStatus), HttpContext.Current.Request.QueryString["sc"].ToString()) : (LoginStatus?)null;
            var showAccessDeniedMessage = HttpContext.Current.Request.QueryString["ReturnUrl"] != null;
            var redirectPath = showAccessDeniedMessage ? HttpContext.Current.Request.QueryString["ReturnUrl"].ToString() : "";
            model.LoginModel = LoginModel.Create(loginType, userName, loginStatus, showAccessDeniedMessage, redirectPath, true, context);
            return model;
        }
        #endregion
    }
}