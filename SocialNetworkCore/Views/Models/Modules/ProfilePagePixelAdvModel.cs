using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfilePagePixelAdvModel
    {
        public List<PixelAdvertisement> Ads { get; set; }

        public static ProfilePagePixelAdvModel Create(System.Data.Objects.ObjectContext context)
        {
            var ar = new AdvRepository(context);
            //************************
            var model = new ProfilePagePixelAdvModel();
            model.Ads = ar.GetProfileAds().ToList();
            return model;
        }
    }
}