using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class Urls
    {
        public static string ModuleViews
        {
            get
            {
                return Statics.GetValue("urls", "ModuleViews");
            }
        }
        public static string AlbumPhotos
        {
            get
            {
                return Statics.GetValue("urls", "AlbumPhotos");
            }
        }
        public static string AlbumLargeThumbnails
        {
            get
            {
                return Statics.GetValue("urls", "AlbumLargeThumbnails");
            }
        }
        public static string AlbumSmallThumbnails
        {
            get
            {
                return Statics.GetValue("urls", "AlbumSmallThumbnails");
            }
        }
        public static string ProfileCovers
        {
            get
            {
                return Statics.GetValue("urls", "ProfileCovers");
            }
        }
        public static string ProfileCoverBases
        {
            get
            {
                return Statics.GetValue("urls", "ProfileCoverBases");
            }
        }
        public static string PostPhotos
        {
            get
            {
                return Statics.GetValue("urls", "PostPhotos");
            }
        }
        public static string PostVideos
        {
            get
            {
                return Statics.GetValue("urls", "PostVideos");
            }
        }
        public static string PostLargeThumbnails
        {
            get
            {
                return Statics.GetValue("urls", "PostLargeThumbnails");
            }
        }
        public static string PostSmallThumbnails
        {
            get
            {
                return Statics.GetValue("urls", "PostSmallThumbnails");
            }
        }
        public static string Emoticons
        {
            get
            {
                return Statics.GetValue("urls", "Emoticons");
            }
        }
        public static string SiteImages
        {
            get
            {
                return Statics.GetValue("urls", "SiteImages");
            }
        }
        public static string PixelAdvertisements
        {
            get
            {
                return Statics.GetValue("urls", "PixelAdvertisements");
            }
        }
        public static string TasksLogFile
        {
            get
            {
                return Statics.GetValue("urls", "TasksLogFile");
            }
        }
        public static string TempFolder
        {
            get
            {
                return Statics.GetValue("urls", "TempFolder");
            }
        }
        public static string VideoProcessor
        {
            get
            {
                return Statics.GetValue("urls", "VideoProcessor");
            }
        }
        public static string PostVideosActualSizeThumbs
        {
            get
            {
                return Statics.GetValue("urls", "PostVideosActualSizeThumbs");
            }
        }
        public static string PostVideosSmallThumbs
        {
            get
            {
                return Statics.GetValue("urls", "PostVideosSmallThumbs");
            }
        }
        public static string LogoNameIcon
        {
            get
            {
                return Statics.GetValue("urls", "LogoNameIcon");
            }
        }
    }
}