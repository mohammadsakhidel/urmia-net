﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialNetApp.CoreServices.LongRunningProcesses {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CoreServices.LongRunningProcesses.IVideoProcessorService")]
    public interface IVideoProcessorService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVideoProcessorService/ProcessPostVideo", ReplyAction="http://tempuri.org/IVideoProcessorService/ProcessPostVideoResponse")]
        void ProcessPostVideo(string inPath, string outPath, string processorPath, int videoFixedWidth, int videoFixedHeight, int associatedVideoRecord);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IVideoProcessorServiceChannel : SocialNetApp.CoreServices.LongRunningProcesses.IVideoProcessorService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class VideoProcessorServiceClient : System.ServiceModel.ClientBase<SocialNetApp.CoreServices.LongRunningProcesses.IVideoProcessorService>, SocialNetApp.CoreServices.LongRunningProcesses.IVideoProcessorService {
        
        public VideoProcessorServiceClient() {
        }
        
        public VideoProcessorServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public VideoProcessorServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VideoProcessorServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VideoProcessorServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void ProcessPostVideo(string inPath, string outPath, string processorPath, int videoFixedWidth, int videoFixedHeight, int associatedVideoRecord) {
            base.Channel.ProcessPostVideo(inPath, outPath, processorPath, videoFixedWidth, videoFixedHeight, associatedVideoRecord);
        }
    }
}
