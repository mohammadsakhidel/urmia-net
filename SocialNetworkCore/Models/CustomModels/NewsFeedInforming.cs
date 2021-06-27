using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetApp.Models
{
    public class NewsFeedInforming
    {
        #region construcors:
        public NewsFeedInforming()
        {
            ViewPosts = true;
            ViewPhotos = true;
            ViewShares = true;
            ViewLikes = false;
            ViewComments = false;
            ViewChangePP = true;
            ViewChangeCover = true;
            ViewChangeInfo = true;
            ViewEducations = true;
            ViewSkills = true;
        }
        public NewsFeedInforming(string str)
        {
            if (str.Length >= 10)
            {
                ViewPosts = str[0] == '1';
                ViewPhotos = str[1] == '1';
                ViewShares = str[2] == '1';
                ViewLikes = str[3] == '1';
                ViewComments = str[4] == '1';
                ViewChangePP = str[5] == '1';
                ViewChangeCover = str[6] == '1';
                ViewChangeInfo = str[7] == '1';
                ViewEducations = str[8] == '1';
                ViewSkills = str[9] == '1';
            }
            else
            {
                var def = NewsFeedInforming.Default;
                ViewPosts = def.ViewPosts;
                ViewPhotos = def.ViewPhotos;
                ViewShares = def.ViewShares;
                ViewLikes = def.ViewLikes;
                ViewComments = def.ViewComments;
                ViewChangePP = def.ViewChangePP;
                ViewChangeCover = def.ViewChangeCover;
                ViewChangeInfo = def.ViewChangeInfo;
                ViewEducations = def.ViewEducations;
                ViewSkills = def.ViewSkills;
            }
        }
        #endregion
        #region Properties:
        public bool ViewPosts { get; set; }
        public bool ViewPhotos { get; set; }
        public bool ViewShares { get; set; }
        public bool ViewLikes { get; set; }
        public bool ViewComments { get; set; }
        public bool ViewChangePP { get; set; }
        public bool ViewChangeCover { get; set; }
        public bool ViewChangeInfo { get; set; }
        public bool ViewEducations { get; set; }
        public bool ViewSkills { get; set; }
        #endregion

        #region statics
        public static NewsFeedInforming Default
        {
            get
            {
                var def = new NewsFeedInforming();
                return def;
            }
        }
        #endregion

        #region methods:
        public string GetString()
        {
            var str =
                (this.ViewPosts ? "1" : "0") +
                (this.ViewPhotos ? "1" : "0") +
                (this.ViewShares ? "1" : "0") +
                (this.ViewLikes ? "1" : "0") +
                (this.ViewComments ? "1" : "0") +
                (this.ViewChangePP ? "1" : "0") +
                (this.ViewChangeCover ? "1" : "0") +
                (this.ViewChangeInfo ? "1" : "0") +
                (this.ViewEducations ? "1" : "0") +
                (this.ViewSkills ? "1" : "0");
            return str;
        }
        #endregion
    }
}