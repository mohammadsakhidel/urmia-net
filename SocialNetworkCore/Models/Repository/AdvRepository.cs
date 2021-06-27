using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class AdvRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public AdvRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public AdvRepository(System.Data.Objects.ObjectContext context)
        {
            db = context != null ? (SocialNetDbEntities)context : new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }
        #endregion

        #region Properties:
        public System.Data.Objects.ObjectContext Context
        {
            get
            {
                return db;
            }
        }
        #endregion
        //**********************************************************************************
        #region SELECT:
        public PixelAdvertisement Get(int id)
        {
            try
            {
                return db.PixelAdvertisements.Single(ad => ad.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<PixelAdvertisement> GetAll()
        {
            return db.PixelAdvertisements.OrderByDescending(ad => ad.DateOfAdd);
        }

        public IEnumerable<PixelAdvertisement> GetHomeAds()
        {
            var all = db.PixelAdvertisements.ToList().Where(ad => ad.Visible && ad.Places.Contains("home"));
            return all.Where(ad => MyHelper.Now >= ad.BeginDate.Value && (!ad.EndDate.HasValue || (ad.EndDate.HasValue && MyHelper.Now <= ad.EndDate.Value)));
        }

        public IEnumerable<PixelAdvertisement> GetProfileAds()
        {
            var all = db.PixelAdvertisements.ToList().Where(ad => ad.Visible && ad.Places.Contains("profile"));
            return all.Where(ad => MyHelper.Now >= ad.BeginDate.Value && (!ad.EndDate.HasValue || (ad.EndDate.HasValue && MyHelper.Now <= ad.EndDate.Value)));
        }

        public IEnumerable<PixelAdvertisement> GetHomePageAds()
        {
            var all = db.PixelAdvertisements.ToList().Where(ad => ad.Visible && ad.Places.Contains("homepage"));
            return all.Where(ad => MyHelper.Now >= ad.BeginDate.Value && (!ad.EndDate.HasValue || (ad.EndDate.HasValue && MyHelper.Now <= ad.EndDate.Value)));
        }
        #endregion

        #region INSERT:
        public void Insert(PixelAdvertisement adv)
        {
            db.PixelAdvertisements.AddObject(adv);
            Save();
        }
        #endregion

        #region UPDATE:
        #endregion

        #region DELETE:
        public void Delete(int pixelAdvId)
        {
            var adv = Get(pixelAdvId);
            // delete file:
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(Urls.PixelAdvertisements + adv.ImageFileName));
            // delete data:
            db.PixelAdvertisements.DeleteObject(adv);
            Save();
        }
        #endregion

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}