using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Web.Mvc;

namespace CoreHelpers
{
    public partial class MyHelper
    {
        #region Date and time related:

        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static DateTime ChangeTime(DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }

        public static string DateToText_WithoutTime(DateTime dt)
        {
            return DateHelper.DateToText(dt);
        }

        public static string DateToText(DateTime dt)
        {
            return DateHelper.DateTimeToText(dt);
        }

        public static string DateToShortText(DateTime dt)
        {
            return DateHelper.DateToShortText(dt);
        }
        #endregion

        #region Url related:

        public static string ToAbsolutePath(string Url)
        {
            return VirtualPathUtility.ToAbsolute(Url);
        }

        public static string ResolveUrl(string path, bool forceHttps)
        {
            if (path.StartsWith("~"))
                path = ToAbsolutePath(path);
            var fullUrl = (forceHttps ? "https://" : "http://") + Configs.DomainName + (path.StartsWith("/") ? "" : "/") + path;
            return fullUrl;
        }

        public static Dictionary<string, string> ExtractQuerystringFromUrl(string url)
        {
            var res = new Dictionary<string, string>();
            var l = url.Length;
            var i = url.IndexOf('?');
            var qs = url.Substring(i + 1, l - i - 1);
            var nameValues = MyHelper.SplitString(qs, "&");
            foreach (var nv in nameValues)
            {
                var splitedNV = MyHelper.SplitString(nv, "=");
                res.Add(splitedNV[0], splitedNV[1]);
            }
            return res;
        }

        public static Dictionary<string, string> ExtractQuerystringFromValues(string qs)
        {
            var res = new Dictionary<string, string>();
            var nameValues = MyHelper.SplitString(qs, "&");
            foreach (var nv in nameValues)
            {
                var splitedNV = MyHelper.SplitString(nv, "=");
                res.Add(splitedNV[0], splitedNV[1]);
            }
            return res;
        }

        #endregion

        #region Text related:

        public static string CutString(object text, int maxLenght)
        {
            string res = "";
            if (text.ToString().Length < maxLenght)
            {
                res = text.ToString();
            }
            else
            {
                res = text.ToString().Substring(0, maxLenght) + " ...";
            }
            return res;
        }

        public static List<string> SplitString(string text, string splitter)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Regex rgx = new Regex(splitter);
                string[] array = rgx.Split(text);
                List<string> list = new List<string>();
                foreach (string st in array)
                {
                    if (st.Trim().Length > 0)
                        list.Add(st);
                }
                return list;
            }
            else
            {
                return new List<string>();
            }
        }

        public static string ExtractMobNumber(string rawNumber)
        {
            string res = "";
            if (rawNumber.Length >= 10)
                res = rawNumber.Substring(rawNumber.Length - 10, 10);
            return res;
        }

        public static bool IsValidMobNumber(string rawNumber)
        {
            bool res = false;
            if (rawNumber.Length >= 10 && ExtractMobNumber(rawNumber).StartsWith("9"))
            {
                res = true;
            }
            return res;
        }

        public static List<string> GetLines(string text)
        {
            Regex rgx = new Regex(Environment.NewLine);
            string[] array = rgx.Split(text);
            List<string> list = new List<string>();
            foreach (string st in array)
            {
                if (st.Trim().Length > 0)
                    list.Add(st);
            }
            return list;
        }

        public static bool IsInt32(string text)
        {
            bool res = false;
            int outInt = 0;
            res = Int32.TryParse(text, out outInt);
            return res;
        }

        public static bool IsByte(string text)
        {
            bool res = false;
            byte outByte = 0;
            res = Byte.TryParse(text, out outByte);
            return res;
        }

        public static bool IsDouble(string text)
        {
            bool res = false;
            double outDouble = 0.0;
            res = double.TryParse(text, out outDouble);
            return res;
        }

        public static bool IsInt16(string text)
        {
            bool res = false;
            short outInt = 0;
            res = Int16.TryParse(text, out outInt);
            return res;
        }

        public static bool IsEmailAddress(string emailAddress)
        {
            string pattern = Patterns.Email;
            Regex rgx = new Regex(pattern);
            bool isMatch = rgx.IsMatch(emailAddress);
            return isMatch;
        }

        public static string ToUrlText(string text)
        {
            return text.Replace(' ', '-');
        }

        public static string GetRandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = RandomProvider.GetThreadRandom();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static List<int> GetRandomNumbers(int includeMin, int excludeMax, int randomCounts)
        {
            List<int> randoms = new List<int>();
            Random generator = RandomProvider.GetThreadRandom();
            while (randoms.Count() < randomCounts && randoms.Count() < excludeMax - includeMin)
            {
                int rNumber = generator.Next(includeMin, excludeMax);
                if (!randoms.Contains(rNumber))
                    randoms.Add(rNumber);
            }
            return randoms;
        }

        public static string Erase(string text, string eraseText)
        {
            return text.Replace(eraseText, "");
        }

        public static string GetPersianMonthName(int month)
        {
            string name = "";
            switch (month)
            {
                case 1:
                    name = "فروردین";
                    break;
                case 2:
                    name = "اردیبهشت";
                    break;
                case 3:
                    name = "خرداد";
                    break;
                case 4:
                    name = "تیر";
                    break;
                case 5:
                    name = "مرداد";
                    break;
                case 6:
                    name = "شهریور";
                    break;
                case 7:
                    name = "مهر";
                    break;
                case 8:
                    name = "آبان";
                    break;
                case 9:
                    name = "آذر";
                    break;
                case 10:
                    name = "دی";
                    break;
                case 11:
                    name = "بهمن";
                    break;
                case 12:
                    name = "اسفند";
                    break;
            }
            return name;
        }

        public static int ClearInt(string digitText)
        {
            string newDigitText = "";
            foreach (char c in digitText)
            {
                if (c >= '0' && c <= '9')
                    newDigitText += c.ToString();
            }
            return Convert.ToInt32(newDigitText);
        }

        public static double ClearDouble(string digitText)
        {
            string newDigitText = "";
            foreach (char c in digitText)
            {
                if ((c >= '0' && c <= '9') || c =='.')
                    newDigitText += c.ToString();
            }
            return Convert.ToDouble(newDigitText, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string HashString(string text)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string Trim(string text)
        {
            return text.Replace(" ", "");
        }

        public static string FormatNumber(int num)
        {
            return num.ToString("N0", new System.Globalization.CultureInfo("en-US"));
        }
        #endregion

        #region Membership related:
        public static bool IsValidUserName(string userName)
        {
            return true;
        }

        public static bool IsValidPassword(string password)
        {
            Regex rgx_password = new Regex(Patterns.Password);
            return rgx_password.IsMatch(password);
        }

        public static string CurrentUserName
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        public static bool UserNameExists(string userName)
        {
            return Membership.GetUser(userName) != null;
        }

        public static bool IsValidRegisterationCode(string code)
        {
            Regex rgx_regCode = new Regex(Patterns.RegisterationCode);
            return rgx_regCode.IsMatch(code);
        }
        #endregion

        #region Image related:
        public static bool IsSupportedPhotoExtension(string Ext)
        {
            return Configs.SupportedPhotoFormats.Contains(Ext.ToLower());
        }

        public static System.Drawing.Bitmap GetThumbnailImage(System.Drawing.Image image, int Thumb_Width, int Thumb_Height)
        {
            Bitmap bmp = new Bitmap(Thumb_Width, Thumb_Height);
            bmp.SetResolution(72, 72);
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, Thumb_Width, Thumb_Height);
            gr.DrawImage(image, rectDestination, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            return bmp;
        }

        public static Image ResizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public static System.Drawing.Bitmap GetRectangleFromBitmapImage(System.Drawing.Bitmap image, int width, int height)
        {
            Rectangle rect = new Rectangle(0, 0, width, height);
            var img = image.Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);
            img.SetResolution(72, 72);
            return img;
        }

        public static System.Drawing.Bitmap GetRectangleFromBitmapImage(System.Drawing.Bitmap image, int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            var img = image.Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);
            img.SetResolution(72, 72);
            return img;
        }

        public static string GetExtension(string FileName)
        {
            return FileName.Substring(FileName.LastIndexOf('.') + 1);
        }
        #endregion

        #region Other methods:
        public static Control FindControl(ControlCollection collection, Type type)
        {
            Control control = null;
            foreach (Control c in collection)
            {
                if (c.GetType() == type)
                    return c;
            }
            return control;
        }

        public static int GetIndexOf(System.Web.UI.WebControls.DropDownList combo, string val)
        {
            int index = -1;
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i].Value == val)
                {
                    index = i;
                    return i;
                }
            }
            return index;
        }

        public static void MessageBoxShow(string MessageText, System.Web.UI.Control ctrl)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(ctrl, typeof(Control), "click", @"alert('" + MessageText + "');", true);
        }

        public static void MessageBoxShow(string MessageText, object ctrl)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript((System.Web.UI.Control)ctrl, typeof(Control), "click", @"alert('" + MessageText + "');", true);
        }

        public static string GetPropertyValue(object o, string p)
        {
            var prop = o.GetType().GetProperties().FirstOrDefault(pr => pr.Name == p);

            if (prop == null)
                throw new ArgumentException(String.Format("object does not have an {0} property", p), "o");

            return prop.GetValue(o, new object[] { }).ToString();
        }
        #endregion
    }
}