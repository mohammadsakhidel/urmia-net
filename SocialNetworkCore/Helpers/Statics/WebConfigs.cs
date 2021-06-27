using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace CoreHelpers
{
    public class WebConfigs
    {
        public static string SocialNetDbConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SocialNetDbConnection"].ConnectionString;
            }
        }

        public static string EntitiesConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SocialNetDbEntities"].ConnectionString;
            }
        }
    }
}