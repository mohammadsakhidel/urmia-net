using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class HmiRequestsModel
    {
        #region Properties:
        public List<Tuple<FriendshipRequest, Member, UserIdentityModel, UserIdentityModel>> NewFriendshipRequests { get; set; }
        #endregion

        #region Methods:
        public static HmiRequestsModel Create(System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            var new_requests = mr.FindNewFriendshipRequest(HttpContext.Current.User.Identity.Name);
            var new_request_senders = mr.Get(new_requests.Select(r => r.From)).ToList();
            var new_friendship_requests_records = new List<Tuple<FriendshipRequest, Member, UserIdentityModel, UserIdentityModel>>();
            foreach(var req in new_requests)
            {
                var req_sender = new_request_senders.Where(s => s.Email == req.From).FirstOrDefault();
                var thumbUidModel = UserIdentityModel.Create(req_sender, 40, UserIdentityType.Thumb, "", "", "", "", context);
                var nameUidModel = UserIdentityModel.Create(req_sender, null, UserIdentityType.FullName, "", "", "", "", context);
                var tuple = new Tuple<FriendshipRequest, Member, UserIdentityModel, UserIdentityModel>(req, req_sender, thumbUidModel, nameUidModel);
                new_friendship_requests_records.Add(tuple);
            }
            //**************************************
            var model = new HmiRequestsModel();
            model.NewFriendshipRequests = new_friendship_requests_records;
            return model;
        }
        #endregion
    }
}