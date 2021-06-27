using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class ActivityInfoModel
    {
        #region properties:
        public DateTime TimeOfActivity { get; set; }
        public byte? VisibleTo { get; set; }
        public string TimeOfActivityText
        {
            get
            {
                string txt = CoreHelpers.MyHelper.DateToText(this.TimeOfActivity);
                return txt;
            }
        }
        public string DetailsPageUrl { get; set; }
        #endregion

        #region methods:
        public static ActivityInfoModel Create(DateTime actTime, byte? visibleTo, string detailsPageUrl, System.Data.Objects.ObjectContext context)
        {
            var model = new ActivityInfoModel();
            model.TimeOfActivity = actTime;
            model.VisibleTo = visibleTo;
            model.DetailsPageUrl = detailsPageUrl;
            return model;
        }
        #endregion
    }
}