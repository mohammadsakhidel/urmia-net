using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace CoreHelpers
{
    public static class TextsProcessor
    {
        private static string ReplaceHtmlLines(string originalString)
        {
            originalString = String.IsNullOrEmpty(originalString) ? "" : originalString;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringReader sr = new System.IO.StringReader(originalString);
            string tmpS = null;
            do
            {
                tmpS = sr.ReadLine();
                if (tmpS != null)
                {
                    sb.Append(tmpS);
                    sb.Append("<br />");
                }
            } while (tmpS != null);
            var convertedString = sb.ToString();
            return convertedString;
        }
        private static string ReplaceEmoticons(string originalText)
        {
            string converted = originalText;
            var emoticons = Dictionaries.Emoticons;
            var emoticonsPath = MyHelper.ToAbsolutePath(Urls.Emoticons);
            foreach (var emoticon in emoticons)
            {
                string img = "<img src=\"" + emoticonsPath + emoticon.Value + ".png" + "\" class=\"emoticon\" />";
                converted = converted.Replace(emoticon.Key, img);
            }
            return converted;
        }
        private static string GetDirection(string text)
        {
            // remove links:
            text = Regex.Replace(text, @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", "");
            // remove html tags: 
            text = Regex.Replace(text, @"<[^>]*>", "");
            // remove special characters:
            text = Regex.Replace(text, @"&quot;|['"",&?%\.*:#/\\-]", "").Replace("!", "").Replace("@", "").Replace("$", "").Replace("_", "");
            // remove white spaces:
            text = text.Replace(" ", "").Replace(Environment.NewLine, "");
            //
            var rtls = 0;
            var ltrs = 0;
            foreach (char c in text)
            {
                var uc = new UnicodeChar(c);
                if (uc.Direction == "rtl")
                    rtls++;
                else ltrs++;
            }
            return rtls >= ltrs ? "rtl" : "ltr";
        }
        private static string ReplaceYoutubeUrls(string text, int playerWidth, int playerHeight)
        {
            var matches = Regex.Matches(text, @"(https?://(www\.)?youtube\.com/.*v=\w+.*)|(https?://youtu\.be/\w+.*)|(.*src=.https?://(www\.)?youtube\.com/v/\w+.*)|(.*src=.https?://(www\.)?youtube\.com/embed/\w+.*)");
            foreach (var match in matches)
            {
                text = text.Replace(match.ToString(), YoutubeHelper.GetFrameFromUrl(match.ToString(), playerWidth, playerHeight));
            }
            return text;
        }
        public static string RenderText(string text)
        {
            return RenderText(text, false, null, null);
        }
        public static string RenderText(string text, bool renderFrame, int? playerWidth, int? playerHeight)
        {
            // encode html:
            string rText = HttpContext.Current.Server.HtmlEncode(text);
            // replace youtube frame:
            if (renderFrame)
            {
                rText = ReplaceYoutubeUrls(rText, playerWidth.Value, playerHeight.Value);
            }
            // get direction:
            var dir = GetDirection(rText);
            // replace lines with <br />:
            rText = ReplaceHtmlLines(rText);
            // emoticons:
            rText = ReplaceEmoticons(rText);
            // allow a links:
            rText = Regex.Replace(rText,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "<a target='_blank' href='$1'>$1</a>");
            // set direction:
            rText = "<div style=\"direction: " + dir + "; display: inline-block; max-width: 100%; \">" + rText + "</div>";
            // return:
            return rText;
        }
    }
}