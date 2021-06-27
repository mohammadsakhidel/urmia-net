using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class PostPhoto
    {
        new public string Url
        {
            get
            {
                return Urls.PostPhotos + this.FileName;
            }
        }
        public string SmallThumbUrl
        {
            get
            {
                return Urls.PostSmallThumbnails + this.FileName;
            }
        }
        public string LargeThumbUrl
        {
            get
            {
                return Urls.PostLargeThumbnails + this.FileName;
            }
        }
        public string UrlOfPostDetails
        {
            get
            {
                return "~/ObjectDetails/" + this.PostId + "?def=" + this.Id;
            }
        }
    }
}