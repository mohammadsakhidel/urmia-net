using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CoreHelpers;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using SocialNetApp.Models.Repository;
using System.Web.Security;

namespace SocialNetApp.Models
{
    public partial class Member
    {
        #region Private Variables:
        private ValidationErrorList _validationErrors = new ValidationErrorList();
        private bool _isValidated = false;
        #endregion

        #region Properties:
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
        public string FullName
        {
            get
            {
                return Name + " " + LastName;
            }
        }
        public string YouOrFullName
        {
            get
            {
                return (this.Email == HttpContext.Current.User.Identity.Name ? Resources.Words.You : (this.StatusCode != (byte)MemberStatus.ScheduledForDelete ? this.FullName : Resources.Words.DeletedAccount));
            }
        }
        public ShamsiDateTime ShamsiBirthDay
        {
            get
            {
                return (BirthDay.HasValue ? ShamsiDateTime.MiladyToShamsi(this.BirthDay.Value) : null);
            }
        }
        public string UrlOfThumb
        {
            get
            {
                if (StatusCode == (byte)MemberStatus.ScheduledForDelete)
                    return Defaults.UrlForDeletedAccountThumb;
                else if (!String.IsNullOrEmpty(ProfilePhoto) && System.IO.File.Exists(HttpContext.Current.Server.MapPath(Urls.AlbumSmallThumbnails + ProfilePhoto)))
                    return Urls.AlbumSmallThumbnails + ProfilePhoto;
                else
                    return Gender.HasValue && !Gender.Value ? Defaults.UrlForFemaleThumb : Defaults.UrlForMaleThumb;
            }
        }
        public string UrlOfLargeThumb
        {
            get
            {
                if (!String.IsNullOrEmpty(ProfilePhoto) && System.IO.File.Exists(HttpContext.Current.Server.MapPath(Urls.AlbumSmallThumbnails + ProfilePhoto)))
                    return Urls.AlbumLargeThumbnails + ProfilePhoto;
                else
                    return Gender.HasValue && !Gender.Value ? Defaults.UrlForFemaleProfileImage : Defaults.UrlForMaleProfileImage;
            }
        }
        public string UrlOfProfilePage
        {
            get
            {
                return "~/@" + this.Alias;
            }
        }
        public string UrlOfCover(System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            ProfileCoverPhoto c = mr.GetMemberProfileCoverPhoto(this.Email);
            return (c != null ? Urls.ProfileCovers + c.FileName : "");
        }
        public IEnumerable<Member> Friends(System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            return mr.FindFriends(this.Email);
        }
        public IEnumerable<Conversation> Conversations(System.Data.Objects.ObjectContext context)
        {
            var mr = new MessagesRepository(context);
            return mr.FindConversations(this.Email);
        }
        public IEnumerable<Album> Albums
        {
            get
            {
                return this.SharedObjects.OfType<Album>();
            }
        }
        public string LastPasswordChangeDateText
        {
            get
            {
                var user = System.Web.Security.Membership.GetUser(this.Email);
                return MyHelper.DateToText(user.LastPasswordChangedDate);
            }
        }
        public MemberStatus Status
        {
            get
            {
                return (MemberStatus)this.StatusCode;
            }
        }
        public string StatusText
        {
            get
            {
                var stat = (MemberStatus)this.StatusCode;
                var isBlocked = this.IsBlocked;
                // is not approved?
                if (stat == MemberStatus.RegisteredNotActivated)
                    return Resources.Words.NotApproved;
                // is locked?
                if (Membership.GetUser(this.Email).IsLockedOut)
                    return Resources.Words.LockedOut;
                // is deleted?
                if (stat == MemberStatus.ScheduledForDelete)
                    return Resources.Words.ScheduledForDelete;
                // is blocked?
                if (isBlocked)
                    return Resources.Words.Blocked;
                // default:
                return Resources.Words.Active;
            }
        }
        public string InlineFullNameLink
        {
            get
            {
                return "<a href=\"" + MyHelper.ToAbsolutePath(this.UrlOfProfilePage) + "\">" + FullName + "</a>";
            }
        }
        public bool HasProfileCover(System.Data.Objects.ObjectContext context)
        {
            var url = this.UrlOfCover(context);
            return !String.IsNullOrEmpty(url) && System.IO.File.Exists(HttpContext.Current.Server.MapPath(url));
        }
        public int Age
        {
            get
            {
                if (this.BirthDay.HasValue)
                {
                    var total_months = (MyHelper.Now.Month - this.BirthDay.Value.Month) + 12 * (MyHelper.Now.Year - this.BirthDay.Value.Year);
                    return (int)(total_months / 12);
                }
                else
                {
                    return 0;
                }
            }
        }
        public Education MaxEducation
        {
            get
            {
                if (this.Educations.Any())
                {
                    return this.Educations.OrderByDescending(e => e.EducationLevel).Take(1).ToList().First();
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Object Methods:
        public void ChangeStatusCode(byte newStatusCode)
        {
            this.StatusCode = newStatusCode;
        }

        public DateTime GetLastActivationDate()
        {
            var usr = Membership.GetUser(this.Email);
            return usr.LastActivityDate;
        }
        public void Validate()
        {
            //Name:
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Name.Trim()))
                ValidationErrors.Add("Name", Resources.Messages.RequiredName);
            else if (Name.Length > 50)
                ValidationErrors.Add("Name", Resources.Messages.StringLengthName);

            //LastName:
            if (String.IsNullOrEmpty(LastName) || String.IsNullOrEmpty(LastName.Trim()))
                ValidationErrors.Add("LastName", Resources.Messages.RequiredLastName);
            else if (LastName.Length > 50)
                ValidationErrors.Add("LastName", Resources.Messages.StringLengthLastName);

            //FullName:
            Regex rgx_FullName = new Regex(Patterns.FullName);
            if (!rgx_FullName.IsMatch(FullName))
            {
                ValidationErrors.Add("FullName", Resources.Messages.RegularExpressionFullName);
            }

            //Email:
            if (String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Email.Trim()))
                ValidationErrors.Add("Email", Resources.Messages.RequiredEmail);
            else
            {
                if (Email.Length > 50)
                    ValidationErrors.Add("Email", Resources.Messages.StringLengthEmail);
                Regex rgx_email = new Regex(Patterns.Email);
                if (!rgx_email.IsMatch(Email))
                    ValidationErrors.Add("Email", Resources.Messages.RegularExpressionEmail);
            }

            //Alias:
            if (String.IsNullOrEmpty(Alias) || String.IsNullOrEmpty(Alias.Trim()))
                ValidationErrors.Add("Alias", Resources.Messages.RequiredAlias);
            else
            {
                if (Alias.Length > 20)
                    ValidationErrors.Add("Alias", Resources.Messages.StringLengthAlias);
                Regex rgx_alias = new Regex(Patterns.Alias);
                if (!rgx_alias.IsMatch(Alias))
                {
                    ValidationErrors.Add("Alias", Resources.Messages.RegularExpressionAlias);
                }
            }

            //BirthDay:
            if (!BirthDay.HasValue)
                ValidationErrors.Add("BirthDay", Resources.Messages.RequiredBirthDay);

            //Gender:
            if (!Gender.HasValue)
                ValidationErrors.Add("Gender", Resources.Messages.RequiredGender);

            //RegistrationCode:
            if (String.IsNullOrEmpty(RegistrationCode) || String.IsNullOrEmpty(RegistrationCode.Trim()))
                ValidationErrors.Add("RegistrationCode", Resources.Messages.RequiredRegistrationCode);
            else if (RegistrationCode.Length > 50)
                ValidationErrors.Add("RegistrationCode", Resources.Messages.StringLengthRegistrationCode);

            //RegistrationDate:
            if (RegistrationDate == null)
                ValidationErrors.Add("RegistrationDate", Resources.Messages.RequiredRegistrationDate);

            _isValidated = true;
        }
        #endregion

        #region Static Methods:
        public static Tuple<bool, ValidationErrorList, Member> Register(string firstName, string lastName,
            string email, string password, DateTime? birthday, bool? gender, byte statusCode, System.Data.Objects.ObjectContext context)
        {
            try
            {
                using (var mr = new MembersRepository(context))
                {
                    context = mr.Context;
                    // create member object:
                    var member = new Member();
                    member.Name = firstName;
                    member.LastName = lastName;
                    member.Email = email;
                    member.BirthDay = birthday;
                    member.Gender = gender;
                    member.RegistrationCode = MyHelper.GetRandomString(10, false);
                    member.RegistrationDate = MyHelper.Now;
                    member.LastEmailNotificationsDate = MyHelper.Now;
                    member.Alias = FindAlias(member.Email, member.BirthDay, context).Substring(0, 20);
                    member.StatusCode = statusCode;
                    member.ProfileVisitCounter = 0;
                    //-----
                    var ve = new ValidationErrorList();
                    if (member.IsValid)
                    {
                        if (mr.Exists(member.Email))
                        {
                            ve.Add("UserExists", Resources.Messages.UserExists);
                            return new Tuple<bool, ValidationErrorList, Member>(false, ve, null);
                        }
                        //**** create & save:
                        MembershipUser newUser = Membership.CreateUser(member.Email, password);
                        newUser.IsApproved = statusCode == (byte)MemberStatus.Active;
                        Roles.AddUserToRole(newUser.UserName, MyRoles.Member);
                        Membership.UpdateUser(newUser);
                        mr.Insert(member);
                        //**** add following for social network account:
                        var flw = new Following();
                        flw.FollowerId = member.Email;
                        flw.FollowedId = Configs.SocialNetworksAccount;
                        flw.DateOfBeginFollowing = MyHelper.Now;
                        mr.Insert(flw);
                        return new Tuple<bool, ValidationErrorList, Member>(true, new ValidationErrorList(), member);
                    }
                    else
                    {
                        return new Tuple<bool, ValidationErrorList, Member>(false, member.ValidationErrors, null);
                    }
                }
            }
            catch(Exception ex)
            {
                var ve = new ValidationErrorList();
                ve.Add("Exception", ex.Message);
                return new Tuple<bool, ValidationErrorList, Member>(false, ve, null);
            }
        }
        public static string YouOrHimOrFullName(Member owner, Member actor)
        {
            var owner_email = owner.Email;
            return (owner_email == HttpContext.Current.User.Identity.Name ? Resources.Words.You : (owner_email == actor.Email ? (actor.Gender.HasValue && actor.Gender.Value ? Resources.Words.His : Resources.Words.Her) : (owner.StatusCode != (byte)MemberStatus.ScheduledForDelete ? owner.FullName : Resources.Words.DeletedAccount)));
        }
        public static Member GetModelFromCollection(FormCollection form, bool generateAlias, System.Data.Objects.ObjectContext context)
        {
            Member member = new Member();
            member.Name = form["Name"];
            member.LastName = form["LastName"];
            member.Email = form["Email"].ToLower();
            member.BirthDay = DateHelper.GetMiladyDateFromInfo(form["Day"], form["Month"], form["Year"]);
            member.Gender = form["Gender"] != null ? (form["Gender"] == "Male" ? true : false) : (bool?)null;
            member.RegistrationCode = MyHelper.GetRandomString(10, false);
            member.RegistrationDate = MyHelper.Now;
            member.LastEmailNotificationsDate = MyHelper.Now;
            if (generateAlias)
                member.Alias = FindAlias(member.Email, member.BirthDay, context);
            return member;
        }
        private static string FindAlias(string email, DateTime? birthDate, System.Data.Objects.ObjectContext context)
        {
            var mr = new Repository.MembersRepository(context);
            //******************
            var reg = new System.Text.RegularExpressions.Regex(Patterns.AliasNonValidCharsRemover);
            var alias_base = email.Split(new char[] { '@' })[0];
            alias_base = reg.Replace(alias_base, "");
            // step 1: email name:
            var alias = alias_base;
            if (!mr.AliasExists(alias))
                return alias;
            // step 2: amail name + birth year:
            alias = alias_base + "_" + birthDate.Value.Year;
            if (!mr.AliasExists(alias))
                return alias;
            // step 3: amail name + random string:
            alias = alias_base + "_" + MyHelper.GetRandomString(3, true);
            if (!mr.AliasExists(alias))
                return alias;
            //else return mail:
            return reg.Replace(email, "");
        }
        public static string GeneratePassword()
        {
            var st1 = MyHelper.GetRandomString(6, true);
            var st2 = MyHelper.GetRandomNumbers(100, 1000, 1)[0];
            return st1 + st2.ToString();
        }
        public static bool IsInAdminGroup(string username, string accessLevel)
        {
            if (Roles.IsUserInRole(username, MyRoles.Administrator))
            {
                return true;
            }
            else if (Roles.IsUserInRole(username, MyRoles.Manager))
            {
                if (string.IsNullOrEmpty(accessLevel))
                    return true;
                var mr = new MembersRepository();
                var userAccess = mr.GetManagerAccessLevel(username);
                return userAccess.Contains(accessLevel);
            }
            return false;
        }
        #endregion

        #region IS VISIBLE TO:
        public bool IsFriendsVisibleTo(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            if ((ps.WhoCanSeeFriends == (byte)VisibleTo.EveryOne) ||
                (ps.WhoCanSeeFriends == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoCanSeeFriends == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoCanSeeFriends == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsBasicInfoVisibleTo(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            if ((ps.WhoCanSeeBasicInformation == (byte)VisibleTo.EveryOne) ||
                (ps.WhoCanSeeBasicInformation == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoCanSeeBasicInformation == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoCanSeeBasicInformation == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsEducationsVisibleTo(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            if ((ps.WhoSeeEducationActivities == (byte)VisibleTo.EveryOne) ||
                (ps.WhoSeeEducationActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoSeeEducationActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoSeeEducationActivities == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsPostingOnWallAvailableTo(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            // is himself:
            if (this.Email == username)
                return true;
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            // is blocked?
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            if (ps.IsBlocked(username))
                return false;
            //is allowed base on privacy settigs:
            var res = false;
            if ((ps.WhoCanWriteOnWall == (byte)VisibleTo.EveryOne) ||
                (ps.WhoCanWriteOnWall == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoCanWriteOnWall == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoCanWriteOnWall == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsFavoritesVisibleTo(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            if ((ps.WhoCanSeeFavorites == (byte)VisibleTo.EveryOne) ||
                (ps.WhoCanSeeFavorites == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoCanSeeFavorites == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoCanSeeFavorites == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsSkillsVisibleTo(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            if ((ps.WhoSeeSkillActivities == (byte)VisibleTo.EveryOne) ||
                (ps.WhoSeeSkillActivities == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoSeeSkillActivities == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoSeeSkillActivities == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsAllowedToSendMessage(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //**********************************
            if (username == this.Email)
                return false;
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            // is blocked?
            if (ps.IsBlocked(username))
                return false;
            // is allowed?
            if ((ps.WhoCanSendMessage == (byte)VisibleTo.EveryOne) ||
                (ps.WhoCanSendMessage == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoCanSendMessage == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoCanSendMessage == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsAllowedToSendFriendshipRequest(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            var are_friends = mr.AreFriends(this.Email, username);
            if (username == this.Email)
                return false;
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty) && !are_friends)
                return true;
            //
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            // is blocked?
            if (ps.IsBlocked(username))
                return false;
            return ps.AllowSendFriendshipRequest && username.Trim().Length > 0 && !are_friends;
        }
        public bool IsAllowedToBeginChat(string username, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            //**********************
            if (username == this.Email)
                return false;
            //is admin?
            if (Member.IsInAdminGroup(username, string.Empty))
                return true;
            //
            var res = false;
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            // is blocked?
            if (ps.IsBlocked(username))
                return false;
            // is allowed in privacy settings:
            if ((ps.WhoCanBeginChat == (byte)VisibleTo.EveryOne) ||
                (ps.WhoCanBeginChat == (byte)VisibleTo.Members && username.Trim().Length > 0) ||
                (ps.WhoCanBeginChat == (byte)VisibleTo.Friends && mr.AreFriends(this.Email, username)) ||
                (ps.WhoCanBeginChat == (byte)VisibleTo.OnlyMe && username == this.Email))
            {
                res = true;
            }
            return res;
        }
        public bool IsAllowedToLeaveComment(string username)
        {
            // is himself:
            if (this.Email == username)
                return true;
            //is admin?
            if (Member.IsInAdminGroup(username, ManagerAccessLevels.core_comments))
                return true;
            // is blocked?
            var ps = this.PrivacySetting != null ? this.PrivacySetting : PrivacySetting.Default;
            if (ps.IsBlocked(username))
                return false;
            // otherwise return true:
            return true;
        }
        #endregion
    }
}