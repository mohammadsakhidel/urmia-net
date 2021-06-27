using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models;

namespace SocialNetApp.Views.Models.Modules
{
    public class FavoritesModel
    {
        #region Properties:
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Behaviour { get; set; }
        public string Fashion { get; set; }
        public string Hobbies { get; set; }
        public string FavoriteMusics { get; set; }
        public string FavoriteMovies { get; set; }
        public string FavoriteBooks { get; set; }
        public string FavoriteQuotes { get; set; }
        #endregion

        #region Methods:
        public static FavoritesModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var fav = member.Favorite;
            var hasFav = fav != null;
            ////////////////////////////////////
            var favModel = new FavoritesModel();
            favModel.AgeFrom = hasFav && fav.AgeFrom.HasValue ? fav.AgeFrom.Value : -1;
            favModel.AgeTo = hasFav && fav.AgeTo.HasValue ? fav.AgeTo.Value : -1;
            favModel.MaritalStatus = hasFav && fav.MaritalStatus.HasValue ? (MaritalStatus)fav.MaritalStatus.Value : MaritalStatus.None;
            favModel.Height = hasFav && fav.Height.HasValue ? fav.Height.Value : -1;
            favModel.Weight = hasFav && fav.Weight.HasValue ? fav.Weight.Value : -1;
            favModel.Behaviour = hasFav && !String.IsNullOrEmpty(fav.Behavior) ? fav.Behavior : "";
            favModel.Fashion = hasFav && !String.IsNullOrEmpty(fav.Fashion) ? fav.Fashion : "0000000";
            favModel.Hobbies = hasFav && !String.IsNullOrEmpty(fav.Hobbies) ? fav.Hobbies : "";
            favModel.FavoriteMusics = hasFav && !String.IsNullOrEmpty(fav.FavoriteMusics) ? fav.FavoriteMusics : "";
            favModel.FavoriteMovies = hasFav && !String.IsNullOrEmpty(fav.FavoriteMovies) ? fav.FavoriteMovies : "";
            favModel.FavoriteBooks = hasFav && !String.IsNullOrEmpty(fav.FavoriteBooks) ? fav.FavoriteBooks : "";
            favModel.FavoriteQuotes = hasFav && !String.IsNullOrEmpty(fav.FavoriteQuotes) ? fav.FavoriteQuotes : "";
            return favModel;
        }
        #endregion
    }
}