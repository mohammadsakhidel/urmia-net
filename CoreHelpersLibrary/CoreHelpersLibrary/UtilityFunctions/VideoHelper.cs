using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using NReco.VideoConverter;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CoreHelpers
{
    public class VideoHelper
    {
        public static System.Drawing.Image GetVideoThumbnail(string filePath)
        {
            Stream thumb = new MemoryStream();
            var converter = new FFMpegConverter();
            converter.GetVideoThumbnail(filePath, thumb);
            Image img = Image.FromStream(thumb);
            return img;
        }

        public static void Convert(string inputFilePath, string outputFilePath, Size size)
        {
            FFMpegConverter c = new FFMpegConverter();
            var sett = new ConvertSettings();
            sett.SetVideoFrameSize(size.Width, size.Height);
            sett.VideoFrameRate = 30;
            var informat = MyHelper.GetExtension(inputFilePath);
            var outformat = MyHelper.GetExtension(outputFilePath);
            c.ConvertMedia(inputFilePath, informat, outputFilePath, outformat, sett);
        }

        public static VideoMetadata GetVideoMetadata(string filePath, string processPath)
        {
            var metadata = new VideoMetadata();
            //  set up the parameters for video info.
            string @params = string.Format("-i \"{0}\"", filePath);
            string output = Run(processPath, @params);
            //get the video size:
            var reg_size = new Regex("(\\d{2,4})x(\\d{2,4})");
            Match matchedSize = reg_size.Match(output);
            if (matchedSize.Success)
            {
                int width = 0; int height = 0;
                int.TryParse(matchedSize.Groups[1].Value, out width);
                int.TryParse(matchedSize.Groups[2].Value, out height);
                metadata.Size = new Size(width, height);
            }

            //get video duration:
            var reg_duration = new Regex("[D|d]uration:.((\\d|:|\\.)*)");
            Match matchedDuration = reg_duration.Match(output);
            if (matchedDuration.Success)
            {
                string temp = matchedDuration.Groups[1].Value;
                string[] timepieces = temp.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4)
                {
                    // Store duration
                    metadata.Duration = new TimeSpan(0, System.Convert.ToInt16(timepieces[0]),
                        System.Convert.ToInt16(timepieces[1]),
                        System.Convert.ToInt16(timepieces[2]),
                        System.Convert.ToInt16(timepieces[3]));
                }
            }
            return metadata;
        }

        private static string Run(string process, string parameters)
        {
            string output = string.Empty;
            StreamReader outputStream = null;
            try
            {
                //  Create a process info.
                ProcessStartInfo oInfo = new ProcessStartInfo(process, parameters);
                oInfo.UseShellExecute = false;
                oInfo.CreateNoWindow = true;
                oInfo.RedirectStandardOutput = true;
                oInfo.RedirectStandardError = true;

                //  Run the process.
                Process proc = System.Diagnostics.Process.Start(oInfo);
                proc.WaitForExit();

                outputStream = proc.StandardError;
                output = outputStream.ReadToEnd();

                proc.Close();
            }
            catch
            {
                output = "";
            }
            finally
            {
                //  Close out the streamreader.
                if (outputStream != null)
                    outputStream.Close();
            }
            return output;
        }
    }
}
