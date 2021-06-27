using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class PostVideo
    {
        public string UrlOfVideoFile
        {
            get
            {
                return Urls.PostVideos + this.FileName;
            }
        }

        public string UrlOfActualSizeThumbnail
        {
            get
            {
                return Urls.PostVideosActualSizeThumbs + string.Format("{0}.jpg", System.IO.Path.GetFileNameWithoutExtension(this.FileName));
            }
        }

        public string UrlOfSmallThumbnail
        {
            get
            {
                return Urls.PostVideosSmallThumbs + string.Format("{0}.jpg", System.IO.Path.GetFileNameWithoutExtension(this.FileName));
            }
        }
    }
}