using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class LikesDialogModel
    {
        public IEnumerable<Like> Likes { get; set; }
        public List<UserIdentityModel> LikerModels { get; set; }

        public static LikesDialogModel Create(IEnumerable<Like> likes, System.Data.Objects.ObjectContext context)
        {
            var model = new LikesDialogModel();
            model.Likes = likes;
            model.LikerModels = model.Likes.Select(l => UserIdentityModel.Create(l.Member, 45, UserIdentityType.ThumbAndFullName, "", "liker_fullname", "", "", context)).ToList();
            return model;
        }
    }
}