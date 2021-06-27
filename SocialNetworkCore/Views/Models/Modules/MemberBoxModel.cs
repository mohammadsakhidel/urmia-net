using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using System.Web.Mvc;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class MemberBoxModel
    {
        private const string _member_seperator = ";";
        private const string _field_seperator = "#";
        //*************************************
        #region Public Properties:
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public int Width { get; set; }
        public IEnumerable<Member> SelectedMembers { get; set; }
        public string SelectedIdentifier { get; set; } //thumb or full
        public string KeyProperty { get; set; }
        public string SelectedMemberKeys
        {
            get
            {
                if (SelectedMembers != null && SelectedMembers.Any())
                {
                    string res = "";
                    var members = SelectedMembers.ToList();
                    for (var i = 0; i < members.Count; i++)
                    {
                        var keyProp = (!string.IsNullOrEmpty(KeyProperty) ? KeyProperty : "Email");
                        var keyPropVal = MyHelper.GetPropertyValue(members[i], keyProp);
                        res += (i == 0 ? keyPropVal : _member_seperator + keyPropVal);
                    }
                    return res;
                }
                else
                {
                    return "";
                }
            }
        }
        public string SelectedMemberInfos
        {
            get
            {
                if (SelectedMembers != null && SelectedMembers.Any())
                {
                    string res = "";
                    var members = SelectedMembers.ToList();
                    for (var i = 0; i < members.Count; i++)
                    {
                        var member = members[i];
                        string info = member.Email + _field_seperator + member.FullName + _field_seperator + MyHelper.ToAbsolutePath(member.UrlOfThumb) + _field_seperator + MyHelper.ToAbsolutePath(member.UrlOfProfilePage);
                        res += (i == 0 ? info : _member_seperator + info);
                    }
                    return res;
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region Methods:
        public static MemberBoxModel Create(string name, string actionName, string controllerName, 
            int? width, List<Member> selectedMembers, string selectedIdentifierType, 
            string keyProperty, System.Data.Objects.ObjectContext context)
        {
            var model = new MemberBoxModel();
            model.Name = name;
            model.ActionName = actionName;
            model.ControllerName = controllerName;
            model.Width = (width.HasValue && width.Value > 0 ? width.Value : 300);
            model.SelectedMembers = selectedMembers != null ? selectedMembers : new List<Member>();
            model.SelectedIdentifier = (!String.IsNullOrEmpty(selectedIdentifierType) && (new string[] { "thumb", "full" }).Contains(selectedIdentifierType) ? selectedIdentifierType : "full");
            model.KeyProperty = keyProperty;
            return model;
        }
        #endregion
    }
}