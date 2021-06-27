using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;
using System.Web.Security;
using SocialNetApp.Views.Models.Modules;
using SocialNetApp.Views.Models.Account;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetApp.Controllers
{
    public class AccountController : MainController
    {
        #region Get Actions:

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(Urls.ModuleViews + "_Login.cshtml", LoginModel.Create(LoginType.InContent, "", null, false, "", true, null));
            }
            else
            {
                var model = LoginViewModel.Create(LoginType.InContent, null);
                return View(model);
            }
        }

        public ActionResult LogOut()
        {
            OnlinesHelper.SignOut(User.Identity.Name);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RecoverPassword()
        {
            var model = RecoverPasswordViewModel.Create(null);
            return View(model);
        }

        public ActionResult Activation()
        {
            var model = ActivationViewModel.Create(null);
            return View(model);
        }

        public ActionResult Register()
        {
            var model = RegisterViewModel.Create(null);
            return View(model);
        }

        public ActionResult AlternativeEmailActivation(string email, string code)
        {
            var model = AlternativeEmailActivationViewModel.Create(email, code, null);
            return View(model);
        }

        public ActionResult ConnectToFacebook()
        {
            using (var mr = new MembersRepository())
            {
                var model = ConnectToFacebookViewModel.Create(Request.QueryString, Session, mr.Context);
                return View(model);
            }
        }
        #endregion

        #region Post Actions:

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public JsonResult Register(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var context = mr.Context;
                    //*** some validations other than model validations:
                    if (form["Email"].ToLower() != form["EmailConfirm"].ToLower())
                        throw new Exception(Resources.Messages.ConfirmEmail);
                    if (!MyHelper.IsValidPassword(form["Password"]))
                        throw new Exception(Resources.Messages.IncorectPassword);
                    // register:
                    var name = form["Name"];
                    var lastName = form["LastName"];
                    var email = form["Email"].ToLower();
                    var password = form["Password"];
                    var birthday = DateHelper.GetMiladyDateFromInfo(form["Day"], form["Month"], form["Year"]);
                    var gender = form["Gender"] != null ? (form["Gender"] == "Male" ? true : false) : (bool?)null;
                    var result = Member.Register(name, lastName, email, password, birthday, gender, (byte)MemberStatus.RegisteredNotActivated, context);
                    if (result.Item1)
                    {
                        //**** send activation code:
                        try
                        {
                            var registeredMember = result.Item3;
                            EmailHelper.SendEmail(registeredMember.Email, Resources.Emails.RegisterationCodeSubject, EmailHelper.GetRegisterationCodeEmailHtml(registeredMember));
                        }
                        catch { }
                        return Json(new { Done = true });
                    }
                    else
                    {
                        return Json(new { Done = false, Errors = result.Item2.Select(ve => ve.Value).ToArray() });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public JsonResult Login(FormCollection form)
        {
            try
            {
                var username = form["Email"].ToLower();
                var password = form["Password"];
                var remember = form["RememberMe"] != null && form["RememberMe"].ToLower() == "on";
                var model = new LoginAttempt() { Email = username, Password = password, RememberMe = remember };
                var res = model.Login(null);
                return Json(new { StatusCode = Convert.ToInt32(res), Errors = new string[] { LoginAttempt.GetLoginMessage(res) } });
            }
            catch(Exception ex)
            {
                return Json(new { Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public JsonResult ActivateAccount(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    string email = form["Email"].ToLower();
                    string code = form["RegistrationCode"].Trim().ToUpper();
                    // are email and code valid?
                    if (!MyHelper.IsEmailAddress(email) || !MyHelper.IsValidRegisterationCode(code))
                        throw new Exception(Resources.Messages.ActivationNotValidEmailOrCode);
                    // user exists?
                    MembershipUser user = Membership.GetUser(email);
                    if (user == null)
                        throw new Exception(Resources.Messages.UserNotExists);
                    var member = mr.Get(email);
                    // is registration code correct?
                    if (member.RegistrationCode.ToUpper() != code)
                        throw new Exception(Resources.Messages.ActivationCodeIsIncorrect);
                    // is user approved currently?
                    if (user.IsApproved)
                        throw new Exception(Resources.Messages.ActivationUserIsCurrentlyActive);
                    //************* ACTIVATE USER:
                    // member status, notApproved -> active
                    member.ChangeStatusCode((byte)MemberStatus.Active);
                    //
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    mr.Save();
                    return Json(new { Done = true, Message = Resources.Messages.ActivationSuccess });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public JsonResult SendRegisterationCode(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    // is email address valid?
                    string email = form["Email"].ToLower();
                    if (!MyHelper.IsEmailAddress(email))
                        throw new Exception(Resources.Messages.NotValidEmail);
                    // user exists?
                    if (Membership.GetUser(email) == null)
                        throw new Exception(Resources.Messages.UserNotExists);
                    // send activation code:
                    var member = mr.Get(email);
                    EmailHelper.SendEmail(email, Resources.Emails.RegisterationCodeSubject, EmailHelper.GetRegisterationCodeEmailHtml(member));
                    return Json(new { Done = true, Message = Resources.Messages.SendCodeSuccessful });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public JsonResult SendPassword(FormCollection form)
        {
            try
            {
                // is email address valid?
                string email = form["Email"].ToLower();
                if (!MyHelper.IsEmailAddress(email))
                    throw new Exception(Resources.Messages.NotValidEmail);
                // user exists?
                MembershipUser user = Membership.GetUser(email);
                if (user == null)
                    throw new Exception(Resources.Messages.UserNotExists);
                // send activation code:
                EmailHelper.SendEmail(email, Resources.Emails.PasswordRecoverySubject, EmailHelper.GetPasswordRecoveryEmailHtml(user.GetPassword()));
                return Json(new { Done = true, Message = Resources.Messages.PasswordSent });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new { Done = false, Errors = new string[] { ex.InnerException.Message } });
                }
                else
                {
                    return Json(new { Done = false, Errors = new string[] { ex.Message } });
                }
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult FacebookConnect(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var context = mr.Context;
                    System.Threading.Thread.Sleep(2000);
                    //---- for authenticated member:
                    if (User.Identity.IsAuthenticated)
                    {
                        var member = mr.Get(User.Identity.Name);
                        var action = (member.FacebookConnection != null ? "edit" : "new");
                        var fbc = action == "edit" ? member.FacebookConnection : new FacebookConnection();
                        fbc.MemberId = member.Email;
                        fbc.UserId = form["userId"];
                        fbc.Email = form["email"];
                        fbc.Name = form["firstName"];
                        fbc.LastName = form["lastName"];
                        fbc.Birthday = DateTime.ParseExact(form["birthday"], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        fbc.Gender = form["gender"].ToLower() == "female" ? false : true;
                        if (!fbc.IsValid)
                            return Json(new { Done = false, Errors = fbc.ValidationErrors.Select(ve => ve.Value).ToArray() });
                        if (action == "edit")
                            mr.Save();
                        else
                            mr.Insert(fbc);
                        return Json(new { Done = true, Message = Resources.Messages.FacebookConnectSuccessfullMessageForAuthenticated });
                    }
                    else
                    {
                        /////// facebook connection:
                        var fbc = new FacebookConnection();
                        fbc.MemberId = form["email"];
                        fbc.UserId = form["userId"];
                        fbc.Email = form["email"];
                        fbc.Name = form["firstName"];
                        fbc.LastName = form["lastName"];
                        fbc.Birthday = DateTime.ParseExact(form["birthday"], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        fbc.Gender = form["gender"].ToLower() == "female" ? false : true;
                        if (!fbc.IsValid)
                            return Json(new { Done = false, Errors = fbc.ValidationErrors.Select(ve => ve.Value).ToArray() });
                        ////// register member:
                        var rndPass = Member.GeneratePassword();
                        var result = Member.Register(fbc.Name, fbc.LastName, fbc.MemberId, rndPass, fbc.Birthday, fbc.Gender, (byte)MemberStatus.Active, context);
                        if (result.Item1)
                        {
                            mr.Insert(fbc);
                            /////// send generated password for email address:
                            try
                            {
                                var registeredMember = result.Item3;
                                EmailHelper.SendEmail(registeredMember.Email, 
                                    Resources.Emails.GeneratedPasswordSubject, 
                                    EmailHelper.GetGeneratedPasswordEmailHtml(registeredMember.Email, rndPass));
                            }
                            catch { }
                            //return: 
                            return Json(new { Done = true, Message = Resources.Messages.FacebookConnectSuccessfullMessageForAnonymous });
                        }
                        else
                        {
                            return Json(new { Done = false, Errors = result.Item2.Select(ve => ve.Value) });
                        }
                        
                    }
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult FacebookLogin(string UserId)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var context = mr.Context;
                    var member = mr.GetByFacebookUserId(UserId);
                    //--- member not null?
                    if (member == null)
                        throw new Exception(Resources.Messages.FacebookLoginConnectionNotExists);
                    //---
                    var user = Membership.GetUser(member.Email);
                    var model = new LoginAttempt() { Email = member.Email, Password = user.GetPassword(), RememberMe = true };
                    var res = model.Login(context);
                    return Json(new { 
                        Done = true,
                        StatusCode = Convert.ToInt32(res), 
                        Errors = new string[] { LoginAttempt.GetLoginMessage(res) } 
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }
        #endregion

        #region privates:
        #endregion
    }
}
