using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using CoreHelpers;
using SocialNetApp.Models.Repository;
using System.Globalization;

namespace SocialNetApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs();
            ClearActiveConnection();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //tasks:
            WebTaskScheduler.Add("RefreshOnlines", Tasks.RefreshOnlines, new TimeSpan(0, 20, 0));
            WebTaskScheduler.Add("RefreshChatSessions", Tasks.RefreshChatSessions, new TimeSpan(0, 60, 0));
            WebTaskScheduler.Add("ClearTempFolder", Tasks.ClearTempFolder, new TimeSpan(12, 0, 0));
            //cultures:
            //CreateCulture("sor", "Sorani", "fa-IR");
        }

        private void ClearActiveConnection()
        {
            var cr = new ConnectionsRepository();
            cr.Clear();
        }

        private static void CreateCulture(string identifier, string fullName, string baseIdentifier)
        {
            string culture = identifier;
            string name = fullName;
            CultureInfo cultureInfo = new CultureInfo(baseIdentifier);
            RegionInfo regionInfo = new RegionInfo(cultureInfo.Name);
            CultureAndRegionInfoBuilder cultureAndRegionInfoBuilder = new CultureAndRegionInfoBuilder(culture, CultureAndRegionModifiers.None);
            cultureAndRegionInfoBuilder.LoadDataFromCultureInfo(cultureInfo);
            cultureAndRegionInfoBuilder.LoadDataFromRegionInfo(regionInfo);
            cultureAndRegionInfoBuilder.CultureEnglishName = name;
            cultureAndRegionInfoBuilder.CultureNativeName = name;
            cultureAndRegionInfoBuilder.Register();
        }
    }
}