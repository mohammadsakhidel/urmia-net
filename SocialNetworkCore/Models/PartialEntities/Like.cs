using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public partial class Like
    {
        public string DateOfLikeText
        {
            get
            {
                string txt = CoreHelpers.MyHelper.DateToText(this.DateOfLike);
                return txt;
            }
        }
    }
}