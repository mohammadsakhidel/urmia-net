using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CoreHelpers
{
    public class Configs
    {
        #region kurdi3tan.com configuration:
        public static string DefaultTheme
        {
            get
            {
                return GetValue("core", "DefaultTheme");
            }
        }
        public static string DefaultLanguage
        {
            get
            {
                var def = SupportedLanguages.FirstOrDefault(l => l.IsDefault);
                return def.Code;
            }
        }
        public static DateType DateType
        {
            get
            {
                var dtType = GetValue("core", "DateType");
                return (DateType)Enum.Parse(typeof(DateType), dtType);
            }
        }
        public static string DomainName
        {
            get
            {
                return GetValue("core", "DomainName");
            }
        }
        public static string RefreshOnlinesSecurityKey
        {
            get
            {
                return GetValue("core", "RefreshOnlinesSecurityKey");
            }
        }
        public static int LowLoadBeginHour
        {
            get
            {
                return Convert.ToInt32(GetValue("core", "LowLoadBeginHour"));
            }
        }
        public static int LowLoadEndHour
        {
            get
            {
                return Convert.ToInt32(GetValue("core", "LowLoadEndHour"));
            }
        }
        public static string SocialNetworksAccount
        {
            get
            {
                return GetValue("core", "SocialNetworksAccount");
            }
        }
        public static List<string> SupportedVideoFormats
        {
            get
            {
                return GetValues("SupportedVideoFormat");
            }
        }
        public static List<string> SupportedPhotoFormats
        {
            get
            {
                return GetValues("SupportedPhotoFormat");
            }
        }
        public static IEnumerable<SupportedLanguage> SupportedLanguages
        {
            get
            {
                var els = GetElements("SupportedLanguage").Select(el => new SupportedLanguage { 
                    Code = el.Value,
                    DisplayName = el.Attribute("DisplayName").Value,
                    IsDefault = el.Attribute("IsDefault") != null ? Convert.ToBoolean(el.Attribute("IsDefault").Value) : false
                });
                return els;
            }
        }
        public static string ConvertedVideosFormat
        {
            get
            {
                return GetValue("core", "ConvertedVideosFormat");
            }
        }
        public static string SocialNetworkName
        {
            get
            {
                return GetValue("core", "SocialNetworkName");
            }
        }
        #endregion

        public static string GetValue(string section, string name)
        {
            var val = "";
            var url = "~/Content/ConfigFiles/configs.xml";
            var doc = XDocument.Load(HttpContext.Current.Server.MapPath(url));
            var root = doc.Root;
            var sectionElement = root.Elements(section);
            val = sectionElement.Any() && sectionElement.Elements(name).Any() ?
                sectionElement.First().Elements(name).First().Value :
                "";
            return val;
        }

        public static List<string> GetValues(string name)
        {
            return GetElements(name).Select(el => el.Value).ToList();
        }

        public static IEnumerable<XElement> GetElements(string name)
        {
            var url = "~/Content/ConfigFiles/configs.xml";
            var doc = XDocument.Load(HttpContext.Current.Server.MapPath(url));
            var root = doc.Root;
            return root.Descendants(name);
        }
    }
    public class EmailAccount
    {
        public static string Provider
        {
            get
            {
                return Configs.GetValue("emailAccount", "Provider");
            }
        }
        public static string Email
        {
            get
            {
                return Configs.GetValue("emailAccount", "Email");
            }
        }
        public static string Password
        {
            get
            {
                return Configs.GetValue("emailAccount", "Password");
            }
        }
        public static string ProviderIP
        {
            get
            {
                return Configs.GetValue("emailAccount", "ProviderIP");
            }
        }
    }
}