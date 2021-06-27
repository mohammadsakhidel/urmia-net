using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class Defaults
    {
        public static string UrlForMaleProfileImage
        {
            get
            {
                return Statics.GetValue("defaultUrls", "UrlForMaleProfileImage");
            }
        }
        public static string UrlForFemaleProfileImage
        {
            get
            {
                return Statics.GetValue("defaultUrls", "UrlForFemaleProfileImage");
            }
        }
        public static string UrlForAlbumCover
        {
            get
            {
                return Statics.GetValue("defaultUrls", "UrlForAlbumCover");
            }
        }
        public static string UrlForMaleThumb
        {
            get
            {
                return Statics.GetValue("defaultUrls", "UrlForMaleThumb");
            }
        }
        public static string UrlForFemaleThumb
        {
            get
            {
                return Statics.GetValue("defaultUrls", "UrlForFemaleThumb");
            }
        }
        public static string UrlForDeletedAccountThumb
        {
            get
            {
                return Statics.GetValue("defaultUrls", "UrlForDeletedAccountThumb");
            }
        }
    }
}