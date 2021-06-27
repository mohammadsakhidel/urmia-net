using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using System.ComponentModel.DataAnnotations;

namespace SocialNetApp.Models
{
    [MetadataType(typeof(AlbumMetadata))]
    public partial class Album
    {
        public string CoverThumbUrl
        {
            get
            {
                try
                {
                    var cover = this.Photos.Single(p => p.IsAlbumCover);
                    if (cover != null)
                        return Urls.AlbumLargeThumbnails + cover.FileName;
                    else
                        return Defaults.UrlForAlbumCover;
                }
                catch
                {
                    return Defaults.UrlForAlbumCover;
                }
            }
        }
    }

    public class AlbumMetadata
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredEmail")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "StringLengthEmail")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RegularExpressionEmail")]
        public object MemberId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredAlbumName")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "StringLengthAlbumName")]
        public object Name { get; set; }

        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "StringLengthAlbumDesc")]
        public object Description { get; set; }
    }
}