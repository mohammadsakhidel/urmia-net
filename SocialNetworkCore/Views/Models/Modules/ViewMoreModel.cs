using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ViewMoreModel
    {
        #region Properties:
        public string RandomId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string UpdateTargetId { get; set; }
        public string CurrentPageIndex { get; set; }
        public bool IsThereMore { get; set; }
        public string Parameters { get; set; }
        #endregion

        #region Methods:
        public static ViewMoreModel Create(string actionName, string controllerName, string targetId, string pageIndex, bool isThereMore, string parameters, System.Data.Objects.ObjectContext context)
        {
            var model = new ViewMoreModel();
            model.IsThereMore = isThereMore;
            if (model.IsThereMore)
            {
                model.RandomId = MyHelper.GetRandomString(5, true);
                model.ActionName = actionName;
                model.ControllerName = controllerName;
                model.UpdateTargetId = targetId;
                model.CurrentPageIndex = pageIndex;
                model.Parameters = parameters;
            }
            return model;
        }
        #endregion
    }
}