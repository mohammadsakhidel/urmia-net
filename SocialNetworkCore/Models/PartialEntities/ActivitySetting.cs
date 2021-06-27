using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;

namespace SocialNetApp.Models
{
    public partial class ActivitySetting
    {
        public List<int> UnwantedActivityIds
        {
            get
            {
                List<int> list = new List<int>();
                if (!String.IsNullOrEmpty(this.UnwantedActivities))
                {
                    var splited = MyHelper.SplitString(this.UnwantedActivities, ";");
                    splited.ForEach(act => list.Add(Convert.ToInt32(act)));
                }
                return list;
            }
            set
            {
                string st = "";
                for (var i = 0; i < value.Count; i++)
                {
                    var act_st = value[i];
                    st += (i > 0 ? ";" : "") + act_st;
                }
                this.UnwantedActivities = st;
            }
        }
        public List<string> UnwantedActorIds
        {
            get
            {
                List<string> list = new List<string>();
                if (!String.IsNullOrEmpty(this.UnwantedActors))
                {
                    var splited = MyHelper.SplitString(this.UnwantedActors, ";");
                    splited.ForEach(actor => list.Add(actor));
                }
                return list;
            }
            set
            {
                string st = "";
                for (var i = 0; i < value.Count; i++)
                {
                    var act_st = value[i];
                    st += (i > 0 ? ";" : "") + act_st;
                }
                this.UnwantedActors = st;
            }
        }
        public Models.NewsFeedInforming NewsFeedInformingObj
        {
            get
            {
                return (!String.IsNullOrEmpty(this.NewsFeedInforming) ? new Models.NewsFeedInforming(this.NewsFeedInforming) : Models.NewsFeedInforming.Default);
            }
        }
        public static ActivitySetting Default
        {
            get
            {
                return new ActivitySetting { UnwantedActivities = "", UnwantedActors = "", NewsFeedInforming = Models.NewsFeedInforming.Default.GetString() };
            }
        }
    }
}