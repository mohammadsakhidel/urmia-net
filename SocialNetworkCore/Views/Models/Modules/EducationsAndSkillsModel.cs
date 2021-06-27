using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class EducationsAndSkillsModel
    {
        #region Properties:
        public List<Education> Educations { get; set; }
        public List<Skill> Skills { get; set; }
        public Dictionary<int, string> EducationLevels { get; set; }
        #endregion

        #region Methods:
        public static EducationsAndSkillsModel Create(Member member, System.Data.Objects.ObjectContext context)
        {
            var model = new EducationsAndSkillsModel();
            model.EducationLevels = Dictionaries.EducationLevels;
            model.Educations = member.Educations.ToList();
            model.Skills = member.Skills.ToList();
            return model;
        }
        #endregion
    }
}