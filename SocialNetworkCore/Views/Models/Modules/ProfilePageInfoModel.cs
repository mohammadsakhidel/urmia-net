using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfilePageInfoModel
    {
        #region Properties:
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public bool IsBasicInfoVisibleToUser { get; set; }
        public BasicInformation BasicInformation { get; set; }
        public bool HasBasicInformation { get; set; }
        public bool IsFavoritesVisibleToUser { get; set; }
        public Favorite Favorites { get; set; }
        public bool HasFavorites { get; set; }
        public bool IsEducationsVisibleToUser { get; set; }
        public List<Education> Educations { get; set; }
        public bool HasEducation { get; set; }
        public bool IsSkillsVisibleToUser { get; set; }
        public List<Skill> Skills { get; set; }
        public bool HasSkill { get; set; }
        //********* modules:
        public AccessDeniedModel AccessDeniedModel { get; set; }
        //*********
        #endregion

        #region Methods:
        public static ProfilePageInfoModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var model = new ProfilePageInfoModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = (viewer != null ? viewer.Email : "");
            //-- visibility check:
            model.IsBasicInfoVisibleToUser = model.ProfileOwner.IsBasicInfoVisibleTo(model.ViewerUserName, context);
            model.IsFavoritesVisibleToUser = model.ProfileOwner.IsFavoritesVisibleTo(model.ViewerUserName, context);
            model.IsEducationsVisibleToUser = model.ProfileOwner.IsEducationsVisibleTo(model.ViewerUserName, context);
            model.IsSkillsVisibleToUser = model.ProfileOwner.IsSkillsVisibleTo(model.ViewerUserName, context);
            if (model.IsBasicInfoVisibleToUser || model.IsFavoritesVisibleToUser || model.IsEducationsVisibleToUser || model.IsSkillsVisibleToUser)
            {
                //-- basic information
                model.BasicInformation = model.IsBasicInfoVisibleToUser ? model.ProfileOwner.BasicInformation : null;
                model.HasBasicInformation = model.BasicInformation != null;
                //-- favorites:
                model.Favorites = model.IsFavoritesVisibleToUser ? model.ProfileOwner.Favorite : null;
                model.HasFavorites = model.Favorites != null;
                //-- educations:
                model.Educations = model.IsEducationsVisibleToUser ? model.ProfileOwner.Educations.OrderByDescending(e => e.EducationLevel).ToList() : null;
                model.HasEducation = model.Educations != null && model.Educations.Any();
                //-- skills:
                model.Skills = model.IsSkillsVisibleToUser ? model.ProfileOwner.Skills.ToList() : null;
                model.HasSkill = model.Skills != null && model.Skills.Any();
            }
            else
            {
                //********* modules:
                model.AccessDeniedModel = AccessDeniedModel.Create(context);
                //*********
            }
            return model;
        }
        #endregion
    }
}