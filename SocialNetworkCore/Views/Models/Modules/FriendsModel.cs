using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class FriendsModel
    {
        #region Properties:
        public List<Tuple<Member, UserIdentityModel>> Friendships { get; set; }
        #endregion

        #region Methods:
        public static FriendsModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            var friends = mr.FindFriends(member.Email);
            var friendships = new List<Tuple<Member, UserIdentityModel>>();
            foreach (var f in friends)
            {
                var uidModel = UserIdentityModel.Create(f, 50, UserIdentityType.ThumbAndFullName, "friend_user_id", "friend_fullname", "", "", context);
                var tuple = new Tuple<Member, UserIdentityModel>(f, uidModel);
                friendships.Add(tuple);
            }
            //////////////////////////////////
            var model = new FriendsModel();
            model.Friendships = friendships;
            return model;
        }
        #endregion
    }
}