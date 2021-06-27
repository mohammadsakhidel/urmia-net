using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using System.IO;
using SocialNetApp.Views.Models.Modules;
using SocialNetApp.Views.Models.HomePage;

namespace SocialNetApp.Controllers
{

    [Authorize(Roles = "Administrator, Manager")]
    public class AdminController : MainController
    {
        #region GET Actions:
        public ActionResult SpecialPosts()
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_spec_posts))
                throw new HttpException(403, "Access Denied.");
            //
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var spsModel = SpecialPostsModel.Create(mr.Context);
                var model = SocialNetApp.Views.Models.HomePage.IndexViewModel.Create(member, "SpecialPosts", spsModel, mr.Context);
                return View("~/Views/HomePage/Index.cshtml", model);
            }
        }

        public ActionResult ObjectChecker()
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_obj_check))
                throw new HttpException(403, "Access Denied.");
            //
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var ocModel = ObjectCheckerModel.Create(mr.Context);
                /////////////////////////////////////////
                var model = IndexViewModel.Create(member, "ObjectChecker", ocModel, mr.Context);
                return View("~/Views/HomePage/Index.cshtml", model);
            }
        }

        public ActionResult Pages()
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_pages))
                throw new HttpException(403, "Access Denied.");
            //
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var pagesModel = PagesModel.Create(mr.Context);
                /////////////////////////////////////////
                var model = IndexViewModel.Create(member, "Pages", pagesModel, mr.Context);
                return View("~/Views/HomePage/Index.cshtml", model);
            }
        }

        public ActionResult PixelAdv()
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_advs))
                throw new HttpException(403, "Access Denied.");
            //
            using (var mr = new MembersRepository())
            {
                var member = mr.Get(User.Identity.Name);
                var advModel = PixelAdvManagerModel.Create(member, TempData, mr.Context);
                /////////////////////////////////////////
                var model = IndexViewModel.Create(member, "PixelAdv", advModel, mr.Context);
                return View("~/Views/HomePage/Index.cshtml", model);
            }
        }

        [AcceptAjax]
        public ActionResult GetEditMemberConsiderationsDialog(string id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_user_mngr))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var mr = new MembersRepository())
                {
                    var member = mr.GetByUserName(id);
                    var model = MemberConsiderationsDialogModel.Create(member, mr.Context);
                    return PartialView(Urls.ModuleViews + "_MemberConsiderationsDialog.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ex);
            }
        }

        [AcceptAjax]
        public ActionResult ShowManagerSettingDialog(string id)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    System.Threading.Thread.Sleep(2000);
                    var member = mr.GetByUserName(id);
                    var model = ManagerSettingDialogModel.Create(member, mr.Context);
                    return PartialView(Urls.ModuleViews + "_ManagerSettingDialog.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ex);
            }
        }
        #endregion

        #region POST Actions:
        [HttpPost]
        [AcceptAjax]
        [ValidateInput(false)]
        public ActionResult SendSpecialPost(FormCollection form)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_spec_posts))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var pr = new PostsRepository())
                {
                    var action = form["FormAction"];
                    if (action == "edit")
                    {
                        var spec_post = (SpecialPost)pr.Get(Convert.ToInt32(form["PostId"]));
                        spec_post.Text = form["PostText"];
                        spec_post.ShowMethod = Convert.ToByte(form["ShowMethod"]);
                        spec_post.Priority = Convert.ToInt16(MyHelper.IsInt16(form["Priority"]) ? form["Priority"] : "0");
                        pr.Save();
                        return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                    }
                    else
                    {
                        var spec_post = new SpecialPost();
                        // shared obj fields:
                        spec_post.MemberId = User.Identity.Name;
                        spec_post.VisibleTo = (byte)VisibleTo.EveryOne;
                        spec_post.AllowCommentTo = (byte)AllowCommentTo.Members;
                        spec_post.DateOfAdd = MyHelper.Now;
                        spec_post.VisitsCounter = 0;
                        // post fields:
                        spec_post.Text = form["PostText"];
                        spec_post.Status = (byte)PostStatus.Visible;
                        spec_post.Type = (byte)PostType.SpecialPost;
                        // special post fields:
                        spec_post.ShowMethod = Convert.ToByte(form["ShowMethod"]);
                        spec_post.Priority = Convert.ToInt16(MyHelper.IsInt16(form["Priority"]) ? form["Priority"] : "0");
                        // save:
                        pr.Insert(spec_post);
                        return Json(new { Done = true, Message = Resources.Messages.SuccessfullySent });
                    }
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeleteSpecialPost(int id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_spec_posts))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var pr = new PostsRepository())
                {
                    pr.Delete(id);
                    return Json(new { Done = true, PostId = id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult EditSpecialPost(int id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_spec_posts))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var pr = new PostsRepository())
                {
                    var post = (SpecialPost)pr.Get(id);
                    return Json(new { Done = true, PostId = post.Id, PostText = post.Text, Priority = post.Priority, ShowMethod = post.ShowMethod });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult ToggleVisibilitySpecialPost(int id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_spec_posts))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var pr = new PostsRepository())
                {
                    var tgl_txt = "";
                    var post = pr.Get(id);
                    if (post.Status == (byte)PostStatus.Visible)
                    {
                        post.Status = (byte)PostStatus.Unvisible;
                        tgl_txt = Resources.Words.Show;
                    }
                    else if (post.Status == (byte)PostStatus.Unvisible)
                    {
                        post.Status = (byte)PostStatus.Visible;
                        tgl_txt = Resources.Words.Hide;
                    }
                    pr.Save();
                    return Json(new { Done = true, PostId = id, NextToggleText = tgl_txt });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeletePage(string name)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_pages))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var cr = new PagesRepository())
                {
                    cr.Delete(name);
                    return Json(new { Done = true, PageId = name });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult EditPage(string name)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_pages))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var cr = new PagesRepository())
                {
                    var pg = cr.Get(name);
                    return Json(new
                    {
                        Done = true,
                        PageId = name,
                        Contents = pg.PageContents.Select(pgc => new string [] { pgc.Language, pgc.Title, pgc.Content }).ToArray(),
                        Name = name
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [ValidateInput(false)]
        public ActionResult SendPage(FormCollection form)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_pages))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var cr = new PagesRepository())
                {
                    if (!String.IsNullOrEmpty(form["PageId"]))
                    { // edit
                        var pgName = form["PageId"];
                        var page = cr.Get(pgName);
                        // is valid?
                        if (!page.IsValid)
                            return Json(new { Done = false, Errors = page.ValidationErrors.Select(ve => ve.Value).ToArray() });
                        foreach (var pgContent in page.PageContents)
                        {
                            pgContent.Title = form[string.Format("Title_{0}", pgContent.Language)];
                            pgContent.Content = form[string.Format("Content_{0}", pgContent.Language)];
                        }
                        // save:
                        cr.Save();
                    }
                    else
                    { // new
                        var page = new SocialNetApp.Models.Page();
                        page.Name = form["PageName"];
                        page.DateOfAdd = MyHelper.Now;
                        // is valid?
                        if (!page.IsValid)
                            return Json(new { Done = false, Errors = page.ValidationErrors.Select(ve => ve.Value).ToArray() });
                        foreach (var lang in Configs.SupportedLanguages)
                        {
                            var pgContent = new PageContent();
                            //pgContent.PageName = page.Name;
                            pgContent.Language = lang.Code;
                            pgContent.Title = form[string.Format("Title_{0}", lang.Code)];
                            pgContent.Content = form[string.Format("Content_{0}", lang.Code)];
                            // save:
                            page.PageContents.Add(pgContent);
                        }
                        cr.Insert(page);
                    }
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult GetMoreActivitiesForAdmin(FormCollection form)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    // get acts:
                    var last_act_date = Convert.ToDateTime(form["CurrentPageIndex"]);
                    var date_to_check = Convert.ToDateTime(form["Parameters"]);
                    var acts = ar.FindActivitiesForAdmin(date_to_check.Date, last_act_date);
                    var last_act_date_new = (acts.Any() ? acts.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
                    var is_there_more = String.IsNullOrEmpty(last_act_date_new) ? false : ar.IsThereMoreActivitiesForAdmin(date_to_check, Convert.ToDateTime(last_act_date_new));
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
        public ActionResult SaveMemberConsiderations(FormCollection form)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_user_mngr))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var mr = new MembersRepository())
                {
                    var member = mr.Get(form["MemberId"]);
                    member.Considerations = form["Considerations"];
                    mr.Save();
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[]{ ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult ToggleMemberBlock(string id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_user_mngr))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                System.Threading.Thread.Sleep(3000);
                using (var mr = new MembersRepository())
                {
                    var member = mr.GetByUserName(id);
                    member.IsBlocked = !member.IsBlocked;
                    mr.Save();
                    var tgl_txt = (member.IsBlocked ? Resources.Words.UnBlockMember : Resources.Words.BlockMember);
                    return Json(new { Done = true, ToggleText = tgl_txt });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SubmitPixelAdv(FormCollection form, HttpPostedFileBase ImageFile)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_advs))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var ar = new AdvRepository())
                {
                    var form_action = form["FormAction"];
                    if (form_action == "new")
                    {
                        // is valid photo format?
                        bool hasPhoto = !(ImageFile == null);
                        bool isValidPhoto = hasPhoto && MyHelper.IsSupportedPhotoExtension(MyHelper.GetExtension(ImageFile.FileName));
                        if (!isValidPhoto)
                            throw new Exception(Resources.Messages.NotValidPhotoFormat);
                        // is valid file size?
                        if (ImageFile.ContentLength > Digits.MaximumPhotoBytes)
                            throw new Exception(Resources.Messages.LargerThanMaximumUpload);
                        // generate filename:
                        var ext = MyHelper.GetExtension(ImageFile.FileName);
                        var filename = GenerateAdvFileName(ext);
                        // get model:
                        var model = PixelAdvertisement.GetModelFromCollection(form);
                        model.DateOfAdd = MyHelper.Now;
                        model.Visible = true;
                        model.Clicks = 0;
                        model.ImageFileName = filename;
                        // is model valid?
                        if (!model.IsValid)
                        {
                            TempData["done"] = false;
                            TempData["errors"] = model.ValidationErrors.Select(ve => ve.Value).ToArray();
                            TempData["form"] = form;
                            return RedirectToAction("PixelAdv", "Admin");
                        }
                        // save:
                        ar.Insert(model);
                        // save image:
                        var img_raw = System.Drawing.Image.FromStream(ImageFile.InputStream);
                        var img_fixed = MyHelper.ResizeImage(img_raw, new System.Drawing.Size(model.WidthBlocks.Value * Digits.PixelAdvBlockSides, model.HeightBlocks.Value * Digits.PixelAdvBlockSides));
                        img_fixed.Save(Server.MapPath(Urls.PixelAdvertisements + model.ImageFileName));
                    }
                    else
                    {
                        var id = Convert.ToInt32(form["Id"]);
                        var adv = ar.Get(id);
                        // get model:
                        var model = PixelAdvertisement.GetModelFromCollection(form);
                        model.DateOfAdd = adv.DateOfAdd;
                        model.Visible = adv.Visible;
                        model.Clicks = adv.Clicks;
                        model.ImageFileName = adv.ImageFileName;
                        // is model valid?
                        if (!model.IsValid)
                        {
                            TempData["done"] = false;
                            TempData["errors"] = model.ValidationErrors.Select(ve => ve.Value).ToArray();
                            TempData["form"] = form;
                            return RedirectToAction("PixelAdv", "Admin");
                        }
                        // update:
                        adv.Title = model.Title;
                        adv.Target = model.Target;
                        adv.WidthBlocks = model.WidthBlocks;
                        adv.HeightBlocks = model.HeightBlocks;
                        adv.Position = model.Position;
                        adv.BeginDate = model.BeginDate;
                        adv.EndDate = model.EndDate;
                        adv.Type = model.Type;
                        adv.Cost = model.Cost;
                        adv.PaymentStatus = model.PaymentStatus;
                        adv.Considerations = model.Considerations;
                        // save image:
                        if (ImageFile != null)
                        {
                            bool isValidPhoto = MyHelper.IsSupportedPhotoExtension(MyHelper.GetExtension(ImageFile.FileName));
                            if (!isValidPhoto)
                                throw new Exception(Resources.Messages.NotValidPhotoFormat);
                            // is valid file size?
                            if (ImageFile.ContentLength > Digits.MaximumPhotoBytes)
                                throw new Exception(Resources.Messages.LargerThanMaximumUpload);
                            // save
                            var img_raw = System.Drawing.Image.FromStream(ImageFile.InputStream);
                            var img_fixed = MyHelper.ResizeImage(img_raw, new System.Drawing.Size(model.WidthBlocks.Value * Digits.PixelAdvBlockSides, model.HeightBlocks.Value * Digits.PixelAdvBlockSides));
                            img_fixed.Save(Server.MapPath(Urls.PixelAdvertisements + model.ImageFileName));
                        }
                        // save data:
                        ar.Save();
                    }
                    // redirect:
                    TempData["done"] = true;
                    return RedirectToAction("PixelAdv", "Admin");
                }
            }
            catch(Exception ex)
            {
                TempData["done"] = false;
                TempData["errors"] = new string[]{ ex.Message };
                TempData["form"] = form;
                return RedirectToAction("PixelAdv", "Admin");
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeletePixelAdv(int id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_advs))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var ar = new AdvRepository())
                {
                    ar.Delete(id);
                    return Json(new { Done = true, Id = id });
                }
            }
            catch(Exception ex)
            {
                return Json(new { 
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult EditPixelAdv(int id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_advs))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var ar = new AdvRepository())
                {
                    var adv = ar.Get(id);
                    // return:
                    var beg_date_info = (adv.BeginDate.HasValue ? DateHelper.GetDateInfo(adv.BeginDate.Value) : null);
                    var end_date_info = (adv.EndDate.HasValue ? DateHelper.GetDateInfo(adv.EndDate.Value) : null);
                    var home_pos = adv.GetPosition("home");
                    var homepage_pos = adv.GetPosition("homepage");
                    var profile_pos = adv.GetPosition("profile");
                    return Json(new
                    {
                        Done = true,
                        adv.Id,
                        adv.Title,
                        adv.Target,
                        WidthBlocks = adv.WidthBlocks.HasValue ? adv.WidthBlocks.Value.ToString() : "",
                        HeightBlocks = adv.HeightBlocks.HasValue ? adv.HeightBlocks.Value.ToString() : "",
                        BeginDateDay = beg_date_info != null ? beg_date_info.Day.ToString() : "",
                        BeginDateMonth = beg_date_info != null ? beg_date_info.Month.ToString() : "",
                        BeginDateYear = beg_date_info != null ? beg_date_info.Year.ToString() : "",
                        EndDateDay = end_date_info != null ? end_date_info.Day.ToString() : "",
                        EndDateMonth = end_date_info != null ? end_date_info.Month.ToString() : "",
                        EndDateYear = end_date_info != null ? end_date_info.Year.ToString() : "",
                        Type = adv.Type.HasValue ? adv.Type.Value.ToString() : "",
                        Cost = adv.Cost.HasValue ? adv.Cost.Value.ToString() : "",
                        PaymentStatus = adv.PaymentStatus.HasValue ? adv.PaymentStatus.Value.ToString() : "",
                        adv.Considerations,
                        HomeVis = home_pos.IsVisible,
                        HomeTopIndex = home_pos.IsVisible ? home_pos.TopIndex.ToString() : "",
                        HomeLeftIndex = home_pos.IsVisible ? home_pos.LeftIndex.ToString() : "",
                        HomePageVis = homepage_pos.IsVisible,
                        HomePageTopIndex = homepage_pos.IsVisible ? homepage_pos.TopIndex.ToString() : "",
                        HomePageLeftIndex = homepage_pos.IsVisible ? homepage_pos.LeftIndex.ToString() : "",
                        ProfileVis = profile_pos.IsVisible,
                        ProfileTopIndex = profile_pos.IsVisible ? profile_pos.TopIndex.ToString() : "",
                        ProfileLeftIndex = profile_pos.IsVisible ? profile_pos.LeftIndex.ToString() : ""
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult ChangeVisibilityPixelAdv(int id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_advs))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var ar = new AdvRepository())
                {
                    var adv = ar.Get(id);
                    adv.Visible = !adv.Visible;
                    ar.Save();
                    return Json(new { Done = true, Id = id, NewVisibility = adv.Visible, NewText = (adv.Visible ? Resources.Words.Hide : Resources.Words.Show) });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeleteMemberProfileCover(string id)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_user_mngr))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                System.Threading.Thread.Sleep(3000);
                using (var mr = new MembersRepository())
                {
                    var member = mr.GetByUserName(id);
                    mr.DeleteProfileCover(member.Email);
                    return Json(new { 
                        Done = true
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult SaveManagerSetting(FormCollection form)
        {
            // has manager access:
            if (!Member.IsInAdminGroup(User.Identity.Name, ManagerAccessLevels.core_super_admin))
                throw new HttpException(403, "Access Denied.");
            //
            try
            {
                using (var mr = new MembersRepository())
                {
                    System.Threading.Thread.Sleep(2000);
                    var alias = form["AssociatedMemberAlias"];
                    var member = mr.GetByUserName(alias);
                    var isManager = !string.IsNullOrEmpty(form["IsManager"]) && form["IsManager"].ToLower() == "on";
                    if (isManager)
                    {
                        var access = "";
                        access += !string.IsNullOrEmpty(form["core_spec_posts"]) && form["core_spec_posts"].ToLower() == "on" ? 
                            (access.Trim().Length > 0 ? ",core_spec_posts" : "core_spec_posts") : "";
                        access += !string.IsNullOrEmpty(form["core_user_mngr"]) && form["core_user_mngr"].ToLower() == "on" ?
                            (access.Trim().Length > 0 ? ",core_user_mngr" : "core_user_mngr") : "";
                        access += !string.IsNullOrEmpty(form["core_acts"]) && form["core_acts"].ToLower() == "on" ?
                            (access.Trim().Length > 0 ? ",core_acts" : "core_acts") : "";
                        access += !string.IsNullOrEmpty(form["core_comments"]) && form["core_comments"].ToLower() == "on" ?
                            (access.Trim().Length > 0 ? ",core_comments" : "core_comments") : "";
                        access += !string.IsNullOrEmpty(form["core_obj_check"]) && form["core_obj_check"].ToLower() == "on" ?
                            (access.Trim().Length > 0 ? ",core_obj_check" : "core_obj_check") : "";
                        access += !string.IsNullOrEmpty(form["core_pages"]) && form["core_pages"].ToLower() == "on" ?
                            (access.Trim().Length > 0 ? ",core_pages" : "core_pages") : "";
                        access += !string.IsNullOrEmpty(form["core_advs"]) && form["core_advs"].ToLower() == "on" ?
                            (access.Trim().Length > 0 ? ",core_advs" : "core_advs") : "";
                        access += !string.IsNullOrEmpty(form["core_super_admin"]) && form["core_super_admin"].ToLower() == "on" ?
                            (access.Trim().Length > 0 ? ",core_super_admin" : "core_super_admin") : "";
                        if (member.Manager != null)
                        {
                            member.Manager.AccessLevel = access;
                            mr.Save();
                        }
                        else
                        {
                            var mngr = new Manager();
                            mngr.MemberId = member.Email;
                            mngr.AccessLevel = access;
                            mr.Insert(mngr);
                            System.Web.Security.Roles.AddUserToRole(member.Email, MyRoles.Manager);
                        }
                    }
                    else
                    {
                        System.Web.Security.Roles.RemoveUserFromRole(member.Email, MyRoles.Manager);
                        mr.DeleteManagerSetting(member.Email);
                    }
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }
        #endregion

        #region PRIVATE METHODS:
        private string GenerateAdvFileName(string ext)
        {
            var filename = MyHelper.GetRandomString(10, true) + "." + ext;
            if (System.IO.File.Exists(Server.MapPath(Urls.PixelAdvertisements + filename)))
            {
                return GenerateAdvFileName(ext);
            }
            return filename;
        }
        #endregion
    }
}
