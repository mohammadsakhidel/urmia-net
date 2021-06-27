using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Net;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using SocialNetApp.CoreServices.Tasks;

namespace CoreHelpers
{
    public static class Tasks
    {
        public static void RefreshOnlines(WebTaskEventArgs e)
        {
            using (var svcClient = new BackgroundTasksServiceClient())
            {
                svcClient.RefreshOnlines("~/Content/Logs/TasksLog.txt");
            }
        }
        public static void RefreshChatSessions(WebTaskEventArgs e)
        {
            using (var svcClient = new BackgroundTasksServiceClient())
            {
                svcClient.RefreshChatSessions("~/Content/Logs/TasksLog.txt");
            }
        }
        public static void ClearTempFolder(WebTaskEventArgs e)
        {
            using (var svcClient = new BackgroundTasksServiceClient())
            {
                svcClient.ClearTempFolder("~/Content/Temp/", "~/Content/Logs/TasksLog.txt");
            }
        }
    }

}