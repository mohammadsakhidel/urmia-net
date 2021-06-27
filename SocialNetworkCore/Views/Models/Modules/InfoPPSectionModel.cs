using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class InfoPPSectionModel
    {
        #region Properties:
        public Member ProfileOwner { get; set; }
        public Member Viewer { get; set; }
        public string ViewerUserName { get; set; }
        public BasicInformation OwnerBasicInformation { get; set; }
        public bool HasBasicInfo { get; set; }
        public Education OwnerHighestEducation { get; set; }
        public bool IsBasicInfoVisibleToUser { get; set; }
        public bool IsEducationVisibleToUser { get; set; }
        #endregion

        #region Methods:
        public static InfoPPSectionModel Create(Member profileOwner, Member viewer, System.Data.Objects.ObjectContext context)
        {
            var model = new InfoPPSectionModel();
            model.ProfileOwner = profileOwner;
            model.Viewer = viewer;
            model.ViewerUserName = (model.Viewer != null ? model.Viewer.Email : "");
            model.IsBasicInfoVisibleToUser = model.ProfileOwner.IsBasicInfoVisibleTo(model.ViewerUserName, context);
            if (model.IsBasicInfoVisibleToUser)
            {
                model.HasBasicInfo = model.ProfileOwner.BasicInformation != null;
                model.OwnerBasicInformation = model.HasBasicInfo ? model.ProfileOwner.BasicInformation : null;
                model.OwnerHighestEducation = model.ProfileOwner.MaxEducation;
                model.IsEducationVisibleToUser = model.ProfileOwner.IsEducationsVisibleTo(model.ViewerUserName, context) && model.OwnerHighestEducation != null;
            }
            return model;
        }

        #endregion
    }
}