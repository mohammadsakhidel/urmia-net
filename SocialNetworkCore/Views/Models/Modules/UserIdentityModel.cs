using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class UserIdentityModel
    {
        #region Properties:
        public Member Member { get; set; }
        public int Sides { get; set; }
        public UserIdentityType Type { get; set; }
        public string PositioningCssClass { get; set; }
        public string FullNamePositioningCssClass { get; set; }
        public string ThumbLinkPositioningCssClass { get; set; }
        public string ThumbImageCssClass { get; set; }
        #endregion

        #region Methods:
        public static UserIdentityModel Create(Member member, int? sides, UserIdentityType? uidType, 
            string posCssClass, string namePosCssClass, string thumbContainerLinkPosCssClass, string thumbImagePosCssClass, System.Data.Objects.ObjectContext context)
        {
            var model = new UserIdentityModel();
            model.Member = member;
            model.Sides = sides.HasValue && sides.Value > 0 ? sides.Value : 40;
            model.Type = uidType.HasValue ? uidType.Value : UserIdentityType.ThumbAndFullName;
            model.PositioningCssClass = posCssClass;
            model.FullNamePositioningCssClass = namePosCssClass;
            model.ThumbLinkPositioningCssClass = thumbContainerLinkPosCssClass;
            model.ThumbImageCssClass = thumbImagePosCssClass;
            return model;
        }
        #endregion
    }
}