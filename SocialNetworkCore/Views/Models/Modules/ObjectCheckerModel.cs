using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class ObjectCheckerModel
    {
        #region Properties:
        public DateTime DateToCheck { get; set; }
        public string DateToCheckString { get; set; }
        public List<Tuple<Activity, object>> ActivityRecords { get; set; }
        public string LastActivityDateString { get; set; }
        public bool IsThereMoreActs { get; set; }
        public ViewMoreModel ViewMoreModel { get; set; }
        #endregion

        #region Methods:
        public static ObjectCheckerModel Create(System.Data.Objects.ObjectContext context)
        {
            var ar = new ActivityRepository(context);
            context = ar.Context;
            var param = new Dictionary<string, object>();
            param.Add("AVCL", ActivityVisibilityCheckLevel.None);
            ///////////////////////////////////////
            var model = new ObjectCheckerModel();
            model.DateToCheck = HttpContext.Current.Request.QueryString["dt"] != null ? Convert.ToDateTime(HttpContext.Current.Request.QueryString["dt"].ToString()) : MyHelper.Now.Date;
            model.DateToCheckString = model.DateToCheck.ToString("yyyy-MM-dd");
            //---- act records:
            var acts = ar.FindActivitiesForAdmin(model.DateToCheck).ToList();
            var actRecords = new List<Tuple<Activity, object>>();
            foreach (var act in acts)
            {
                var actModel = ActivityModel.Create(act, param, context);
                actRecords.Add(new Tuple<Activity, object>(act, actModel));
            }
            //----
            model.ActivityRecords = actRecords;
            model.LastActivityDateString = (model.ActivityRecords.Any() ? model.ActivityRecords.Last().Item1.TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
            model.IsThereMoreActs = (model.ActivityRecords.Any() ? ar.IsThereMoreActivitiesForAdmin(MyHelper.Now.Date, model.ActivityRecords.Last().Item1.TimeOfAct) : false);
            model.ViewMoreModel = ViewMoreModel.Create("GetMoreActivitiesForAdmin", "Admin", "oc_activities", model.LastActivityDateString, model.IsThereMoreActs, model.DateToCheckString, context);
            return model;
        }
        #endregion
    }
}