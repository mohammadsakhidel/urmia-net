using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CoreHelpers
{
    public class Statics
    {
        public static string GetValue(string section, string name)
        {
            var val = "";
            var url = "~/Content/ConfigFiles/statics.xml";
            var doc = XDocument.Load(HttpContext.Current.Server.MapPath(url));
            var root = doc.Root;
            var sectionElement = root.Elements(section);
            val = sectionElement.Any() && sectionElement.Elements(name).Any() ?
                sectionElement.First().Elements(name).First().Value :
                "";
            return val;
        }
    }
}