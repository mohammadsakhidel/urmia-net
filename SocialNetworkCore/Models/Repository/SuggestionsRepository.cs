using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models.Repository
{
    public class SuggestionsRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public SuggestionsRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public SuggestionsRepository(System.Data.Objects.ObjectContext context)
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
        public List<string> FindSuggestedEducationBranch(string txt, int count)
        {
            var q = (from e in db.Educations
                    where e.EducationBranch.StartsWith(txt)
                    orderby e.EducationBranch
                    select e.EducationBranch).Distinct().Take(count);
            return q.ToList();
        }
        public List<string> FindSuggestedEducationLocation(string txt, int count)
        {
            var q = (from e in db.Educations
                     where e.EducationLocation.StartsWith(txt)
                     orderby e.EducationLocation
                     select e.EducationLocation).Distinct().Take(count);
            return q.ToList();
        }
        public List<string> FindSuggestedSkill(string txt, int count)
        {
            var q = (from s in db.Skills
                     where s.SkillTitle.StartsWith(txt)
                     orderby s.SkillTitle
                     select s.SkillTitle).Distinct().Take(count);
            return q.ToList();
        }
        public List<string> FindSuggestedLivingCities(string txt, int count)
        {
            txt = txt.ToLower();
            var q1 = (from bi in db.BasicInformations
                      where bi.LivingCity.ToLower().StartsWith(txt)
                      orderby bi.LivingCity
                      select bi.LivingCity);
            var q2 = (from bi in db.BasicInformations
                      where !bi.LivingCity.ToLower().StartsWith(txt) && bi.LivingCity.ToLower().Contains(txt)
                      orderby bi.LivingCity
                      select bi.LivingCity);
            return q1.Concat(q2).Distinct().Take(count).ToList();
        }
        public List<string> FindSuggestedTags(string txt, int count)
        {
            var q = (from tag in db.SharedObjectTags
                     where tag.Tag.StartsWith(txt)
                     orderby tag.Tag ascending
                     select tag.Tag).Distinct().Take(count);
            return q.ToList();
        }
        public IEnumerable<Member> FindRecommendedPeople(string memberId)
        {
            var mr = new MembersRepository(db);
            // exceptions for suggest:
            var admins = mr.GetAdminNames();
            var except = mr.FindFriendNames(memberId).Concat(admins).ToList();
            except.Add(memberId);
            //
            var members = mr.GetRandomActiveMembers(Digits.MaxRecommendationsCount, true, except);
            return members;
        }
        public IEnumerable<Member> FindRecommendedOnlines(string memberId)
        {
            var mr = new MembersRepository(db);
            var admins = mr.GetAdminNames();
            var onlines = mr.GetOnlineMembers().Where(o => !admins.Contains(o.Email) && o.Email != memberId).OrderByDescending(o => o.RegistrationDate).Take(Digits.MaxRecommendationsCount);
            return onlines;
        }
        public IEnumerable<Member> FindProfileViewers(string memberId)
        {
            var recs = (from pvg in
                           (from pv in db.ProfileViews
                            where pv.ProfileOwnerId == memberId
                            group pv by pv.ViewerId into g
                            select g.OrderByDescending(p => p.DateOfView).FirstOrDefault())
                       join m in db.Members
                       on pvg.ViewerId equals m.Email
                       where m.StatusCode == (byte)MemberStatus.Active
                       orderby pvg.DateOfView descending
                       select m).Take(Digits.MaxRecommendationsCount);
            return recs;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}