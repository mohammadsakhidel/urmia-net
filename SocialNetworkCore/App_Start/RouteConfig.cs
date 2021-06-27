using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialNetApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // Profile:
            routes.MapRoute(
                name: "Profile",
                url: "@{username}/{content}",
                defaults: new { controller = "Members", action = "ProfilePage", content = UrlParameter.Optional }
            );
            // photos:
            routes.MapRoute(
                name: "ObjectDetails",
                url: "ObjectDetails/{id}",
                defaults: new { controller = "Objects", action = "ObjectDetails" }
            );
            // pages:
            routes.MapRoute(
                name: "Pages",
                url: "Pages/{name}",
                defaults: new { controller = "Pages", action = "Page" }
            );
            // Go To Link:
            routes.MapRoute(
                name: "GoLink",
                url: "Go",
                defaults: new { controller = "Home", action = "Go" }
            );
            // Default:
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}