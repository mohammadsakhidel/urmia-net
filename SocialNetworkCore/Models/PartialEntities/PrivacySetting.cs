using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using SocialNetApp.Models.Repository;

namespace SocialNetApp.Models
{
    public partial class PrivacySetting
    {
        #region Statics:
        public static PrivacySetting GetModelFromCollection(System.Web.Mvc.FormCollection form)
        {
            var ps = new PrivacySetting();
            ps.MemberId = HttpContext.Current.User.Identity.Name;
            ps.WhoSeeLikeActivities = Convert.ToByte(form["WhoSeeLikeActivities"]);
            ps.WhoSeeCommentActivities = Convert.ToByte(form["WhoSeeCommentActivities"]);
            ps.WhoSeeFriendshipActivities = Convert.ToByte(form["WhoSeeFriendshipActivities"]);
            ps.WhoSeeChangeInfoActivities = Convert.ToByte(form["WhoSeeChangeInfoActivities"]);
            ps.WhoSeeEducationActivities = Convert.ToByte(form["WhoSeeEducationActivities"]);
            ps.WhoSeeSkillActivities = Convert.ToByte(form["WhoSeeSkillActivities"]);
            ps.WhoSeeChangeCoverActivities = Convert.ToByte(form["WhoSeeChangeCoverActivities"]);
            ps.WhoCanWriteOnWall = Convert.ToByte(form["WhoCanWriteOnWall"]);
            ps.WhoCanSendMessage = Convert.ToByte(form["WhoCanSendMessage"]);
            ps.WhoCanBeginChat = Convert.ToByte(form["WhoCanBeginChat"]);
            ps.AllowSendFriendshipRequest = (form["AllowSendFriendshipRequest"] != null && form["AllowSendFriendshipRequest"] == "on" ? true : false);
            ps.WhoCanSeeFriends = Convert.ToByte(form["WhoCanSeeFriends"]);
            ps.WhoCanSeeBasicInformation = Convert.ToByte(form["WhoCanSeeBasicInformation"]);
            ps.WhoCanSeeFavorites = Convert.ToByte(form["WhoCanSeeFavorites"]);
            ps.DisplayInSearch = (form["DisplayInSearch"] != null && form["DisplayInSearch"] == "on" ? true : false);
            ps.BlockedUsers = form["BlockedUsers"];
            ps.MembersCanFollowMe = (form["MembersCanFollowMe"] != null && form["MembersCanFollowMe"] == "on" ? true : false);
            return ps;
        }

        public static PrivacySetting Default
        {
            get
            {
                var ps = new PrivacySetting();
                ps.WhoSeeLikeActivities = (byte)VisibleTo.Members;
                ps.WhoSeeCommentActivities = (byte)VisibleTo.Members;
                ps.WhoSeeFriendshipActivities = (byte)VisibleTo.Members;
                ps.WhoSeeChangeInfoActivities = (byte)VisibleTo.Members;
                ps.WhoSeeChangeCoverActivities = (byte)VisibleTo.Members;
                ps.WhoSeeEducationActivities = (byte)VisibleTo.Members;
                ps.WhoSeeSkillActivities = (byte)VisibleTo.Members;
                ps.WhoCanWriteOnWall = (byte)VisibleTo.Members;
                ps.WhoCanSendMessage = (byte)VisibleTo.Members;
                ps.WhoCanBeginChat = (byte)VisibleTo.Members;
                ps.AllowSendFriendshipRequest = true;
                ps.WhoCanSeeFriends = (byte)VisibleTo.Members;
                ps.WhoCanSeeBasicInformation = (byte)VisibleTo.EveryOne;
                ps.WhoCanSeeFavorites = (byte)VisibleTo.EveryOne;
                ps.DisplayInSearch = true;
                ps.BlockedUsers = "";
                ps.MembersCanFollowMe = true;
                return ps;
            }
        }
        #endregion

        #region Properties:
        public IEnumerable<Member> BlockedMembers(System.Data.Objects.ObjectContext context)
        {
            if (!String.IsNullOrEmpty(BlockedUsers))
            {
                var mr = new MembersRepository(context);
                return mr.Get(MyHelper.SplitString(BlockedUsers, ";"));
            }
            else
            {
                return new List<Member>();
            }
        }

        public IEnumerable<string> BlockedUserNames
        {
            get
            {
                if (!String.IsNullOrEmpty(BlockedUsers))
                {
                    return MyHelper.SplitString(BlockedUsers, ";");
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        public bool IsBlocked(string email)
        {
            var res = false;
            if (BlockedUserNames.Contains(email))
                res = true;
            return res;
        }
        #endregion

        #region Validation:
        private ValidationErrorList _validationErrors = new ValidationErrorList();
        private bool _isValidated = false;
        public ValidationErrorList ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
            set
            {
                _validationErrors = value;
            }
        }
        public bool IsValid
        {
            get
            {
                if (!_isValidated)
                    Validate();
                return ValidationErrors.Count() == 0;
            }
        }
        public void Validate()
        {
            //Text:
            if (!String.IsNullOrEmpty(BlockedUsers))
            {
                if (BlockedUsers.Length > 1000)
                    ValidationErrors.Add("BlockedUsers", Resources.Messages.StringLengthGeneral);
            }

            _isValidated = true;
        }
        #endregion
    }
}