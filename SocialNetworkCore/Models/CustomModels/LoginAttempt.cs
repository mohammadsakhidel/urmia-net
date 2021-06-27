using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CoreHelpers;
using System.Web.Mvc;
using System.Web.Security;

namespace SocialNetApp.Models
{
    public class LoginAttempt
    {
        #region Properties:
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        #endregion

        #region Validation:
        private ValidationErrorList _validationErrors = new ValidationErrorList();
        private bool _isValidated = false;
        public ValidationErrorList ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
            set
            {
                _validationErrors = value;
            }
        }
        public bool IsValid
        {
            get
            {
                if (!_isValidated)
                    Validate();
                return ValidationErrors.Count() == 0;
            }
        }
        public void Validate()
        {
            //Email:
            Regex rgx_email = new Regex(Patterns.Email);
            if (String.IsNullOrEmpty(Email))
                ValidationErrors.Add("Email", Resources.Messages.RequiredEmail);
            else
            {
                if (Email.Length > 50)
                    ValidationErrors.Add("Email", Resources.Messages.StringLengthEmail);
                if (!rgx_email.IsMatch(Email))
                    ValidationErrors.Add("Email", Resources.Messages.RegularExpressionEmail);
            }

            //Password:
            if (!MyHelper.IsValidPassword(Password))
                ValidationErrors.Add("Password", Resources.Messages.IncorectPassword);
        }
        #endregion

        #region Object Methods:
        public LoginStatus Login(System.Data.Objects.ObjectContext context)
        {
            var mr = new SocialNetApp.Models.Repository.MembersRepository(context);
            Member member = mr.Get(this.Email);
            // this process is based on related UML activity diagram.
            // is user name and password valid expressions?
            if (!this.IsValid)
                return LoginStatus.NotValidAttempt;
            // are user name and password valid by provider?
            if (!MembershipService.ValidateUser(this.Email, this.Password))
            {
                // user exists?
                if (member == null)
                    return LoginStatus.UserNotExists;
                // get user:
                var user = Membership.GetUser(this.Email);
                // is member currently locked out?
                if (user.IsLockedOut)
                {
                    
                    if (user.LastLockoutDate.AddHours(Digits.LockedOutHours) < MyHelper.Now)
                    {
                        user.UnlockUser();
                        Membership.UpdateUser(user);
                        return this.Login(context);
                    }
                    else
                    {
                        return LoginStatus.UserLockedOut;
                    }
                }
                // is approved?
                if (member.StatusCode == (byte)MemberStatus.RegisteredNotActivated)
                    return LoginStatus.UserNotApproved;
                // otherwise password is incorrect
                return LoginStatus.WrongPassword;
            }
            // is blocked?
            if (member.IsBlocked)
                return LoginStatus.BlockedAccount;
            // is deleted account?
            if (member.StatusCode == (byte)MemberStatus.ScheduledForDelete)
            {
                AccountDeleteSchedule delete_schedule = mr.GetDeleteSchedule(member.Email);
                if (delete_schedule != null && delete_schedule.DateOfDelete.AddDays(Digits.MaxDaysOfAccountDeleteCanceling) < MyHelper.Now)
                {
                    return LoginStatus.DeletedAccount;
                }
                else
                {
                    // deleted -> active
                    member.ChangeStatusCode((byte)MemberStatus.Active);
                    //
                    delete_schedule.Status = (byte)DeleteScheduleStatus.Canceled;
                    mr.Save();
                }
            }
            // save cookie and return:
            MembershipService.SetCookie(this.Email, this.RememberMe);
            return LoginStatus.Successful;
        }

        public static string GetLoginMessage(LoginStatus status)
        {
            string msg = "";
            switch (status)
            {
                case LoginStatus.BlockedAccount:
                    msg = Resources.Messages.LoginBlockedAccount;
                    break;
                case LoginStatus.DeletedAccount:
                    msg = Resources.Messages.LoginDeletedAccount;
                    break;
                case LoginStatus.NotValidAttempt:
                    msg = Resources.Messages.LoginNotValidAttempt;
                    break;
                case LoginStatus.UserLockedOut:
                    msg = Resources.Messages.LoginUserLockedOut;
                    break;
                case LoginStatus.UserNotApproved:
                    msg = Resources.Messages.LoginUserNotApproved;
                    break;
                case LoginStatus.UserNotExists:
                    msg = Resources.Messages.LoginUserNotExists;
                    break;
                case LoginStatus.WrongPassword:
                    msg = Resources.Messages.LoginWrongPassword;
                    break;
            }
            return msg;
        }
        #endregion

        #region Static Members:
        public static LoginAttempt GetModelFromCollection(FormCollection form)
        {
            LoginAttempt model = new LoginAttempt();
            model.Email = form["Email"];
            model.Password = form["Password"];
            model.RememberMe = (!String.IsNullOrEmpty(form["RememberMe"]) && form["RememberMe"] == "on" ? true : false);
            return model;
        }
        #endregion
    }

    
}