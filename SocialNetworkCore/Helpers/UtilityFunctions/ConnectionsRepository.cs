using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace CoreHelpers
{
    public class ConnectionsRepository : IDisposable
    {
        public SignalConnectionPool LoadPool(HttpContext context)
        {
            var pool = new SignalConnectionPool();
            if (context != null && context.Application["SignalConnectionPool"] != null)
            {
                pool = (SignalConnectionPool)context.Application["SignalConnectionPool"];
            }
            return pool;
        }

        public void SavePool(HttpContext context, SignalConnectionPool pool)
        {
            context.Application.Lock();
            context.Application["SignalConnectionPool"] = pool;
            context.Application.UnLock();
        }

        public SignalConnection Get(string connectionId)
        {
            try
            {
                var db = LoadPool(HttpContext.Current);
                return db[connectionId];
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<SignalConnection> FindConnections(string memberId)
        {
            var db = LoadPool(HttpContext.Current);
            var coll = db.Values.Where(con => con.MemberId == memberId);
            return coll;
        }

        public bool IsOnline(string memberId)
        {
            var db = LoadPool(HttpContext.Current);
            return db.Values.Where(con => con.MemberId == memberId).Any();
        }

        public void Insert(SignalConnection con)
        {
            var db = LoadPool(HttpContext.Current);
            db.Add(con.Id, con);
            SavePool(HttpContext.Current, db);
        }

        public void Delete(HttpContext context, string conId)
        {
            var db = LoadPool(context);
            db.Remove(conId);
            SavePool(context, db);
        }

        public void DeleteByMemberId(string memberId)
        {
            var db = LoadPool(HttpContext.Current);
            var connections = db.Values.Where(con => con.MemberId == memberId).ToList();
            for (var i = 0; i < connections.Count(); i++)
            {
                db.Remove(connections[i].Id);
            }
            SavePool(HttpContext.Current, db);
        }

        public void Refresh(HttpContext context)
        {
            var db = LoadPool(context);
            var allConnections = db.Values.ToList();
            for (var i = 0; i < allConnections.Count(); i++)
            {
                var c = allConnections[i];
                if (c.DateOfEstablish.AddMinutes(Digits.OnlineWindowMinutes) < MyHelper.Now)
                    db.Remove(c.Id);
            }
            SavePool(context, db);
        }

        public void Clear()
        {
            var db = LoadPool(HttpContext.Current);
            db.Clear();
            SavePool(HttpContext.Current, db);
        }

        public void Dispose()
        {
            //nothing to do
        }
    }
}