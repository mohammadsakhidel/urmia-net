using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class HomePixelAdvModel
    {
        public List<PixelAdvertisement> Ads { get; set; }

        public static HomePixelAdvModel Create(System.Data.Objects.ObjectContext context)
        {
            var ar = new AdvRepository(context);
            var model = new HomePixelAdvModel();
            model.Ads = ar.GetHomeAds().ToList();
            return model;
        }
    }
}