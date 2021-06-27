using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using System.Web.Mvc;

namespace SocialNetApp.Views.Models.Modules
{
    public class PixelAdvManagerModel
    {
        #region Properties:
        public bool IsPostBack { get; set; }
        public List<PixelAdvertisement> Ads { get; set; }
        public bool Done { get; set; }
        public List<string> Errors { get; set; }
        public FormCollection Form { get; set; }
        public string FormAction { get; set; }
        #endregion 

        #region Methods:
        public static PixelAdvManagerModel Create(Member member, TempDataDictionary dic, System.Data.Objects.ObjectContext context)
        {
            var ar = new AdvRepository(context);
            bool? done = dic["done"] != null ? (bool?)dic["done"] : (bool?)null;
            string[] errors = dic["errors"] != null ? (string[])dic["errors"] : null;
            FormCollection form = dic["form"] != null ? (FormCollection)dic["form"] : null;
            ///////////////////////////////////////
            var model = new PixelAdvManagerModel();
            model.Ads = ar.GetAll().ToList();
            model.IsPostBack = done.HasValue;
            model.Form = form;
            model.FormAction = model.IsPostBack && !done.Value ? model.Form["FormAction"] : "new";
            model.Errors = errors != null ? errors.ToList() : new List<string>();
            return model;
        }
        #endregion
    }
}