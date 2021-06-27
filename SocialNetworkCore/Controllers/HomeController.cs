using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetApp.Models.Repository;
using SocialNetApp.Views.Models.Home;
using CoreHelpers;
using Facebook;
using System.Dynamic;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Controllers
{
    public class HomeController : MainController
    {
        #region Testing Facebook Share:
        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult TestFb()
        {
            try
            {
                var appId = System.Configuration.ConfigurationManager.AppSettings["FbAppId"];
                var appSec = System.Configuration.ConfigurationManager.AppSettings["FbAppSec"];
                var fbClient = new FacebookClient();
                dynamic token = fbClient.Get("oauth/access_token", new
                {
                    client_id = appId,
                    client_secret = appSec,
                    grant_type = "client_credentials"
                });
                fbClient.AccessToken = token.access_token;
                //
                dynamic parameters = new ExpandoObject();
                parameters.title = "A test message.";
                parameters.message = "This is a post for testing facebook integration in my App.";
                var result = fbClient.Post("812604868795650/feed", parameters);
                return Json(new { Done = true, Message = fbClient.AccessToken });
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Message = ex.Message });
            }
        }
        #endregion

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "HomePage");
            }
            else
            {
                using (var mr = new MembersRepository())
                {
                    var model = SocialNetApp.Views.Models.Home.IndexViewModel.Create(mr.Context);
                    return View(model);
                }
            }
        }

        public ActionResult PageNotFoundError()
        {
            if (!Request.IsAjaxRequest())
            {
                var model = PageNotFoundErrorViewModel.Create(null);
                return View(model);
            }
            else
            {
                var model = ErrorDialogModel.Create("Internal Server Error.", null);
                return View(Urls.ModuleViews + "_ErrorDialog.cshtml", model);
            }
        }

        public ActionResult InternalServerError()
        {
            if (!Request.IsAjaxRequest())
            {
                var model = InternalServerErrorViewModel.Create(null);
                return View(model);
            }
            else
            {
                var model = ErrorDialogModel.Create("Internal Server Error.", null);
                return View(Urls.ModuleViews + "_ErrorDialog.cshtml", model);
            }
        }

        public void Go(string dest, string src, string info)
        {
            try
            {
                using (var advrep = new AdvRepository())
                {
                    if (src == "pixeladv")
                    {
                        var adv = advrep.Get(Convert.ToInt32(info));
                        adv.Clicks = adv.Clicks + 1;
                        advrep.Save();
                    }
                }
            }
            catch
            {
            }
            Response.Redirect(dest);
        }
    }
}
