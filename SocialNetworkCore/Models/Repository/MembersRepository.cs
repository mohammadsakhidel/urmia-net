using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class MembersRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public MembersRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public MembersRepository(System.Data.Objects.ObjectContext context)
        {
            db = context != null ? (SocialNetDbEntities)context : new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }
        #endregion

        #region Properties:
        public System.Data.Objects.ObjectContext Context
        {
            get
            {
                return db;
            }
        }
        #endregion
        //**********************************************************************************
        #region SELECT:
        public Member Get(string email)
        {
            try
            {
                return db.Members.Single(m => m.Email == email);
            }
            catch
            {
                return null;
            }
        }
        public Member GetByUserName(string username)
        {
            try
            {
                return db.Members.Single(m => m.Alias == username);
            }
            catch
            {
                return null;
            }
        }
        public Member GetByFacebookUserId(string fbUserId)
        {
            try
            {
                var q = from fbc in db.FacebookConnections
                        join m in db.Members
                        on fbc.MemberId equals m.Email
                        where fbc.UserId == fbUserId
                        select m;
                return q.Any() ? q.First() : null;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Member> Get(IEnumerable<string> emails)
        {
            try
            {
                return db.Members.Where(m => emails.Contains(m.Email));
            }
            catch
            {
                return new List<Member>();
            }
        }
        public IEnumerable<Member> GetByUserName(IEnumerable<string> usernames)
        {
            try
            {
                return db.Members.Where(m => usernames.Contains(m.Alias));
            }
            catch
            {
                return null;
            }
        }
        public ProfileCoverPhoto GetMemberProfileCoverPhoto(string memberId)
        {
            return db.SharedObjects.OfType<ProfileCoverPhoto>().Where(pcp => pcp.MemberId == memberId).FirstOrDefault();
        }
        public IEnumerable<Member> GetAllMembers()
        {
            return db.Members.AsEnumerable();
        }
        public IEnumerable<Member> GetAllActiveMembers(int pgIndex)
        {
            return db.Members.Where(m =>
                m.StatusCode == (byte)MemberStatus.Active
                ).OrderBy(m => m.Name).ThenBy(m => m.LastName).Skip(pgIndex * Digits.PeoplePageSize).Take(Digits.PeoplePageSize);
        }
        public int GetAllActiveMembersCount()
        {
            return db.Members.Where(m =>
                m.StatusCode == (byte)MemberStatus.Active
                ).Count();
        }
        public bool Exists(string email)
        {
            return db.Members.Where(m => m.Email == email).Any();
        }
        public string GetRegisterationCode(string email)
        {
            try
            {
                return db.Members.Single(m => m.Email == email).RegistrationCode;
            }
            catch
            {
                return "";
            }
        }
        public bool AliasExists(string alias)
        {
            return db.Members.Where(m => m.Alias == alias).Any();
        }
        public IEnumerable<Member> FindFriends(string email)
        {
            var list = from f in db.FriendshipRelations
                       join m in db.Members
                       on (email != f.MemberOneId ? f.MemberOneId : f.MemberTwoId) equals m.Email
                       where ((f.MemberOneId == email || f.MemberTwoId == email) && m.StatusCode == (byte)MemberStatus.Active)
                       select m;
            return list;
        }
        public IEnumerable<Member> FindFriends(string email, List<string> excepted)
        {
            var list = from f in db.FriendshipRelations
                       join m in db.Members
                       on (email != f.MemberOneId ? f.MemberOneId : f.MemberTwoId) equals m.Email
                       where ((f.MemberOneId == email || f.MemberTwoId == email) && 
                       m.StatusCode == (byte)MemberStatus.Active &&
                       (email != f.MemberOneId ? !excepted.Contains(f.MemberOneId) : !excepted.Contains(f.MemberTwoId)))
                       select m;
            return list;
        }
        public IEnumerable<Member> FindFollowedMembers(string email, List<string> excepted)
        {
            var list = from flwing in db.Followings
                       join flwed in db.Members
                       on flwing.FollowedId equals flwed.Email
                       where (flwing.FollowerId == email &&
                        flwed.StatusCode == (byte)MemberStatus.Active &&
                        !excepted.Contains(flwed.Email)) &&
                        (flwed.PrivacySetting != null ? 
                        // if restricted from followed privacy setting, only retrieve friends:
                        (flwed.PrivacySetting.MembersCanFollowMe || db.FriendshipRelations.Where(fr => (fr.MemberOneId == flwed.Email && fr.MemberTwoId == email) || (fr.MemberTwoId == flwed.Email && fr.MemberOneId == email)).Any()) :
                        true)
                       select flwed;
            return list;
        }
        public IEnumerable<Member> FindPagedFriends(string email, int pgIndex)
        {
            var list = (from f in db.FriendshipRelations
                        join m in db.Members
                        on (email != f.MemberOneId ? f.MemberOneId : f.MemberTwoId) equals m.Email
                        where (f.MemberOneId == email || f.MemberTwoId == email) && m.StatusCode == (byte)MemberStatus.Active
                        orderby f.DateOfAdd
                        select m).Skip(Digits.ListingItemsPageSize * pgIndex).Take(Digits.ListingItemsPageSize);
            return list;
        }
        public int GetFriendsCount(string email)
        {
            var c = (from f in db.FriendshipRelations
                        join m in db.Members
                        on (email != f.MemberOneId ? f.MemberOneId : f.MemberTwoId) equals m.Email
                        where (f.MemberOneId == email || f.MemberTwoId == email) && m.StatusCode == (byte)MemberStatus.Active
                        orderby f.DateOfAdd
                        select f).Count();
            return c;
        }
        public IEnumerable<Member> RetrieveSomeFriends(string email, int count)
        {
            var list = (from f in db.FriendshipRelations
                        join m in db.Members
                        on (email != f.MemberOneId ? f.MemberOneId : f.MemberTwoId) equals m.Email
                        where (f.MemberOneId == email || f.MemberTwoId == email) && m.StatusCode == (byte)MemberStatus.Active
                        orderby Guid.NewGuid()
                        select m).Take(count);
            return list;
        }
        public IEnumerable<string> FindFriendNames(string email)
        {
            var list = from f in db.FriendshipRelations
                       where f.MemberOneId == email || f.MemberTwoId == email
                       select (email != f.MemberOneId ? f.MemberOneId : f.MemberTwoId);
            return list;
        }
        public FriendshipRequest GetPendingFriendshipRequest(string from, string to)
        {
            var req = db.FriendshipRequests.Where(f => f.From == from && f.To == to && f.Status == (byte)FriendshipStatus.Requested).FirstOrDefault();
            return req;
        }
        public IEnumerable<Member> FindAssociatedUsers(string associatedToUser, string partialName)
        {
            var q = from m in db.Members
                    let isActive = m.StatusCode == (byte)MemberStatus.Active
                    where m.Email != associatedToUser && (m.Name + " " + m.LastName).StartsWith(partialName) && isActive
                    select m;
            return q.ToList();
        }
        public bool AreFriends(string user1, string user2)
        {
            return db.FriendshipRelations.Where(f => 
                (f.MemberOneId == user1 && f.MemberTwoId == user2) ||
                (f.MemberOneId == user2 && f.MemberTwoId == user1)).Any() || user1 == user2;
        }
        public bool IsScheduledForDelete(string email)
        {
            try
            {
                return db.AccountDeleteSchedules.Where(s => s.MemberId == email && s.Status == (byte)DeleteScheduleStatus.Scheduled).Any();
            }
            catch
            {
                return false;
            }
        }
        public AccountDeleteSchedule GetDeleteSchedule(string email)
        {
            try
            {
                return db.AccountDeleteSchedules.Where(s => s.MemberId == email && s.Status == (byte)DeleteScheduleStatus.Scheduled).ToList().Last();
            }
            catch
            {
                return null;
            }
        }
        public PrivacySetting GetPrivacy(string memberId)
        {
            try
            {
                return db.PrivacySettings.Single(ps => ps.MemberId == memberId);
            }
            catch
            {
                return PrivacySetting.Default;
            }
        }
        public bool HasSkill(string memberId, string skill)
        {
            return db.Skills.Where(s => s.MemberId == memberId && s.SkillTitle == skill).Any();
        }
        public IEnumerable<Skill> GetSkills(string memberId)
        {
            return db.Skills.Where(s => s.MemberId == memberId);
        }
        public Skill GetSkill(int id)
        {
            try
            {
                return db.Skills.Single(s => s.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public Education GetEducation(int id)
        {
            try
            {
                return db.Educations.Single(s => s.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Education> GetEducations(string memberId)
        {
            return db.Educations.Where(e => e.MemberId == memberId);
        }
        public AlbumPhoto FindProfilePhoto(string email)
        {
            try
            {
                return db.SharedObjects.OfType<AlbumPhoto>().Single(p => p.MemberId == email && p.IsProfilePhoto);
            }
            catch
            {
                return null;
            }
        }
        public bool PendingFriendshipRequestExists(string mem1, string mem2)
        {
            return db.FriendshipRequests
                .Where(fr => ((fr.From == mem1 && fr.To == mem2) || (fr.From == mem2 && fr.To == mem1)) && 
                    fr.Status == (byte)FriendshipStatus.Requested).Any();
        }
        public IEnumerable<FriendshipRequest> FindNewFriendshipRequest(string email)
        {
            return db.FriendshipRequests.Where(f => f.To == email && f.Status == (byte)FriendshipStatus.Requested).OrderByDescending(f => f.DateOfCreate);
        }
        public int FindNewFriendshipRequestsCount(string email)
        {
            return db.FriendshipRequests.Where(f => f.To == email && f.Status == (byte)FriendshipStatus.Requested).Count();
        }
        public int FindNewFriendshipRequestsCountGTDate(string email, DateTime dt)
        {
            return db.FriendshipRequests.Where(f => f.To == email && f.Status == (byte)FriendshipStatus.Requested && f.DateOfCreate > dt).Count();
        }
        public FriendshipRequest GetFriendshipRequest(int reqId)
        {
            try
            {
                return db.FriendshipRequests.Single(f => f.Id == reqId);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Member> GetMembersForHomeCollection()
        {
            var list = GetRandomActiveMembers(Digits.HomeMemberCollectionSize, true, new List<string> { });
            return list;
        }
        public bool IsDeservedForWelcome(string memberId)
        {
            try
            {
                var member = Get(memberId);
                return member.RegistrationDate.AddDays(Digits.WelcomeMessageWindowDays) >= MyHelper.Now;
            }
            catch
            {
                return false;
            }
        }
        public NotificationSetting GetNotificationSettings(string memberId)
        {
            try
            {
                return db.NotificationSettings.Single(n => n.MemberId == memberId);
            }
            catch
            {
                var set = NotificationSetting.Default;
                set.MemberId = memberId;
                return set;
            }
        }
        public IEnumerable<Member> GetOnlineMembers()
        {
            var cr = new ConnectionsRepository();
            var connections = cr.LoadPool(HttpContext.Current).Values.Select(v => v.MemberId).Distinct().ToList();
            var q = from m in db.Members
                    join c in connections
                    on m.Email equals c
                    select m;
            return q;
        }
        public IEnumerable<Member> SearchMembers(SearchMemberInfo info)
        {
            //conditions:
            var query = (info.JustOnlines ? GetOnlineMembers().AsQueryable() : db.Members.Include("BasicInformation").Where(m =>
                m.StatusCode == (byte)MemberStatus.Active
                ).AsQueryable());
            // members privacy:
            query = query.Where(m => m.PrivacySetting == null || (m.PrivacySetting != null && m.PrivacySetting.DisplayInSearch));
            //
            if (!String.IsNullOrEmpty(info.FullName))
                query = query.Where(m => (m.Name + " " + m.LastName).Contains(info.FullName));
            if (info.Gender.HasValue)
                query = query.Where(m => m.Gender.HasValue && m.Gender.Value == info.Gender.Value);
            if (info.EducationLevel.HasValue)
                query = query.Where(m => m.Educations.Where(e => e.EducationLevel == info.EducationLevel.Value).Any());
            if (info.MaritalStatus.HasValue)
                query = query.Where(m => m.BasicInformation != null && m.BasicInformation.MaritalStatus.HasValue && m.BasicInformation.MaritalStatus.Value == info.MaritalStatus.Value);
            if (info.JustWithPP)
                query = query.Where(m => !String.IsNullOrEmpty(m.ProfilePhoto));
            if (info.AgeFrom.HasValue)
                query = query.Where(m => m.BirthDay.HasValue && (MyHelper.Now.Year - m.BirthDay.Value.Year >= info.AgeFrom));
            if (info.AgeTo.HasValue)
                query = query.Where(m => m.BirthDay.HasValue && (MyHelper.Now.Year - m.BirthDay.Value.Year <= info.AgeTo));
            if (info.LivingRegion.HasValue)
                query = query.Where(m => m.BasicInformation != null && m.BasicInformation.LivingRegion.HasValue && m.BasicInformation.LivingRegion.Value == info.LivingRegion);
            if (!String.IsNullOrEmpty(info.LivingCity))
                query = query.Where(m => m.BasicInformation != null && !String.IsNullOrEmpty(m.BasicInformation.LivingCity) && m.BasicInformation.LivingCity.Replace(" ", "").Trim() == info.LivingCity.Replace(" ", "").Trim());
            // return:
            return query.OrderBy(m => Guid.NewGuid()).Take(Digits.SearchMembersPageSize);
        }
        public IEnumerable<Member> GetRandomActiveMembers(int count, bool just_with_pp, IEnumerable<string> exceptfor)
        {
            var members_count = db.Members.Where(m =>
                m.StatusCode == (byte)MemberStatus.Active
                && !exceptfor.Contains(m.Email) && 
                (just_with_pp ? !String.IsNullOrEmpty(m.ProfilePhoto) : true)).Count();
            var skip_count = (members_count > count ? MyHelper.GetRandomNumbers(0, members_count - count + 1, 1).First() : 0);
            var members = db.Members.Where(m =>
                m.StatusCode == (byte)MemberStatus.Active
                && !exceptfor.Contains(m.Email) && (just_with_pp ? !String.IsNullOrEmpty(m.ProfilePhoto) : true)
                ).OrderBy(m => m.Email).Skip(skip_count).Take(count).ToList();
            return members.OrderBy(m => Guid.NewGuid());
        }
        public IEnumerable<string> GetAdminNames()
        {
            return System.Web.Security.Roles.GetUsersInRole(MyRoles.Administrator);
        }
        public bool IsFollowing(string followerId, string followedId)
        {
            return db.Followings.Where(f => f.FollowerId == followerId && f.FollowedId == followedId).Any();
        }
        public bool IsFollowingAllowed(Member follower, Member followed)
        {
            var flwdPrivacy = followed.PrivacySetting ?? PrivacySetting.Default;
            var isFollowingAllowed = follower != null &&
                follower.Email != followed.Email &&
                flwdPrivacy.MembersCanFollowMe;
            return isFollowingAllowed;
        }
        public int GetFollowersCount(string memberId)
        {
            return db.Followings.Where(f => f.FollowedId == memberId).Count();
        }
        public string GetManagerAccessLevel(string memberId)
        {
            var mngr = db.Managers.SingleOrDefault(m => m.MemberId == memberId);
            if (mngr != null)
                return mngr.AccessLevel;
            return string.Empty;
        }
        #endregion
        #region INSERT:
        public void Insert(Favorite fav)
        {
            db.DeleteFavorite(fav.MemberId);
            db.Favorites.AddObject(fav);
            AddChangeInfoActivity(fav);
            Save();
        }

        public void Insert(PrivacySetting ps)
        {
            db.DeletePrivacySetting(ps.MemberId);
            db.PrivacySettings.AddObject(ps);
            Save();
        }

        public void Insert(NotificationSetting ns)
        {
            db.DeleteNotificationSetting(ns.MemberId);
            db.NotificationSettings.AddObject(ns);
            Save();
        }

        public void Add(AccountSetting ac_set)
        {
            db.DeleteAccountSetting(ac_set.MemberId);
            db.AccountSettings.Detach(ac_set);
            db.AccountSettings.AddObject(ac_set);
        }

        public void Insert(Member member)
        {
            member.Email = member.Email.ToLower();
            db.Members.AddObject(member);
            Save();
        }

        public void Add(BasicInformation info)
        {
            db.DeleteBasicInformation(info.MemberId);
            db.BasicInformations.AddObject(info);
            AddChangeInfoActivity(info);
        }

        public void Add(AccountDeleteSchedule schedule)
        {
            db.AccountDeleteSchedules.AddObject(schedule);
        }

        public void Insert(Skill skill)
        {
            db.Skills.AddObject(skill);
            AddSkillActivity(skill);
            Save();
        }

        public void Insert(Education edu)
        {
            db.Educations.AddObject(edu);
            AddEducationActivity(edu);
            Save();
        }

        public void Insert(FriendshipRequest friendshipReq)
        {
            db.FriendshipRequests.AddObject(friendshipReq);
            Save();
            // notify if online:
            if (OnlinesHelper.IsOnline(friendshipReq.To))
            {
                var cr = new ConnectionsRepository();
                var connections = cr.FindConnections(friendshipReq.To).ToList();
                var signals = connections.Select(con => new NotifySignal { Type = NotifySignalType.NewRequest, ConnectionId = con.Id });
                if (connections.Any())
                {
                    SocialNetHub.SendNotifySignals(signals);
                }
            }
            else //send email notification if needed:
            {
                var reciever = Get(friendshipReq.To);
                var sett = reciever.NotificationSetting ?? NotificationSetting.Default;
                if (sett.OnFriendshipRequest && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                {
                    EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                    reciever.LastEmailNotificationsDate = MyHelper.Now;
                    Save();
                }
            }
        }

        public void Add(FriendshipRelation rel)
        {
            db.FriendshipRelations.AddObject(rel);
        }

        public void Add(ProfileView pv)
        {
            db.ProfileViews.AddObject(pv);
        }

        public void Add(ProfileCoverPhoto pcp)
        {
            db.SharedObjects.AddObject(pcp);
        }

        public void AddAcceptRequestNotification(FriendshipRequest friendshipReq)
        {
            var not = new AcceptRequestNotification();
            not.MemberId = friendshipReq.From;
            not.CreateTime = MyHelper.Now;
            not.Status = (byte)NotificationStatus.Unread;
            not.AcceptedBy = friendshipReq.To;
            db.Notifications.AddObject(not);
            // notify if online:
            if (OnlinesHelper.IsOnline(not.MemberId))
            {
                var cr = new ConnectionsRepository();
                var connections = cr.FindConnections(not.MemberId).ToList();
                var signals = connections.Select(con => new NotifySignal { Type = NotifySignalType.NewNotification, ConnectionId = con.Id });
                if (connections.Any())
                {
                    SocialNetHub.SendNotifySignals(signals);
                }
            }
            else //send email notification if needed:
            {
                var reciever = Get(not.MemberId);
                var sett = reciever.NotificationSetting ?? NotificationSetting.Default;
                if (sett.OnFriendshipRequest && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                {
                    EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                    reciever.LastEmailNotificationsDate = MyHelper.Now;
                }
            }
        }

        public void Insert(Following following)
        {
            db.Followings.AddObject(following);
            Save();
        }

        public void Add(Following following)
        {
            db.Followings.AddObject(following);
        }

        public void Insert(FacebookConnection fbc)
        {
            db.FacebookConnections.AddObject(fbc);
            Save();
        }

        public void Insert(Manager mngr)
        {
            db.Managers.AddObject(mngr);
            Save();
        }
        #endregion
        #region DELETE:
        public void Delete(BasicInformation info)
        {
            db.BasicInformations.DeleteObject(info);
        }

        public void DeleteProfileCover(string memberId)
        {
            var c = db.SharedObjects.OfType<ProfileCoverPhoto>().Where(pcp => pcp.MemberId == memberId).FirstOrDefault();
            if (c != null)
            {
                // clear data:
                db.SharedObjects.DeleteObject(c);
                Save();
                // delete files:
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(Urls.ProfileCovers + c.FileName)))
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(Urls.ProfileCovers + c.FileName));
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(Urls.ProfileCoverBases + c.FileName)))
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(Urls.ProfileCoverBases + c.FileName));
            }
        }

        public void RemoveFriendshipRelation(string member, string friend)
        {
            var friendshipRel = db.FriendshipRelations.Where(f => 
                (f.MemberOneId == member && f.MemberTwoId == friend) || 
                (f.MemberOneId == friend && f.MemberTwoId == member));
            foreach (var f in friendshipRel)
            {
                db.FriendshipRelations.DeleteObject(f);
            }
        }

        public void DeleteFriendshipRelation(string member, string friend)
        {
            RemoveFriendshipRelation(member, friend);
            Save();
        }

        public void Delete(ProfileCoverPhoto p)
        {
            db.SharedObjects.DeleteObject(p);
            Save();
        }

        public void Delete(Skill skill)
        {
            db.Skills.DeleteObject(skill);
            Save();
        }

        public void Delete(Education edu)
        {
            db.Educations.DeleteObject(edu);
            Save();
        }

        public void DeleteFriendshipRequest(int reqId)
        {
            FriendshipRequest f = db.FriendshipRequests.Single(fr => fr.Id == reqId);
            if (f != null)
            {
                db.FriendshipRequests.DeleteObject(f);
                Save();
            }
        }

        public void Remove(string followerId, string followedId)
        {
            var flwings = db.Followings.Where(f => f.FollowerId == followerId && f.FollowedId == followedId);
            foreach (var f in flwings)
            {
                db.Followings.DeleteObject(f);
            }
        }

        public void Delete(string followerId, string followedId)
        {
            Remove(followerId, followedId);
            Save();
        }

        public void DeleteManagerSetting(string email)
        {
            var o = db.Managers.SingleOrDefault(m => m.MemberId == email);
            if (o != null)
            {
                db.Managers.DeleteObject(o);
                Save();
            }

        }
        #endregion
        #region Update:
        public void IncreaseProfileCounter(string memberId)
        {
            db.IncreaseProfileCounter(memberId);
        }
        #endregion
        #region Private Methods:
        public void AddChangeInfoActivity(BasicInformation info)
        {
            var act = new ChangeInfoActivity();
            act.MemberId = info.MemberId;
            act.TimeOfAct = MyHelper.Now;
            db.Activities.AddObject(act);
        }

        public void AddChangeInfoActivity(Favorite info)
        {
            var act = new ChangeInfoActivity();
            act.MemberId = info.MemberId;
            act.TimeOfAct = MyHelper.Now;
            db.Activities.AddObject(act);
        }

        public void AddChangeCoverActivity(string memberId, int newCoverPhotoId)
        {
            var act = new ChangeCoverActivity();
            act.MemberId = memberId;
            act.TimeOfAct = MyHelper.Now;
            act.NewPofileCoverPhotoId = newCoverPhotoId;
            db.Activities.AddObject(act);
        }

        public void AddChangePPActivity(string memberId, int photoId)
        {
            var act = new ChangePPActivity();
            act.MemberId = memberId;
            act.TimeOfAct = MyHelper.Now;
            act.PhotoId = photoId;
            db.Activities.AddObject(act);
        }

        public void AddEducationActivity(Education edu)
        {
            var act = new EducationActivity();
            act.MemberId = edu.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.EducationId = edu.Id;
            db.Activities.AddObject(act);
        }

        public void AddSkillActivity(Skill skill)
        {
            var act = new SkillActivity();
            act.MemberId = skill.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.SkillId = skill.Id;
            db.Activities.AddObject(act);
        }
        #endregion
        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}