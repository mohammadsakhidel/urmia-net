using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class BasicInfoModel
    {
        #region Properties:
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateInfo BirthDateInfo { get; set; }
        public bool Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Fashion { get; set; }
        public string Behaviour { get; set; }
        public string AboutMe { get; set; }
        public int LivingRegion { get; set; }
        public string LivingCity { get; set; }
        #endregion

        #region statics:
        public static BasicInfoModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var hasBasicInfo = member.BasicInformation != null;
            BasicInformation bi = hasBasicInfo ? member.BasicInformation : null;
            ///////////////////////////////////////////////////
            var biModel = new BasicInfoModel();
            biModel.Name = member.Name;
            biModel.LastName = member.LastName;
            biModel.BirthDateInfo = DateHelper.GetDateInfo(member.BirthDay.Value);
            biModel.Gender = member.Gender.HasValue ? member.Gender.Value : true;
            biModel.MaritalStatus = (hasBasicInfo && bi.MaritalStatus.HasValue ? (MaritalStatus)bi.MaritalStatus : MaritalStatus.None);
            biModel.Height = (hasBasicInfo && bi.Height.HasValue ? bi.Height.Value : -1);
            biModel.Height = (hasBasicInfo && bi.Height.HasValue ? bi.Height.Value : -1);
            biModel.Weight = (hasBasicInfo && bi.Weight.HasValue ? bi.Weight.Value : -1);
            biModel.Fashion = (hasBasicInfo && !String.IsNullOrEmpty(bi.Fashion) ? bi.Fashion : "0000000");
            biModel.Behaviour = (hasBasicInfo && !String.IsNullOrEmpty(bi.Behavior) ? bi.Behavior : "");
            biModel.AboutMe = (hasBasicInfo && !String.IsNullOrEmpty(bi.AboutMe) ? bi.AboutMe : "");
            biModel.LivingRegion = (hasBasicInfo && bi.LivingRegion.HasValue ? bi.LivingRegion.Value : -1);
            biModel.LivingCity = (hasBasicInfo && !String.IsNullOrEmpty(bi.LivingCity) ? bi.LivingCity : "");
            return biModel;
        }
        #endregion
    }
}