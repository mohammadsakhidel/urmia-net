using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public partial class Notification
    {
        public string CreateTimeText
        {
            get
            {
                string txt = CoreHelpers.DateHelper.DateTimeToText(this.CreateTime);
                return txt;
            }
        }
    }
}