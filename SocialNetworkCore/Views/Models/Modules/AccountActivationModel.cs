using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using System.Web.Security;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class AccountActivationModel
    {
        #region Properties:
        public string Email { get; set; }
        public string ActivationCode { get; set; }
        public bool IsValidRegisterationCode { get; set; }
        public bool IsValidEmailAddress { get; set; }
        public bool ActivationSucceed { get; set; }
        public string ActivationErrorMessage { get; set; }
        #endregion

        #region Methods:
        public static AccountActivationModel Create(System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //*********************************************
            var moduleModel = new AccountActivationModel();
            moduleModel.Email = (HttpContext.Current.Request.QueryString["email"] != null ? HttpContext.Current.Request.QueryString["email"].ToString() : "");
            moduleModel.ActivationCode = (HttpContext.Current.Request.QueryString["code"] != null ? HttpContext.Current.Request.QueryString["code"].ToString() : "");
            moduleModel.IsValidEmailAddress = MyHelper.IsEmailAddress(moduleModel.Email);
            moduleModel.IsValidRegisterationCode = MyHelper.IsValidRegisterationCode(moduleModel.ActivationCode);
            // activation account and set module's associated model properties:
            if (moduleModel.IsValidEmailAddress && moduleModel.IsValidRegisterationCode)
            {
                // user exists?
                MembershipUser user = Membership.GetUser(moduleModel.Email);
                if (user == null)
                {
                    moduleModel.ActivationSucceed = false;
                    moduleModel.ActivationErrorMessage = Resources.Messages.UserNotExists;
                }
                else
                {
                    // is registration code correct?
                    var member = mr.Get(moduleModel.Email);
                    if (mr.GetRegisterationCode(moduleModel.Email).ToUpper() != moduleModel.ActivationCode)
                    {
                        moduleModel.ActivationSucceed = false;
                        moduleModel.ActivationErrorMessage = Resources.Messages.ActivationCodeIsIncorrect;
                    }
                    else
                    {
                        // is user approved currently?
                        if (user.IsApproved)
                        {
                            moduleModel.ActivationSucceed = false;
                            moduleModel.ActivationErrorMessage = Resources.Messages.ActivationUserIsCurrentlyActive;
                        }
                        else
                        {
                            //************* ACTIVATE USER:
                            user.IsApproved = true;
                            member.ChangeStatusCode((byte)MemberStatus.Active);
                            Membership.UpdateUser(user);
                            mr.Save();
                            moduleModel.ActivationSucceed = true;
                        }
                    }
                }
            }
            return moduleModel;
        }
        #endregion
    }
}