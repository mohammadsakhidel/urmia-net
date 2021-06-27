using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class HomePagePixelAdvModel
    {
        #region Properties:
        public List<PixelAdvertisement> Ads { get; set; }
        #endregion

        #region Methods:
        public static HomePagePixelAdvModel Create(System.Data.Objects.ObjectContext context)
        {
            var ar = new AdvRepository(context);
            var model = new HomePagePixelAdvModel();
            model.Ads = ar.GetHomePageAds().ToList();
            return model;
        }
        #endregion
    }
}