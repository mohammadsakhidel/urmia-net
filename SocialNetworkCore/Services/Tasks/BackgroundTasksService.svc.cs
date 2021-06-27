using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CoreHelpers;
using System.ServiceModel.Activation;

namespace SocialNetApp.Services.Tasks
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BackgroundTasksService : IBackgroundTasksService
    {
        public void RefreshOnlines(string logPath)
        {
            OnlinesHelper.Refresh(System.Web.HttpContext.Current);
            System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath(logPath), string.Format("RefreshOnlines executed at: {0} {1} {2}", MyHelper.Now.ToLongDateString(), MyHelper.Now.ToLongTimeString(), Environment.NewLine));
        }

        public void RefreshChatSessions(string logPath)
        {
            ChatHelper.RefreshChatSessions();
            System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath(logPath), string.Format("RefreshChatSessions executed at: {0} {1} {2}", MyHelper.Now.ToLongDateString(), MyHelper.Now.ToLongTimeString(), Environment.NewLine));
        }


        public void ClearTempFolder(string tempFolderUrl, string logUrl)
        {
            string rootPath = System.Web.HttpContext.Current.Server.MapPath(tempFolderUrl);
            var fileNames = System.IO.Directory.GetFiles(rootPath);
            foreach (var fn in fileNames)
            {
                var lastWriteDate = System.IO.File.GetLastWriteTime(fn);
                if (lastWriteDate.AddDays(1) < MyHelper.Now)
                {
                    System.IO.File.Delete(fn);
                }
            }
            System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath(logUrl), string.Format("ClearTempFolder executed at: {0} {1} {2}", MyHelper.Now.ToLongDateString(), MyHelper.Now.ToLongTimeString(), Environment.NewLine));
        }
    }
}
