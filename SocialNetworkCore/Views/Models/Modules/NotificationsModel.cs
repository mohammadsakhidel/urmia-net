using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class NotificationsModel
    {
        #region Properties:
        public IEnumerable<NotificationModel> NotificationModels { get; set; }
        public ViewMoreModel ViewMoreModel { get; set; }
        #endregion

        #region Methods:
        public static NotificationsModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            // get data from db:
            var nr = new NotificationRepository(context);
            context = nr.Context;
            var notifications = nr.GetPagedNotifications(HttpContext.Current.User.Identity.Name, 0).ToList();
            var isThereMore = nr.IsThereMoreNotifications(HttpContext.Current.User.Identity.Name, 0);
            //Notification modules model
            List<NotificationModel> notificationModels = new List<NotificationModel>();
            foreach (var not in notifications)
            {
                var notModel = NotificationModel.Create(not, context);
                notificationModels.Add(notModel);
            }
            // gather NotificationsModel:
            var notsModel = new NotificationsModel();
            notsModel.NotificationModels = notificationModels;
            notsModel.ViewMoreModel = ViewMoreModel.Create("GetMoreNotifications", "HomePage", "notifications_list", "0", isThereMore, "", context);
            // set notificatins as read:
            foreach (var n in notifications)
            {
                n.Status = (byte)NotificationStatus.Read;
            }
            nr.Save();
            return notsModel;
        }
        #endregion
    }
}