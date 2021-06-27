using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class VideoPlayerModel
    {
        public Video AssociatedVideo { get; set; }
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public string PlayerId { get; set; }

        public static VideoPlayerModel Create(Video video, int? displayWidth, System.Data.Objects.ObjectContext context)
        {
            var model = new VideoPlayerModel();
            model.PlayerId = MyHelper.GetRandomString(15, true);
            model.AssociatedVideo = video;
            model.DisplayWidth = displayWidth ?? Digits.VideosDisplayWidth;
            model.DisplayHeight = (model.AssociatedVideo.Width.HasValue && model.AssociatedVideo.Height.HasValue ?
                calcVideoDisplayWidth(model.AssociatedVideo.Width.Value, model.AssociatedVideo.Height.Value, displayWidth) :
                (Digits.VideosDisplayWidth * 9) / 16);
            return model;
        }

        private static int calcVideoDisplayWidth(int actualWidth, int actualHeight, int? displayWidth)
        {
            const int adjustingPixels = 32;  // for adjusting flowplayer, the height of controls bar.
            if (actualHeight < actualWidth)
            {
                var resizer = new Resizer(actualWidth, actualHeight, ResizeType.WidthFix, (displayWidth ?? Digits.VideosDisplayWidth));
                return resizer.NewHeight + adjustingPixels;
            }
            else
            {
                var resizer = new Resizer(actualWidth, actualHeight, ResizeType.HeightFix, (int)((displayWidth ?? Digits.VideosDisplayWidth) * 0.75));
                return resizer.NewHeight + adjustingPixels;
            }
        }
    }
}