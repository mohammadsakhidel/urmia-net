using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class PagesRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public PagesRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public PagesRepository(System.Data.Objects.ObjectContext context)
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
        #region Select
        public IEnumerable<Page> GetAll()
        {
            return db.Pages.OrderBy(p => p.DateOfAdd);
        }
        public Page Get(string name)
        {
            try
            {
                return db.Pages.Single(t => t.Name == name);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Delete
        public void Delete(string pageName)
        {
            db.Pages.DeleteObject(Get(pageName));
            Save();
        }
        #endregion

        #region Insert
        public void Insert(Page pg)
        {
            db.Pages.AddObject(pg);
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