using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Views.Models.Modules
{
    public class ProfileCoverModel
    {
        #region Properties:
        public bool IsPostBack { get; set; }
        public bool? UploadingDone { get; set; }
        public string UploadingErrorMessage { get; set; }
        public ProfileCoverPhoto PCP { get; set; }
        public SimpleUploaderModel SimpleUploaderModel { get; set; }
        #endregion

        #region Methods
        public static ProfileCoverModel Create(Member member, System.Web.Mvc.TempDataDictionary tempData, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            context = mr.Context;
            ///////////////////////////////////////////
            var pcModel = new ProfileCoverModel();
            pcModel.IsPostBack = tempData["done"] != null;
            pcModel.UploadingDone = pcModel.IsPostBack ? (bool)tempData["done"] : (bool?)null;
            pcModel.UploadingErrorMessage = pcModel.IsPostBack && tempData["error"] != null ? tempData["error"].ToString() : "";
            pcModel.PCP = mr.GetMemberProfileCoverPhoto(member.Email);
            pcModel.SimpleUploaderModel = SimpleUploaderModel.Create(context);
            return pcModel;
        }
        #endregion
    }
}