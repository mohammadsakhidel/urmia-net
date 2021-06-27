using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using System.IO;
using SocialNetApp.Views.Models.Layouts;

namespace SocialNetApp.Controllers
{
    public abstract class MainController : Controller
    {
        public MainController()
        {
            // site views theme setting:
            SiteViewModel.Theme = Configs.DefaultTheme;
            SiteViewModel.Language = Configs.DefaultLanguage;//"kur";
            SiteViewModel.StyleSheetsUrl = String.Format("~/App_Themes/{0}/{1}/", SiteViewModel.Theme, SiteViewModel.Language);
            // set current culture:
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SiteViewModel.Language);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(SiteViewModel.Language);
            // site layout view model:
            var siteViewModel = SiteViewModel.Create();
            ViewBag.SiteViewModel = siteViewModel;
        }

        public string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}