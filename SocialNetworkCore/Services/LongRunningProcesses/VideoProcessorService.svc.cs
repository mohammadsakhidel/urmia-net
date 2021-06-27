using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Services.LongRunningProcesses
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class VideoProcessorService : IVideoProcessorService
    {
        public void ProcessPostVideo(string inPath, string outPath, string processorPath, 
            int videoFixedWidth, int videoFixedHeight, int associatedVideoRecord)
        {
            if (System.IO.File.Exists(inPath))
            {
                Task.Factory.StartNew(() =>
                            processPostVideo(inPath, outPath, processorPath, videoFixedWidth, videoFixedHeight, associatedVideoRecord),
                            TaskCreationOptions.LongRunning);
            }
            else
            {
                saveVideoProcessResult(associatedVideoRecord, VideoProcessResult.Failed, "Temp video file not found.");
                deleteInputFile(inPath);
            }
        }

        private void processPostVideo(string inPath, string outPath, string processorPath, int videoFixedWidth, int videoFixedHeight, int associatedVideoRecord)
        {
            try
            {
                var metadata = VideoHelper.GetVideoMetadata(inPath, processorPath);
                var resizer = metadata.Size.HasValue ?
                    new Resizer(metadata.Size.Value.Width, metadata.Size.Value.Height, ResizeType.WidthFix, videoFixedWidth) :
                    null;
                VideoHelper.Convert(inPath, outPath, new System.Drawing.Size { Width = videoFixedWidth, Height = (resizer != null ? resizer.NewHeight : videoFixedHeight) });
                saveVideoProcessResult(associatedVideoRecord, VideoProcessResult.Successfull, "");
                deleteInputFile(inPath);
            }
            catch(Exception ex)
            {
                saveVideoProcessResult(associatedVideoRecord, VideoProcessResult.Failed, ex.Message.Substring(0, 1000));
                deleteInputFile(inPath);
            }
        }

        private void saveVideoProcessResult(int videoId, VideoProcessResult res, string message)
        {
            var vr = new VideoRepository();
            var video = vr.Get(videoId);
            video.ProcessResult = (byte)res;
            video.ProcessResultMessage = message;
            vr.Save();
        }

        private void deleteInputFile(string inputPath)
        {
            if (System.IO.File.Exists(inputPath))
                System.IO.File.Delete(inputPath);
        }
    }
}
