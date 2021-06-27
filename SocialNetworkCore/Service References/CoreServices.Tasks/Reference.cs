﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialNetApp.CoreServices.Tasks {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CoreServices.Tasks.IBackgroundTasksService")]
    public interface IBackgroundTasksService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBackgroundTasksService/RefreshOnlines", ReplyAction="http://tempuri.org/IBackgroundTasksService/RefreshOnlinesResponse")]
        void RefreshOnlines(string logPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBackgroundTasksService/RefreshChatSessions", ReplyAction="http://tempuri.org/IBackgroundTasksService/RefreshChatSessionsResponse")]
        void RefreshChatSessions(string logPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBackgroundTasksService/ClearTempFolder", ReplyAction="http://tempuri.org/IBackgroundTasksService/ClearTempFolderResponse")]
        void ClearTempFolder(string tempFolderUrl, string logUrl);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBackgroundTasksServiceChannel : SocialNetApp.CoreServices.Tasks.IBackgroundTasksService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BackgroundTasksServiceClient : System.ServiceModel.ClientBase<SocialNetApp.CoreServices.Tasks.IBackgroundTasksService>, SocialNetApp.CoreServices.Tasks.IBackgroundTasksService {
        
        public BackgroundTasksServiceClient() {
        }
        
        public BackgroundTasksServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BackgroundTasksServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BackgroundTasksServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BackgroundTasksServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void RefreshOnlines(string logPath) {
            base.Channel.RefreshOnlines(logPath);
        }
        
        public void RefreshChatSessions(string logPath) {
            base.Channel.RefreshChatSessions(logPath);
        }
        
        public void ClearTempFolder(string tempFolderUrl, string logUrl) {
            base.Channel.ClearTempFolder(tempFolderUrl, logUrl);
        }
    }
}
