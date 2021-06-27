using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class HmiNotificationsModel
    {
        #region
        public List<NotificationModel> NotificationModels { get; set; }
        #endregion
        
        #region
        public static HmiNotificationsModel Create(System.Data.Objects.ObjectContext context)
        {
            var nr = new NotificationRepository(context);
            context = nr.Context;
            // notifications:
            var notificationModels = new List<NotificationModel>();
            var notifications = nr.GetNotifications(HttpContext.Current.User.Identity.Name, Digits.MaxHmiItems);
            foreach (var n in notifications)
            {
                var notModel = NotificationModel.Create(n, context);
                notificationModels.Add(notModel);
                n.Status = (byte)NotificationStatus.Read;
            }
            nr.Save();
            ////////////////////////////////////
            var model = new HmiNotificationsModel();
            model.NotificationModels = notificationModels;
            return model;
        }
        #endregion
    }
}