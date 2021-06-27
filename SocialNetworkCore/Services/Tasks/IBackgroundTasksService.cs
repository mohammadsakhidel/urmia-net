using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;

namespace SocialNetApp.Services.Tasks
{
    [ServiceContract]
    public interface IBackgroundTasksService
    {
        [OperationContract]
        void RefreshOnlines(string logPath);

        [OperationContract]
        void RefreshChatSessions(string logPath);

        [OperationContract]
        void ClearTempFolder(string tempFolderUrl, string logUrl);
    }
}
