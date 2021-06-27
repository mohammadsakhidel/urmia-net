using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class MemberBriefModel
    {
        public int Index { get; set; }
        public int ContainerCols { get; set; }
        public Member Member { get; set; }
        public int RowIndex { get; set; }
        public string RowStyle { get; set; }
        public bool IsMemberOnline { get; set; }
        public string GenderText { get; set; }
        //****** modules:
        public UserIdentityModel ThumbUserIdentityModel { get; set; }
        public UserIdentityModel NameUserIdentityModel { get; set; }
        //******

        public static MemberBriefModel Create(Member member, int index, int containerColumns, System.Data.Objects.ObjectContext context)
        {
            var model = new MemberBriefModel();
            model.Member = member;
            model.Index = index;
            model.ContainerCols = containerColumns;
            model.RowIndex = (int)(model.Index / model.ContainerCols);
            model.RowStyle = (model.RowIndex % 2 == 0 ? "even" : "odd");
            model.IsMemberOnline = OnlinesHelper.IsOnline(model.Member.Email);
            model.GenderText = model.Member.Gender.HasValue && !model.Member.Gender.Value ? Resources.Words.Female : Resources.Words.Male;
            //****** modules:
            model.ThumbUserIdentityModel = UserIdentityModel.Create(model.Member, 45, UserIdentityType.Thumb, "", "", "", "", context);
            model.NameUserIdentityModel = UserIdentityModel.Create(model.Member, null, UserIdentityType.FullName, "", "", "", "", context);
            //******
            return model;
        }
    }
}