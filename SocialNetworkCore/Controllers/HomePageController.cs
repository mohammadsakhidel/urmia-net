using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using SocialNetApp.Models.Repository;
using System.IO;
using SocialNetApp.Models;
using System.Web.Security;
using SocialNetApp.Views.Models.Modules;
using SocialNetApp.Views.Models.HomePage;

namespace SocialNetApp.Controllers
{
    [Authorize(Roles = MyRoles.Member)]
    public class HomePageController : MainController
    {
        #region Get Actions:
        public ActionResult Index()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var hpModel = HomePageModel.Create(member, mr.Context);
                //////////////////////////////////////////
                var model = IndexViewModel.Create(member, "HomePage", hpModel, mr.Context);
                return View("~/Views/HomePage/Index.cshtml", model);
            }
        }

        [AllowAnonymous]
        public ActionResult Public(string tag)
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var psModel = PublicSharesModel.Create(member, tag, mr.Context);
                //////////////////////////////////////////
                var model = IndexViewModel.Create(member, "PublicShares", psModel, mr.Context);
                return View("~/Views/HomePage/Index.cshtml", model);
            }
        }

        public ActionResult Notifications()
        {
            using (var mr = new MembersRepository())
            {
                // ************** get current member:
                var member = mr.Get(User.Identity.Name);
                var notsModel = NotificationsModel.Create(member, mr.Context);
                // ************** gather homepage modules and return view:
                var model = IndexViewModel.Create(member, "Notifications", notsModel, mr.Context);
                return View("~/Views/HomePage/Index.cshtml", model);
            }
        }

        public ActionResult BasicInfo()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var biModel = BasicInfoModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_BasicInfo.cshtml", biModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "BasicInfo", biModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult Favorites()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var favModel = FavoritesModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_Favorites.cshtml", favModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "Favorites", favModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult EducationsAndSkills()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var eduModel = EducationsAndSkillsModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_EducationsAndSkills.cshtml", eduModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "EducationsAndSkills", eduModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult Photos()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var phModel = PhotosModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_Photos.cshtml", phModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "Photos", phModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult ProfileCover()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var pcModel = ProfileCoverModel.Create(member, TempData, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_ProfileCover.cshtml", pcModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "ProfileCover", pcModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult Friends()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var frModel = FriendsModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_Friends.cshtml", frModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "Friends", frModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult Messages()
        {
            using (var mr = new MembersRepository())
            using (var msgr = new MessagesRepository(mr.Context))
            {
                var member = mr.Get(User.Identity.Name);
                var msgsModel = MessagesModel.Create(member, RouteData.Values, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_Messages.cshtml", msgsModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "Messages", msgsModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
            
        }

        public ActionResult Privacy()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var prvModel = PrivacyModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_Privacy.cshtml", prvModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "Privacy", prvModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult NewsFeedSettings()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var nfsModel = NewsFeedSettingsModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_NewsFeedSettings.cshtml", nfsModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "NewsFeedSettings", nfsModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult MailNotifications()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var miModel = MailInformsModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_MailInforms.cshtml", miModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "MailInforms", miModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult AccountSettings()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var accModel = AccountSettingsModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_AccountSettings.cshtml", accModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "AccountSettings", accModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        public ActionResult DeleteAccount()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var delModel = DeleteAccountModel.Create(member, mr.Context);
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_DeleteAccount.cshtml", delModel);
                }
                else
                {
                    var model = IndexViewModel.Create(member, "DeleteAccount", delModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        [AcceptAjax]
        public ActionResult HmiSettings()
        {
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var model = HmiSettingsModel.Create(member, mr.Context);
                Response.CacheControl = "no-cache";
                return PartialView(Urls.ModuleViews + "_HmiSettings.cshtml", model);
            }
        }

        [AcceptAjax]
        public ActionResult HmiRequests()
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var model = HmiRequestsModel.Create(mr.Context);
                    Response.CacheControl = "no-cache";
                    var pview = RenderViewToString(Urls.ModuleViews + "_HmiRequests.cshtml", model);
                    return Json(new
                    {
                        Done = true,
                        PartialView = pview
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptAjax]
        public ActionResult HmiMessages()
        {
            try
            {
                using (var mr = new MessagesRepository())
                {
                    var model = HmiMessagesModel.Create(mr.Context);
                    var pview = RenderViewToString(Urls.ModuleViews + "_HmiMessages.cshtml", model);
                    Response.CacheControl = "no-cache";
                    return Json(new
                    {
                        Done = true,
                        PartialView = pview
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult HmiNotifications()
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    using (var nr = new NotificationRepository())
                    {
                        var notModel = HmiNotificationsModel.Create(nr.Context);
                        var pview = RenderViewToString(Urls.ModuleViews + "_HmiNotifications.cshtml", notModel);
                        Response.CacheControl = "no-cache";
                        var newCounter = nr.GetUnreadNotificationsCount(User.Identity.Name);
                        return Json(new
                        {
                            Done = true,
                            PartialView = pview,
                            NewCounter = newCounter
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch(Exception ex)
                {
                    return Json(new
                    {
                        Done = false,
                        Errors = new string[] { ex.Message }
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Notifications");
            }
        }

        [AcceptAjax]
        public ActionResult ShowRemoveFriendDialog(string id)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var member = mr.GetByUserName(id);
                    var model = RemoveFriendDialogModel.Create(member, mr.Context);
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_RemoveFriendDialog.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ex);
            }
        }
        #endregion
        #region Post Actions:
        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public ActionResult BasicInfo(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var context = mr.Context;
                    var memberToEdit = mr.Get(User.Identity.Name);
                    var editedMember = Member.GetModelFromCollection(form, false, context);
                    editedMember.Alias = memberToEdit.Alias;
                    var basicInfo = BasicInformation.GetModelFromCollection(form);
                    // are models valid:
                    if (!editedMember.IsValid || !basicInfo.IsValid)
                        return Json(new { Done = false, Errors = editedMember.ValidationErrors.Select(ve => ve.Value).ToArray().Concat(basicInfo.ValidationErrors.Select(ve => ve.Value).ToArray()) });
                    // update member:
                    memberToEdit.Name = editedMember.Name;
                    memberToEdit.LastName = editedMember.LastName;
                    memberToEdit.BirthDay = editedMember.BirthDay;
                    memberToEdit.Gender = editedMember.Gender;
                    // set basic information:
                    mr.Add(basicInfo);
                    // save:
                    mr.Save();
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
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
        public ActionResult Favorites(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var favorites = Favorite.GetModelFromCollection(form);
                    if (!favorites.IsValid)
                        return Json(new { Done = false, Errors = favorites.ValidationErrors.Select(va => va.Value).ToArray() });
                    mr.Insert(favorites);
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
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
        public ActionResult Privacy(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var ps = PrivacySetting.GetModelFromCollection(form);
                    // is valid?
                    if (!ps.IsValid)
                        return Json(new { Done = false, Errors = ps.ValidationErrors.Select(ve => ve.Value).ToArray() });
                    // save:
                    mr.Insert(ps);
                    // return:
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }
        
        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public ActionResult NewsFeedSettings(FormCollection form)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    var memberId = form["MemberId"];
                    var setting = ar.GetActivitySetting(memberId);
                    if (setting != null)
                    {
                        setting.NewsFeedInforming =
                            (!String.IsNullOrEmpty(form["ViewPosts"]) && form["ViewPosts"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewPhotos"]) && form["ViewPhotos"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewShares"]) && form["ViewShares"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewLikes"]) && form["ViewLikes"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewComments"]) && form["ViewComments"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewChangePP"]) && form["ViewChangePP"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewChangeCover"]) && form["ViewChangeCover"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewChangeInfo"]) && form["ViewChangeInfo"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewEducations"]) && form["ViewEducations"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewSkills"]) && form["ViewSkills"].ToLower() == "on" ? "1" : "0");
                        setting.UnwantedActors = form["UnwantedActors"];
                        ar.Save();
                    }
                    else
                    {
                        setting = new ActivitySetting();
                        setting.MemberId = memberId;
                        setting.NewsFeedInforming =
                            (!String.IsNullOrEmpty(form["ViewPosts"]) && form["ViewPosts"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewPhotos"]) && form["ViewPhotos"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewShares"]) && form["ViewShares"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewLikes"]) && form["ViewLikes"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewComments"]) && form["ViewComments"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewChangePP"]) && form["ViewChangePP"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewChangeCover"]) && form["ViewChangeCover"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewChangeInfo"]) && form["ViewChangeInfo"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewEducations"]) && form["ViewEducations"].ToLower() == "on" ? "1" : "0") +
                            (!String.IsNullOrEmpty(form["ViewSkills"]) && form["ViewSkills"].ToLower() == "on" ? "1" : "0");
                        setting.UnwantedActors = form["UnwantedActors"];
                        setting.UnwantedActivities = "";
                        ar.Insert(setting);
                    }
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public ActionResult AccountSettings(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    // ********** create objects:
                    var member = mr.Get(User.Identity.Name);
                    var ac_sett = (member.AccountSetting != null ? member.AccountSetting : AccountSetting.Default);
                    // member fields:
                    var is_alias_changed = member.Alias != form["Alias"];
                    member.Alias = form["Alias"];
                    // account setting fields:
                    ac_sett.MemberId = User.Identity.Name;
                    ac_sett.MobNumber = form["MobNumber"];
                    ac_sett.Language = form["Language"];
                    var is_alt_email_changed = ac_sett.AlternativeEmail != form["AlternativeEmail"];
                    if (is_alt_email_changed)
                    {
                        ac_sett.AlternativeEmail = form["AlternativeEmail"];
                        ac_sett.AlternativeEmailApproved = false;
                        var hasAltEmail = !String.IsNullOrEmpty(ac_sett.AlternativeEmail);
                        ac_sett.AlternativeEmailActivationCode = hasAltEmail ? MyHelper.GetRandomString(12, false) : "";
                    }
                    // ********** validation:
                    var validation_errors = new List<string>();
                    // member validation:
                    if (!member.IsValid)
                        validation_errors = validation_errors.Concat(member.ValidationErrors.Select(ve => ve.Value)).ToList();
                    // account setting validation:
                    if (!ac_sett.IsValid)
                        validation_errors = validation_errors.Concat(ac_sett.ValidationErrors.Select(ve => ve.Value)).ToList();
                    // alias exists?
                    if (is_alias_changed && mr.AliasExists(member.Alias))
                        validation_errors.Add(Resources.Messages.AliasExists);
                    // password change validations:
                    var oldpass = form["OldPassword"];
                    var newpass = form["NewPassword"];
                    var newpass_confirm = form["NewPasswordConfirm"];
                    var user = Membership.GetUser(User.Identity.Name);
                    var change_pass = oldpass.Trim().Length > 0 && newpass.Trim().Length > 0 && newpass_confirm.Trim().Length > 0;
                    if (change_pass)
                    {
                        // is old pass correct?
                        if (user.GetPassword() != oldpass)
                            validation_errors.Add(Resources.Messages.OldPasswordIncorrect);
                        // are new and confirm equal?
                        if (newpass != newpass_confirm)
                            validation_errors.Add(Resources.Messages.WrongPasswordConfirm);
                        // is new pass valid?
                        var rgx_pass = new System.Text.RegularExpressions.Regex(Patterns.Password);
                        if (!rgx_pass.IsMatch(newpass))
                            validation_errors.Add(Resources.Messages.IncorectPassword);
                    }
                    // return errors:
                    if (validation_errors.Count > 0)
                        return Json(new { Done = false, Errors = validation_errors.ToArray() });
                    // ********** process and save:
                    mr.Add(ac_sett);
                    mr.Save();
                    if (change_pass)
                        user.ChangePassword(oldpass, newpass);
                    // ********* send approve email for alternative email:
                    var sendEmailNeeded = is_alt_email_changed && !String.IsNullOrEmpty(ac_sett.AlternativeEmail) && !String.IsNullOrEmpty(ac_sett.AlternativeEmailActivationCode);
                    if (sendEmailNeeded)
                    {
                        try {
                            var emailHtml = EmailHelper.GetAlternativeEmaiActivationCodeEmailHtml(ac_sett);
                            EmailHelper.SendEmail(ac_sett.AlternativeEmail, Resources.Emails.AlternatveEmailActivationSubject, emailHtml);
                        }
                        catch { }
                    }
                    //**********
                    var msg = Resources.Messages.SuccessfullySaved + (sendEmailNeeded ? "<br />" + Resources.Messages.AltEmailShouldBeActivated : "");
                    return Json(new { Done = true, Message = msg });
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
        public ActionResult MailInforms(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var model = NotificationSetting.GetModelFromCollection(form);
                    model.MemberId = User.Identity.Name;
                    mr.Insert(model);
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeleteAccount(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var schedule = AccountDeleteSchedule.GetModelFromCollection(form);
                    // is valid model?
                    if (!schedule.IsValid)
                        return Json(new { Done = false, Errors = schedule.ValidationErrors.Select(ve => ve.Value).ToArray() });
                    // process:
                    var member = mr.Get(schedule.MemberId);
                    mr.Add(schedule);
                    // member status code, active -> deleted
                    member.ChangeStatusCode((byte)MemberStatus.ScheduledForDelete);
                    // save
                    mr.Save();
                    OnlinesHelper.SignOut(User.Identity.Name);
                    // return:
                    return Json(new { Done = true, Message = Resources.Messages.DeleteAccountSuccessMEssage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult RemoveFriend(string id)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var friend_to_del = mr.GetByUserName(id);
                    mr.RemoveFriendshipRelation(User.Identity.Name, friend_to_del.Email);
                    mr.Remove(User.Identity.Name, friend_to_del.Email);
                    mr.Remove(friend_to_del.Email, User.Identity.Name);
                    // return:
                    mr.Save();
                    return Json(new { Done = true, FriendId = friend_to_del.RegistrationCode });
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
        public ActionResult AddSkill(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var added_skill = form["AddedSkill"];
                    if (!mr.HasSkill(User.Identity.Name, added_skill))
                    {
                        Skill skill = new Skill();
                        skill.MemberId = form["MemberId"];
                        skill.SkillTitle = added_skill;
                        skill.QualityLevel = 0;
                        // is valid model?
                        if (!skill.IsValid)
                        {
                            return Json(new { Done = false, Errors = skill.ValidationErrors.Select(ve => ve.Value).ToArray() });
                        }
                        //
                        mr.Insert(skill);
                    }
                    var skills = mr.GetSkills(form["MemberId"]);
                    return Json(new { Done = true, Skills = skills.Select(s => new { Id = s.Id, SkillTitle = s.SkillTitle }).ToArray() });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[]{ ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeleteSkill(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var skillId = Convert.ToInt32(form["SkillID"]);
                    var skill = mr.GetSkill(skillId);
                    mr.Delete(skill);
                    return Json(new { Done = true, SkillId = skillId });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[]{ ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public ActionResult AddEducation(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var edu = new Education();
                    edu.MemberId = form["MemberId"];
                    if (!String.IsNullOrEmpty(form["EducationLevel"]))
                        edu.EducationLevel = Convert.ToByte(form["EducationLevel"]);
                    edu.EducationBranch = form["EducationBranch"];
                    edu.EducationLocation = form["EducationLocation"];
                    if (!String.IsNullOrEmpty(form["FromYear"]) && !String.IsNullOrEmpty(form["ToYear"]))
                    {
                        edu.FromYear = Convert.ToInt16(form["FromYear"]);
                        edu.ToYear = Convert.ToInt16(form["ToYear"]);
                    }
                    // is model valid?
                    if (!edu.IsValid)
                    {
                        return Json(new { Done = false, Errors = edu.ValidationErrors.Select(ve => ve.Value).ToArray() });
                    }
                    // save:
                    mr.Insert(edu);
                    // return:
                    var educations = mr.GetEducations(form["MemberId"]);
                    return Json(new
                    {
                        Done = true,
                        Educations = educations.Select(e => new
                        {
                            Id = e.Id,
                            EducationBranch = e.EducationBranch,
                            EducationLocation = e.EducationLocation,
                            EducationLevelText = e.EducationLevelText
                        }).ToArray()
                    });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeleteEducation(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var eduId = Convert.ToInt32(form["EducationId"]);
                    var edu = mr.GetEducation(eduId);
                    mr.Delete(edu);
                    return Json(new { Done = true, EducationId = eduId });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult GetMoreActivities(FormCollection form)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    // get acts:
                    var last_act_date = Convert.ToDateTime(form["CurrentPageIndex"]);
                    var acts = ar.FindNewsFeedActivities(User.Identity.Name, last_act_date);
                    var last_act_date_new = (acts.Any() ? acts.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
                    var is_there_more = String.IsNullOrEmpty(last_act_date_new) ? false : ar.IsThereMoreNewsFeedActivities(User.Identity.Name, Convert.ToDateTime(last_act_date_new));
                    // create partial view:
                    var param = new Dictionary<string, object>();
                    param.Add("AVCL", ActivityVisibilityCheckLevel.None);
                    var partial_view = "";
                    foreach (var act in acts)
                    {
                        partial_view += RenderViewToString(act.PartialViewUrl, ActivityModel.Create(act, param , ar.Context));
                    }
                    // return:
                    return Json(new
                    {
                        Done = true,
                        RandomId = form["RandomId"],
                        IsThereMore = is_there_more,
                        PartialView = partial_view,
                        NextPageIndex = last_act_date_new
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { 
                    Done = false, 
                    Errors = new string[] { ex.Message }, 
                    RandomId = form["RandomId"] 
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [AllowAnonymous]
        public ActionResult GetMorePublicActivities(FormCollection form)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    var tag = !string.IsNullOrEmpty(form["Parameters"]) ? MyHelper.ExtractQuerystringFromValues(form["Parameters"])["tag"] : "";
                    // get acts:
                    var last_act_date = Convert.ToDateTime(form["CurrentPageIndex"]);
                    var acts = ar.FindPublicActivities(User.Identity.Name, tag, last_act_date);
                    var last_act_date_new = (acts.Any() ? acts.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
                    var is_there_more = String.IsNullOrEmpty(last_act_date_new) ? false : ar.IsThereMorePublicActivities(User.Identity.Name, tag, Convert.ToDateTime(last_act_date_new));
                    // create partial view:
                    var param = new Dictionary<string, object>();
                    param.Add("AVCL", ActivityVisibilityCheckLevel.None);
                    var partial_view = "";
                    foreach (var act in acts)
                    {
                        partial_view += RenderViewToString(act.PartialViewUrl, ActivityModel.Create(act, param, ar.Context));
                    }
                    // return:
                    return Json(new
                    {
                        Done = true,
                        RandomId = form["RandomId"],
                        IsThereMore = is_there_more,
                        PartialView = partial_view,
                        NextPageIndex = last_act_date_new
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message },
                    RandomId = form["RandomId"]
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult GetMoreNotifications(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var nr = new NotificationRepository(mr.Context);
                    var currentPageIndex = Convert.ToInt32(form["CurrentPageIndex"]);
                    var nextPageIndex = currentPageIndex + 1;
                    // get nots:
                    var notifications = nr.GetPagedNotifications(User.Identity.Name, nextPageIndex);
                    var is_there_more = nr.IsThereMoreNotifications(User.Identity.Name, nextPageIndex);
                    // create partial view:
                    var partial_view = "";
                    foreach (var not in notifications)
                    {
                        partial_view +=
                            "<div class=\"notification_item\">" +
                                RenderViewToString(Urls.ModuleViews + "_Notification.cshtml", NotificationModel.Create(not, mr.Context)) +
                            "</div>";
                    }
                    // set as read:
                    foreach (var n in notifications)
                    {
                        n.Status = (byte)NotificationStatus.Read;
                    }
                    nr.Save();
                    // return:
                    return Json(new
                    {
                        Done = true,
                        RandomId = form["RandomId"],
                        IsThereMore = is_there_more,
                        PartialView = partial_view,
                        NextPageIndex = nextPageIndex
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message },
                    RandomId = form["RandomId"]
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult MarkAllNotificationsAsRead()
        {
            try
            {
                using (var nr = new NotificationRepository())
                {
                    nr.MarkAllAsRead(User.Identity.Name);
                    return Json(new { Done = true });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[]{ ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult DeleteAllNotifications()
        {
            try
            {
                using (var nr = new NotificationRepository())
                {
                    foreach (var not in nr.GetNotifications(User.Identity.Name))
                    {
                        nr.Remove(not);
                    }
                    nr.Save();
                    return Json(new
                    {
                        Done = true
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }
        #endregion
        #region PRIVATE METHODS:
        
        #endregion
    }
}
