using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetApp.Models.Repository;
using CoreHelpers;
using SocialNetApp.Views.Models.Pages;

namespace SocialNetApp.Controllers
{
    public class PagesController : MainController
    {
        public ActionResult Page(string name)
        {
            using (var pr = new PagesRepository())
            {
                var pg = pr.Get(name);
                return View("~/Views/Pages/Page.cshtml", 
                    PageViewModel.Create(pg, pr.Context));
            }
        }
    }
}
