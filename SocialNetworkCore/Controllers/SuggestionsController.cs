using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Controllers
{
    public class SuggestionsController : MainController
    {
        #region GET ACTIONS:
        [AcceptAjax]
        [Authorize(Roles=MyRoles.Member)]
        public ActionResult FindEducationBranch(string query)
        {
            using (var sr = new SuggestionsRepository())
            {
                var list = sr.FindSuggestedEducationBranch(query, Digits.MaxAutoCompleteItems);
                return Json(new
                {
                    suggestions = list.ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult FindEducationLocation(string query)
        {
            using (var sr = new SuggestionsRepository())
            {
                var list = sr.FindSuggestedEducationLocation(query, Digits.MaxAutoCompleteItems);
                return Json(new
                {
                    suggestions = list.ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult FindSkill(string query)
        {
            using (var sr = new SuggestionsRepository())
            {
                var list = sr.FindSuggestedSkill(query, Digits.MaxAutoCompleteItems);
                return Json(new
                {
                    suggestions = list.ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult FindLivingCity(string query)
        {
            using (var sr = new SuggestionsRepository())
            {
                var list = sr.FindSuggestedLivingCities(query, Digits.MaxAutoCompleteItems);
                return Json(new
                {
                    suggestions = list.ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult FindTags(string query)
        {
            using (var sr = new SuggestionsRepository())
            {
                return Json(new
                {
                    suggestions = sr.FindSuggestedTags(query, Digits.MaxAutoCompleteItems)
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
