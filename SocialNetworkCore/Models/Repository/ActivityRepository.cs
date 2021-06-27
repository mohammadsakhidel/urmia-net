using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class ActivityRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public ActivityRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public ActivityRepository(System.Data.Objects.ObjectContext context)
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
        public Activity Get(int id)
        {
            try
            {
                return db.Activities.Single(act => act.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Activity> FindNewsFeedActivities(string email)
        {
            return GetNewsFeedActivities(email, null);
        }
        public IEnumerable<Activity> FindNewsFeedActivities(string email, DateTime dt)
        {
            return GetNewsFeedActivities(email, dt);
        }
        public bool IsThereMoreNewsFeedActivities(string email, DateTime dt)
        {
            return GetNewsFeedActivities(email, dt).Any();
        }
        public IEnumerable<Activity> FindPublicActivities(string email, string tag)
        {
            return GetPublicActivities(email, tag, null);
        }
        public IEnumerable<Activity> FindPublicActivities(string email, string tag, DateTime dt)
        {
            return GetPublicActivities(email, tag, dt);
        }
        public bool IsThereMorePublicActivities(string email, string tag, DateTime dt)
        {
            return GetPublicActivities(email, tag, dt).Any();
        }
        public IEnumerable<Activity> FindMemberActivitiesForHisWall(string wallowner, string viewer)
        {
            return GetMemberActivitiesForHisWall(wallowner, viewer, null);
        }
        public IEnumerable<Activity> FindMemberActivitiesForHisWall(string wallowner, string viewer, DateTime dt)
        {
            return GetMemberActivitiesForHisWall(wallowner, viewer, dt);
        }
        public bool IsThereMoreWallActivities(string wallowner, string viewer, DateTime dt)
        {
            return GetMemberActivitiesForHisWall(wallowner, viewer, dt).Any();
        }
        public IEnumerable<Activity> FindProfilePageActivitis(string profile_owner, string viewer)
        {
            /*
            var acts = db.Activities
                .Include("Member")
                .Where(
                    act =>
                    (act.MemberId == email && !(act is PostOnWallActivity))
                )
                .OrderByDescending(act => act.TimeOfAct)
                .Take(Digits.ActivitiesPageSize);
             * */
            return GetProfilePageActivitis(profile_owner, viewer, null);
        }
        public IEnumerable<Activity> FindProfilePageActivitis(string profile_owner, string viewer, DateTime dt)
        {
            return GetProfilePageActivitis(profile_owner, viewer, dt);
        }
        public bool IsThereMoreProfilePageActivitis(string profile_owner, string viewer, DateTime dt)
        {
            return GetProfilePageActivitis(profile_owner, viewer, dt).Any();
        }
        public PostActivity FindPostActivity(int postId)
        {
            try
            {
                return db.Activities.OfType<PostActivity>().Single(act => act.PostId == postId);
            }
            catch
            {
                return null;
            }
        }
        public PostOnWallActivity FindPostOnWallActivity(int postId)
        {
            try
            {
                return db.Activities.OfType<PostOnWallActivity>().Single(act => act.PostId == postId);
            }
            catch
            {
                return null;
            }
        }
        public ActivitySetting GetActivitySetting(string email)
        {
            try
            {
                return db.ActivitySettings.Single(set => set.MemberId == email);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Activity> FindActivitiesForAdmin(DateTime date)
        {
            var acts = (from act in db.Activities.Include("Member")
                        where (act is PostActivity || act is PhotoActivity || act is PostOnWallActivity) && (act.TimeOfAct.Year == date.Year && act.TimeOfAct.Month == date.Month && act.TimeOfAct.Day == date.Day)
                        orderby act.TimeOfAct descending
                        select act).Take(Digits.ActivitiesPageSize);
            return acts;
        }
        public IEnumerable<Activity> FindActivitiesForAdmin(DateTime date_of_act, DateTime pagingDateTime)
        {
            var acts = (from act in db.Activities.Include("Member")
                        where (act is PostActivity || act is PhotoActivity || act is PostOnWallActivity) && (act.TimeOfAct.Year == date_of_act.Year && act.TimeOfAct.Month == date_of_act.Month && act.TimeOfAct.Day == date_of_act.Day) && (act.TimeOfAct < pagingDateTime)
                        orderby act.TimeOfAct descending
                        select act).Take(Digits.ActivitiesPageSize);
            return acts;
        }
        public bool IsThereMoreActivitiesForAdmin(DateTime date_of_act, DateTime pagingDateTime)
        {
            var acts = db.Activities
                .Where(act =>
                    (act is PostActivity || act is PhotoActivity || act is PostOnWallActivity) && (act.TimeOfAct.Year == date_of_act.Year && act.TimeOfAct.Month == date_of_act.Month && act.TimeOfAct.Day == date_of_act.Day) && (act.TimeOfAct < pagingDateTime)
                );
            return acts.Any();
        }
        #endregion

        #region INSERT:
        public void InsertActivity(Post post)
        {
            var act = new PostActivity();
            act.MemberId = post.MemberId;
            act.TimeOfAct = MyHelper.Now;
            act.PostId = post.Id;
            db.Activities.AddObject(act);
            Save();
        }

        public void Insert(ActivitySetting sett)
        {
            db.ActivitySettings.AddObject(sett);
            Save();
        }
        #endregion

        #region DELETE:
        public void Delete(Activity act)
        {
            db.Activities.DeleteObject(act);
            Save();
        }
        #endregion

        public void Save()
        {
            db.SaveChanges();
        }

        #region Private Methods:
        private IEnumerable<Activity> GetNewsFeedActivities(string email, DateTime? dt)
        {
            var mr = new MembersRepository(db);
            // default privacy settings:
            #region Get Default Privacy Setting Values:
            var WhoSeeLikeActivitiesDef = PrivacySetting.Default.WhoSeeLikeActivities;
            var WhoSeeCommentActivitiesDef = PrivacySetting.Default.WhoSeeCommentActivities;
            var WhoSeeFriendshipActivitiesDef = PrivacySetting.Default.WhoSeeFriendshipActivities;
            var WhoSeeChangeInfoActivitiesDef = PrivacySetting.Default.WhoSeeChangeInfoActivities;
            var WhoSeeEducationActivitiesDef = PrivacySetting.Default.WhoSeeEducationActivities;
            var WhoSeeSkillActivitiesDef = PrivacySetting.Default.WhoSeeSkillActivities;
            var WhoSeeChangeCoverActivitiesDef = PrivacySetting.Default.WhoSeeChangeCoverActivities;
            #endregion
            // ^^^^^^^^^^^
            var is_auth = !String.IsNullOrEmpty(email);
            var act_settings = GetActivitySetting(email);
            var unwanted_acts = (act_settings != null ? act_settings.UnwantedActivityIds : new List<int>());
            var unwanted_actors = (act_settings != null ? act_settings.UnwantedActorIds : new List<string>());
            var newsfeed_sett = (act_settings != null ? act_settings.NewsFeedInformingObj : NewsFeedInforming.Default);
            var followedMembers = mr.FindFollowedMembers(email, unwanted_actors);
            if (!followedMembers.Any())
                return Enumerable.Empty<Activity>();
            // get  without object activities:
            #region Collect Activities Without Shared Object:
            var nobj_acts =
                from flwed in followedMembers
                join act in db.Activities on flwed.Email equals act.MemberId
                join privacy in db.PrivacySettings on act.MemberId equals privacy.MemberId into row
                from privacy in row.DefaultIfEmpty()
                where
                (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                !unwanted_acts.Contains(act.Id) &&
                    //ChangeInfoActivity
                ((newsfeed_sett.ViewChangeInfo &&
                act is ChangeInfoActivity &&
                (((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == email) ||
                ((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == email && f.MemberOneId == act.MemberId)) || email == act.MemberId) select f).Any()) ||
                ((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                ((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.EveryOne))) ||
                    //ChangeCoverActivity
                (newsfeed_sett.ViewChangeCover &&
                act is ChangeCoverActivity &&
                (((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == email) ||
                ((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == email && f.MemberOneId == act.MemberId)) || email == act.MemberId) select f).Any()) ||
                ((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                ((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.EveryOne))) ||
                    //EducationActivity
                (newsfeed_sett.ViewEducations &&
                act is EducationActivity &&
                (((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == email) ||
                ((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == email && f.MemberOneId == act.MemberId)) || email == act.MemberId) select f).Any()) ||
                ((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                ((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.EveryOne))) ||
                    //SkillActivity
                (newsfeed_sett.ViewSkills &&
                act is SkillActivity &&
                (((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == email) ||
                ((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == email && f.MemberOneId == act.MemberId)) || email == act.MemberId) select f).Any()) ||
                ((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                ((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.EveryOne))))
                select act as Activity;
            #endregion
            //get post acts:
            #region Collect Post Activities:
            var po_acts = 
                newsfeed_sett.ViewPosts ?

                from flwed in followedMembers
                join act in db.Activities.OfType<PostActivity>() on flwed.Email equals act.MemberId
                join post in db.SharedObjects.OfType<Post>() on act.PostId equals post.Id
                where
                (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                !unwanted_acts.Contains(act.Id) &&
                (post.Status == (byte)PostStatus.Visible) &&
                ((post.VisibleTo == (byte)VisibleTo.OnlyMe && post.MemberId == email) ||
                (post.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == post.MemberId) || (f.MemberTwoId == email && f.MemberOneId == post.MemberId)) || email == post.MemberId) select f).Any()) ||
                (post.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                (post.VisibleTo == (byte)VisibleTo.EveryOne))
                select act as Activity

                : (new List<Activity>()).AsEnumerable();
            #endregion
            //get photo acts:
            #region Collect Photo Activities:
            var ph_acts =
                newsfeed_sett.ViewPhotos ?

                from flwed in followedMembers
                join act in db.Activities.OfType<PhotoActivity>() on flwed.Email equals act.MemberId
                join photo in db.SharedObjects.OfType<Photo>() on act.PhotoId equals photo.Id
                where
                (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                !unwanted_acts.Contains(act.Id) &&
                ((photo.VisibleTo == (byte)VisibleTo.OnlyMe && photo.MemberId == email) ||
                (photo.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == photo.MemberId) || (f.MemberTwoId == email && f.MemberOneId == photo.MemberId)) || email == photo.MemberId) select f).Any()) ||
                (photo.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                (photo.VisibleTo == (byte)VisibleTo.EveryOne))
                select act as Activity

                : (new List<Activity>()).AsEnumerable();
            #endregion
            //get sharing acts:
            #region Collect Sharing Activities:
            var sh_acts =
                newsfeed_sett.ViewShares ?

                from flwed in followedMembers
                join act in db.Activities.OfType<SharingActivity>() on flwed.Email equals act.MemberId
                join sharing in db.SharedObjects.OfType<Sharing>() on act.SharingId equals sharing.Id
                join obj in db.SharedObjects on sharing.SharedObjectId equals obj.Id
                where
                (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                !unwanted_acts.Contains(act.Id) &&
                ((sharing.VisibleTo == (byte)VisibleTo.OnlyMe && sharing.MemberId == email) ||
                (sharing.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == sharing.MemberId) || (f.MemberTwoId == email && f.MemberOneId == sharing.MemberId)) || email == sharing.MemberId) select f).Any()) ||
                (sharing.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                (sharing.VisibleTo == (byte)VisibleTo.EveryOne)) &&
                ((obj.VisibleTo == (byte)VisibleTo.OnlyMe && obj.MemberId == email) ||
                (obj.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == obj.MemberId) || (f.MemberTwoId == email && f.MemberOneId == obj.MemberId)) || email == obj.MemberId) select f).Any()) ||
                (obj.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                (obj.VisibleTo == (byte)VisibleTo.EveryOne))
                select act as Activity

                : (new List<Activity>()).AsEnumerable();
            #endregion
            //get object like acts:
            #region Collect SharedObjectLike Activities:
            var like_acts = (new List<Activity>()).AsEnumerable();
            if (newsfeed_sett.ViewLikes)
            {
                var like_acts_base =
                    from flwed in followedMembers
                    join act in db.Activities.OfType<LikeActivity>() on flwed.Email equals act.MemberId
                    join like in db.Likes.OfType<SharedObjectLike>() on act.LikeId equals like.Id
                    join obj in db.SharedObjects on like.SharedObjectId equals obj.Id
                    join privacy in db.PrivacySettings on act.MemberId equals privacy.MemberId into set1
                    from privacy in set1.DefaultIfEmpty()
                    where
                    !unwanted_acts.Contains(act.Id) &&
                    (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                    (((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == email) ||
                    ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == email && f.MemberOneId == act.MemberId)) || email == act.MemberId) select f).Any()) ||
                    ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                    ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.EveryOne))
                    select new { Activity = act, Object = obj, SharedObject = (obj is Sharing ? (from sharing in db.SharedObjects.OfType<Sharing>() where sharing.Id == obj.Id select sharing).FirstOrDefault().SharedObject : null) };
                like_acts =
                    from row in like_acts_base
                    where
                     ((row.Object.VisibleTo == (byte)VisibleTo.OnlyMe && row.Object.MemberId == email) ||
                     (row.Object.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == row.Object.MemberId) || (f.MemberTwoId == email && f.MemberOneId == row.Object.MemberId)) || email == row.Object.MemberId) select f).Any()) ||
                     (row.Object.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                     (row.Object.VisibleTo == (byte)VisibleTo.EveryOne)) &&
                     (row.SharedObject == null ||
                     ((row.SharedObject.VisibleTo == (byte)VisibleTo.OnlyMe && row.SharedObject.MemberId == email) ||
                     (row.SharedObject.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == row.SharedObject.MemberId) || (f.MemberTwoId == email && f.MemberOneId == row.SharedObject.MemberId)) || email == row.SharedObject.MemberId) select f).Any()) ||
                     (row.SharedObject.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                     (row.SharedObject.VisibleTo == (byte)VisibleTo.EveryOne)))
                    select row.Activity as Activity;
            }
            #endregion
            //get comment acts:
            #region Collect Comment Activities:
            var com_acts = (new List<Activity>()).AsEnumerable();
            if (newsfeed_sett.ViewComments)
            {
                var com_acts_base =
                    from flwed in followedMembers
                    join act in db.Activities.OfType<CommentActivity>() on flwed.Email equals act.MemberId
                    join com in db.Comments on act.CommentId equals com.Id
                    join obj in db.SharedObjects on com.SharedObjectId equals obj.Id
                    join privacy in db.PrivacySettings on act.MemberId equals privacy.MemberId into set1
                    from privacy in set1.DefaultIfEmpty()
                    where
                    !unwanted_acts.Contains(act.Id) &&
                    (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                    (((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == email) ||
                    ((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == email && f.MemberOneId == act.MemberId)) || email == act.MemberId) select f).Any()) ||
                    ((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                    ((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.EveryOne))
                    select new { Activity = act, Object = obj, SharedObject = (obj is Sharing ? (from sharing in db.SharedObjects.OfType<Sharing>() where sharing.Id == obj.Id select sharing).FirstOrDefault().SharedObject : null) };
                com_acts =
                    from row in com_acts_base
                    where
                     ((row.Object.VisibleTo == (byte)VisibleTo.OnlyMe && row.Object.MemberId == email) ||
                     (row.Object.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == row.Object.MemberId) || (f.MemberTwoId == email && f.MemberOneId == row.Object.MemberId)) || email == row.Object.MemberId) select f).Any()) ||
                     (row.Object.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                     (row.Object.VisibleTo == (byte)VisibleTo.EveryOne)) &&
                     (row.SharedObject == null ||
                     ((row.SharedObject.VisibleTo == (byte)VisibleTo.OnlyMe && row.SharedObject.MemberId == email) ||
                     (row.SharedObject.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == row.SharedObject.MemberId) || (f.MemberTwoId == email && f.MemberOneId == row.SharedObject.MemberId)) || email == row.SharedObject.MemberId) select f).Any()) ||
                     (row.SharedObject.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                     (row.SharedObject.VisibleTo == (byte)VisibleTo.EveryOne)))
                    select row.Activity as Activity;
            }
            #endregion
            //get change pp acts:
            #region Collect ChamgePP Activities:
            var cpp_acts =
                newsfeed_sett.ViewChangePP ?

                from flwed in followedMembers
                join act in db.Activities.OfType<ChangePPActivity>() on flwed.Email equals act.MemberId
                join photo in db.SharedObjects.OfType<Photo>() on act.PhotoId equals photo.Id
                where
                (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                !unwanted_acts.Contains(act.Id) &&
                ((photo.VisibleTo == (byte)VisibleTo.OnlyMe && photo.MemberId == email) ||
                (photo.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == photo.MemberId) || (f.MemberTwoId == email && f.MemberOneId == photo.MemberId)) || email == photo.MemberId) select f).Any()) ||
                (photo.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                (photo.VisibleTo == (byte)VisibleTo.EveryOne))
                select act as Activity

                : (new List<Activity>()).AsEnumerable();
            #endregion
            //concat and return all:
            var acts = po_acts.Concat(ph_acts).Concat(sh_acts).Concat(like_acts).Concat(com_acts).Concat(cpp_acts).Concat(nobj_acts).OrderByDescending(act => act.TimeOfAct).Take(Digits.ActivitiesPageSize);
            return acts;
        }
        private IEnumerable<Activity> GetPublicActivities(string email, string tag, DateTime? dt)
        {
            var mr = new MembersRepository(db);
            var is_auth = !String.IsNullOrEmpty(email);
            var is_viewer_admin = Member.IsInAdminGroup(email, string.Empty);
            // act settings:
            var act_settings = GetActivitySetting(email);
            var unwanted_acts = (act_settings != null ? act_settings.UnwantedActivityIds : new List<int>());
            var unwanted_actors = (act_settings != null ? act_settings.UnwantedActorIds : new List<string>());
            // get post activities:
            #region Collect Public Post Activities:
            var po_acts = from act in db.Activities.OfType<PostActivity>()
                          join mem in db.Members
                          on act.MemberId equals mem.Email
                          join post in db.SharedObjects.OfType<Post>() 
                          on act.PostId equals post.Id
                          where
                          (String.IsNullOrEmpty(tag) || post.SharedObjectTags.Select(t => t.Tag).Contains(tag)) &&
                          mem.StatusCode == (byte)MemberStatus.Active &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          !unwanted_acts.Contains(act.Id) &&
                          !unwanted_actors.Contains(act.MemberId) &&
                          (post.Status == (byte)PostStatus.Visible) &&
                          (is_viewer_admin ||
                          ((post.VisibleTo == (byte)VisibleTo.OnlyMe && post.MemberId == email) ||
                          (post.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == email && f.MemberTwoId == post.MemberId) || (f.MemberTwoId == email && f.MemberOneId == post.MemberId)) || email == post.MemberId) select f).Any()) ||
                          (post.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (post.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            // get photo activities:
            #region Collect Public Photo Activities:
            /*
            var ph_acts = from act in db.Activities.OfType<PhotoActivity>()
                          join photo in db.SharedObjects.OfType<Photo>() on act.PhotoId equals photo.Id
                          where
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          !unwanted_acts.Contains(act.Id) &&
                          !unwanted_actors.Contains(act.MemberId) &&
                          (is_viewer_admin ||
                          ((photo.VisibleTo == (byte)VisibleTo.OnlyMe && photo.MemberId == email) ||
                          (photo.VisibleTo == (byte)VisibleTo.Friends && (from f in db.Friendships where ((((f.From == email && f.To == photo.MemberId) || (f.To == email && f.From == photo.MemberId)) && f.Status == (byte)FriendshipStatus.Confirmed) || email == photo.MemberId) select f).Any()) ||
                          (photo.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (photo.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
             * */
            #endregion
            // concat and return:
            var acts = po_acts.OrderByDescending(act => act.TimeOfAct).Take(Digits.ActivitiesPageSize);
            return acts;
        }
        private IEnumerable<Activity> GetMemberActivitiesForHisWall(string wallowner, string viewer, DateTime? dt)
        {
            var is_auth = !String.IsNullOrEmpty(viewer);
            var is_viewer_admin = Member.IsInAdminGroup(viewer, string.Empty);
            // get post acts:
            #region Collect Wall Post Activities:
            var po_acts = from act in db.Activities.OfType<PostActivity>()
                          join post in db.SharedObjects.OfType<Post>() on act.PostId equals post.Id
                          where
                          act.MemberId == wallowner &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          (post.Status == (byte)PostStatus.Visible) &&
                          (is_viewer_admin ||
                          ((post.VisibleTo == (byte)VisibleTo.OnlyMe && post.MemberId == viewer) ||
                          (post.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == post.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == post.MemberId)) || viewer == post.MemberId) select f).Any()) ||
                          (post.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (post.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            // get photo acts:
            #region Collect Wall Photo Activities:
            var ph_acts = from act in db.Activities.OfType<PhotoActivity>()
                          join photo in db.SharedObjects.OfType<Photo>() on act.PhotoId equals photo.Id
                          where
                          act.MemberId == wallowner &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          (is_viewer_admin ||
                          ((photo.VisibleTo == (byte)VisibleTo.OnlyMe && photo.MemberId == viewer) ||
                          (photo.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == photo.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == photo.MemberId)) || viewer == photo.MemberId) select f).Any()) ||
                          (photo.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (photo.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            //get Sharing acts:
            #region Collect Wall Sharing Activities:
            var sh_acts = from act in db.Activities.OfType<SharingActivity>()
                          join sharing in db.SharedObjects.OfType<Sharing>() on act.SharingId equals sharing.Id
                          join obj in db.SharedObjects on sharing.SharedObjectId equals obj.Id
                          where
                          act.MemberId == wallowner &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          (is_viewer_admin ||
                          ((sharing.VisibleTo == (byte)VisibleTo.OnlyMe && sharing.MemberId == viewer) ||
                          (sharing.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == sharing.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == sharing.MemberId)) || viewer == sharing.MemberId) select f).Any()) ||
                          (sharing.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (sharing.VisibleTo == (byte)VisibleTo.EveryOne))) &&
                          (is_viewer_admin ||
                          ((obj.VisibleTo == (byte)VisibleTo.OnlyMe && obj.MemberId == viewer) ||
                          (obj.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == obj.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == obj.MemberId)) || viewer == obj.MemberId) select f).Any()) ||
                          (obj.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (obj.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            // get post on wall acts:
            #region Collect PostOnWall Activities:
            var pw_acts = from act in db.Activities.OfType<PostOnWallActivity>()
                          join post in db.SharedObjects.OfType<Post>() on act.PostId equals post.Id
                          where
                          act.WallOwner == wallowner &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          (post.Status == (byte)PostStatus.Visible) &&
                          (is_viewer_admin ||
                          viewer == wallowner ||
                          ((post.VisibleTo == (byte)VisibleTo.OnlyMe && post.MemberId == viewer) ||
                          (post.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == post.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == post.MemberId)) || viewer == post.MemberId) select f).Any()) ||
                          (post.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (post.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            // concat and return:
            var acts = po_acts.Concat(ph_acts).Concat(sh_acts).Concat(pw_acts).OrderByDescending(act => act.TimeOfAct).Take(Digits.ActivitiesPageSize);
            return acts;
        }
        private IEnumerable<Activity> GetProfilePageActivitis(string owner, string viewer, DateTime? dt)
        {
            var is_auth = !String.IsNullOrEmpty(viewer);
            var is_viewer_admin = Member.IsInAdminGroup(viewer, string.Empty);
            // default privacy settings:
            #region Get Default Privacy Setting Values:
            var WhoSeeLikeActivitiesDef = PrivacySetting.Default.WhoSeeLikeActivities;
            var WhoSeeCommentActivitiesDef = PrivacySetting.Default.WhoSeeCommentActivities;
            var WhoSeeFriendshipActivitiesDef = PrivacySetting.Default.WhoSeeFriendshipActivities;
            var WhoSeeChangeInfoActivitiesDef = PrivacySetting.Default.WhoSeeChangeInfoActivities;
            var WhoSeeEducationActivitiesDef = PrivacySetting.Default.WhoSeeEducationActivities;
            var WhoSeeSkillActivitiesDef = PrivacySetting.Default.WhoSeeSkillActivities;
            var WhoSeeChangeCoverActivitiesDef = PrivacySetting.Default.WhoSeeChangeCoverActivities;
            #endregion
            // get  without object activities:
            #region Collect Activities Without Shared Object:
            var nobj_acts = from act in db.Activities
                            join privacy in db.PrivacySettings on act.MemberId equals privacy.MemberId into row
                            from privacy in row.DefaultIfEmpty()
                            where
                            act.MemberId == owner &&
                            (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                                //ChangeInfoActivity
                            ((act is ChangeInfoActivity &&
                            (is_viewer_admin ||
                            (((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == viewer) ||
                            ((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == act.MemberId)) || viewer == act.MemberId) select f).Any()) ||
                            ((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                            ((privacy != null ? privacy.WhoSeeChangeInfoActivities : WhoSeeChangeInfoActivitiesDef) == (byte)VisibleTo.EveryOne)))) ||
                                //ChangeCoverActivity
                            (act is ChangeCoverActivity &&
                            (is_viewer_admin ||
                            (((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == viewer) ||
                            ((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == act.MemberId)) || viewer == act.MemberId) select f).Any()) ||
                            ((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                            ((privacy != null ? privacy.WhoSeeChangeCoverActivities : WhoSeeChangeCoverActivitiesDef) == (byte)VisibleTo.EveryOne)))) ||
                                //EducationActivity
                            (act is EducationActivity &&
                            (is_viewer_admin ||
                            (((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == viewer) ||
                            ((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == act.MemberId)) || viewer == act.MemberId) select f).Any()) ||
                            ((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                            ((privacy != null ? privacy.WhoSeeEducationActivities : WhoSeeEducationActivitiesDef) == (byte)VisibleTo.EveryOne)))) ||
                                //SkillActivity
                            (act is SkillActivity &&
                            (is_viewer_admin ||
                            (((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == viewer) ||
                            ((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == act.MemberId)) || viewer == act.MemberId) select f).Any()) ||
                            ((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                            ((privacy != null ? privacy.WhoSeeSkillActivities : WhoSeeSkillActivitiesDef) == (byte)VisibleTo.EveryOne)))))
                            select act as Activity;
            #endregion
            //get post acts:
            #region Collect Post Activities:
            var po_acts = from act in db.Activities.OfType<PostActivity>()
                          join post in db.SharedObjects.OfType<Post>() on act.PostId equals post.Id
                          where
                          act.MemberId == owner &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          (post.Status == (byte)PostStatus.Visible) &&
                          (is_viewer_admin ||
                          ((post.VisibleTo == (byte)VisibleTo.OnlyMe && post.MemberId == viewer) ||
                          (post.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == post.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == post.MemberId)) || viewer == post.MemberId) select f).Any()) ||
                          (post.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (post.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            //get photo acts:
            #region Collect Photo Activities:
            var ph_acts = from act in db.Activities.OfType<PhotoActivity>()
                          join photo in db.SharedObjects.OfType<Photo>() on act.PhotoId equals photo.Id
                          where
                          act.MemberId == owner &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          (is_viewer_admin ||
                          ((photo.VisibleTo == (byte)VisibleTo.OnlyMe && photo.MemberId == viewer) ||
                          (photo.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == photo.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == photo.MemberId)) || viewer == photo.MemberId) select f).Any()) ||
                          (photo.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (photo.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            //get sharing acts:
            #region Collect Sharing Activities:
            var sh_acts = from act in db.Activities.OfType<SharingActivity>()
                          join sharing in db.SharedObjects.OfType<Sharing>() on act.SharingId equals sharing.Id
                          join obj in db.SharedObjects on sharing.SharedObjectId equals obj.Id
                          where
                          act.MemberId == owner &&
                          (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                          (is_viewer_admin ||
                          ((sharing.VisibleTo == (byte)VisibleTo.OnlyMe && sharing.MemberId == viewer) ||
                          (sharing.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == sharing.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == sharing.MemberId)) || viewer == sharing.MemberId) select f).Any()) ||
                          (sharing.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (sharing.VisibleTo == (byte)VisibleTo.EveryOne))) &&
                          (is_viewer_admin ||
                          ((obj.VisibleTo == (byte)VisibleTo.OnlyMe && obj.MemberId == viewer) ||
                          (obj.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == obj.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == obj.MemberId)) || viewer == obj.MemberId) select f).Any()) ||
                          (obj.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                          (obj.VisibleTo == (byte)VisibleTo.EveryOne)))
                          select act as Activity;
            #endregion
            //get object like acts:
            #region Collect SharedObjectLike Activities:
            var like_acts_base = from act in db.Activities.OfType<LikeActivity>()
                                 join like in db.Likes.OfType<SharedObjectLike>() on act.LikeId equals like.Id
                                 join obj in db.SharedObjects on like.SharedObjectId equals obj.Id
                                 join privacy in db.PrivacySettings on act.MemberId equals privacy.MemberId into set1
                                 from privacy in set1.DefaultIfEmpty()
                                 where
                                 act.MemberId == owner &&
                                 (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                                 (is_viewer_admin ||
                                 (((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == viewer) ||
                                 ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == act.MemberId)) || viewer == act.MemberId) select f).Any()) ||
                                 ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                                 ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.EveryOne)))
                                 select new { Activity = act, Object = obj, SharedObject = (obj is Sharing ? (from sharing in db.SharedObjects.OfType<Sharing>() where sharing.Id == obj.Id select sharing).FirstOrDefault().SharedObject : null) };
            var like_acts = from row in like_acts_base
                            where
                            (is_viewer_admin ||
                            ((row.Object.VisibleTo == (byte)VisibleTo.OnlyMe && row.Object.MemberId == viewer) ||
                            (row.Object.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == row.Object.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == row.Object.MemberId)) || viewer == row.Object.MemberId) select f).Any()) ||
                            (row.Object.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                            (row.Object.VisibleTo == (byte)VisibleTo.EveryOne))) &&
                            (row.SharedObject == null ||
                            is_viewer_admin ||
                            ((row.SharedObject.VisibleTo == (byte)VisibleTo.OnlyMe && row.SharedObject.MemberId == viewer) ||
                            (row.SharedObject.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == row.SharedObject.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == row.SharedObject.MemberId)) || viewer == row.SharedObject.MemberId) select f).Any()) ||
                            (row.SharedObject.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                            (row.SharedObject.VisibleTo == (byte)VisibleTo.EveryOne)))
                            select row.Activity as Activity;
            #endregion
            //get comment like acts:
            #region Collect CommentLike Activities:
            var comlik_acts = from act in db.Activities.OfType<LikeActivity>()
                              join like in db.Likes.OfType<CommentLike>() on act.LikeId equals like.Id
                              join com in db.Comments on like.CommentId equals com.Id
                              join obj in db.SharedObjects on com.SharedObjectId equals obj.Id
                              join privacy in db.PrivacySettings on act.MemberId equals privacy.MemberId into set1
                              from privacy in set1.DefaultIfEmpty()
                              where
                              act.MemberId == owner &&
                              (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                              (is_viewer_admin ||
                              (((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == viewer) ||
                              ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == act.MemberId)) || viewer == act.MemberId) select f).Any()) ||
                              ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                              ((privacy != null ? privacy.WhoSeeLikeActivities : WhoSeeLikeActivitiesDef) == (byte)VisibleTo.EveryOne))) &&
                              (is_viewer_admin ||
                              ((obj.VisibleTo == (byte)VisibleTo.OnlyMe && obj.MemberId == viewer) ||
                              (obj.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == obj.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == obj.MemberId)) || viewer == obj.MemberId) select f).Any()) ||
                              (obj.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                              (obj.VisibleTo == (byte)VisibleTo.EveryOne)))
                              select act as Activity;
            #endregion
            //get comment acts:
            #region Collect Comment Activities:
            var com_acts_base = from act in db.Activities.OfType<CommentActivity>()
                                join com in db.Comments on act.CommentId equals com.Id
                                join obj in db.SharedObjects on com.SharedObjectId equals obj.Id
                                join privacy in db.PrivacySettings on act.MemberId equals privacy.MemberId into set1
                                from privacy in set1.DefaultIfEmpty()
                                where
                                act.MemberId == owner &&
                                (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                                (is_viewer_admin || (((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.OnlyMe && act.MemberId == viewer) ||
                                ((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == act.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == act.MemberId)) || viewer == act.MemberId) select f).Any()) ||
                                ((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.Members && is_auth) ||
                                ((privacy != null ? privacy.WhoSeeCommentActivities : WhoSeeCommentActivitiesDef) == (byte)VisibleTo.EveryOne)))
                                select new { Activity = act, Object = obj, SharedObject = (obj is Sharing ? (from sharing in db.SharedObjects.OfType<Sharing>() where sharing.Id == obj.Id select sharing).FirstOrDefault().SharedObject : null) };
            var com_acts = from row in com_acts_base
                           where
                            (is_viewer_admin ||
                            ((row.Object.VisibleTo == (byte)VisibleTo.OnlyMe && row.Object.MemberId == viewer) ||
                            (row.Object.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == row.Object.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == row.Object.MemberId)) || viewer == row.Object.MemberId) select f).Any()) ||
                            (row.Object.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                            (row.Object.VisibleTo == (byte)VisibleTo.EveryOne))) &&
                            (row.SharedObject == null ||
                            is_viewer_admin ||
                            ((row.SharedObject.VisibleTo == (byte)VisibleTo.OnlyMe && row.SharedObject.MemberId == viewer) ||
                            (row.SharedObject.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == row.SharedObject.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == row.SharedObject.MemberId)) || viewer == row.SharedObject.MemberId) select f).Any()) ||
                            (row.SharedObject.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                            (row.SharedObject.VisibleTo == (byte)VisibleTo.EveryOne)))
                           select row.Activity as Activity;
            #endregion
            //get change pp acts:
            #region Collect ChamgePP Activities:
            var cpp_acts = from act in db.Activities.OfType<ChangePPActivity>()
                           join photo in db.SharedObjects.OfType<Photo>() on act.PhotoId equals photo.Id
                           where
                           act.MemberId == owner &&
                           (dt.HasValue ? act.TimeOfAct < dt.Value : true) &&
                           (is_viewer_admin ||
                           ((photo.VisibleTo == (byte)VisibleTo.OnlyMe && photo.MemberId == viewer) ||
                           (photo.VisibleTo == (byte)VisibleTo.Friends && (from f in db.FriendshipRelations where (((f.MemberOneId == viewer && f.MemberTwoId == photo.MemberId) || (f.MemberTwoId == viewer && f.MemberOneId == photo.MemberId)) || viewer == photo.MemberId) select f).Any()) ||
                           (photo.VisibleTo == (byte)VisibleTo.Members && is_auth) ||
                           (photo.VisibleTo == (byte)VisibleTo.EveryOne)))
                           select act as Activity;
            #endregion
            //concat and return:
            return nobj_acts.Concat(po_acts).Concat(ph_acts).Concat(sh_acts).Concat(like_acts).Concat(comlik_acts).Concat(com_acts).Concat(cpp_acts).OrderByDescending(act => act.TimeOfAct).Take(Digits.ActivitiesPageSize);
        }
        #endregion

        public void Dispose()
        {
            db.Dispose();
        }
    }
}