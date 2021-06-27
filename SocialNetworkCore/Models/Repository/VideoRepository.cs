using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class VideoRepository
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public VideoRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public VideoRepository(System.Data.Objects.ObjectContext context)
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
        //***************************************************************************
        public Video Get(int id)
        {
            return db.SharedObjects.OfType<Video>().SingleOrDefault(o => o.Id == id);
        }

        public void Save()
        {
            db.SaveChanges();
        }
        #endregion
    }
}