using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SocialNetApp.Services.LongRunningProcesses
{
    [ServiceContract]
    public interface IVideoProcessorService
    {
        [OperationContract]
        void ProcessPostVideo(string inPath, string outPath, string processorPath, int videoFixedWidth, int videoFixedHeight, int associatedVideoRecord);
    }
}
