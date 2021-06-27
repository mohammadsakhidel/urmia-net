using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public partial class Sharing
    {
        public string GetUrlOfDetails(int def_photo)
        {
            return "~/ObjectDetails/" + this.Id + "?def=" + def_photo;
        }
    }
}