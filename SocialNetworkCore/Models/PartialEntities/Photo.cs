using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class Photo
    {
        public string Url
        {
            get
            {
                string url = "";
                if (this is AlbumPhoto)
                {
                    url = Urls.AlbumPhotos + this.FileName;
                }
                else if (this is PostPhoto)
                {
                    url = Urls.PostPhotos + this.FileName;
                }
                else if (this is ProfileCoverPhoto)
                {
                    url = Urls.ProfileCovers + this.FileName;
                }
                return url;
            }
        }

        public string AbsolutePath
        {
            get
            {
                var path = MyHelper.ToAbsolutePath(this.Url);
                return path;
            }
        }
    }
}