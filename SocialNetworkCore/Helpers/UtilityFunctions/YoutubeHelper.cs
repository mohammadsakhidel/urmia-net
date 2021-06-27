using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public static class YoutubeHelper
    {
        public static string GetFrameFromUrl(string url, int width, int height)
        {
            var qs = MyHelper.ExtractQuerystringFromUrl(url);
            var frame = string.Format(
                "<iframe src=\"//www.youtube.com/embed/{2}\" class=\"youtube_frame\" width=\"{0}\" height=\"{1}\" frameborder=\"0\" allowfullscreen></iframe>",
                width, height, qs["v"]);
            return frame;
        }
    }
}