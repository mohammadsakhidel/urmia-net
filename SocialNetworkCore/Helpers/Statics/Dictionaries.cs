using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class Dictionaries
    {
        public static Dictionary<int, string> Months
        {
            get
            {
                Dictionary<int, string> months = new Dictionary<int, string>();
                months.Add(1, Resources.Words.Month1);
                months.Add(2, Resources.Words.Month2);
                months.Add(3, Resources.Words.Month3);
                months.Add(4, Resources.Words.Month4);
                months.Add(5, Resources.Words.Month5);
                months.Add(6, Resources.Words.Month6);
                months.Add(7, Resources.Words.Month7);
                months.Add(8, Resources.Words.Month8);
                months.Add(9, Resources.Words.Month9);
                months.Add(10, Resources.Words.Month10);
                months.Add(11, Resources.Words.Month11);
                months.Add(12, Resources.Words.Month12);
                return months;
            }
        }
        public static Dictionary<int, string> EducationLevels
        {
            get
            {
                Dictionary<int, string> months = new Dictionary<int, string>();
                months.Add(1, Resources.Words.EduLevel_Diploma);
                months.Add(2, Resources.Words.EduLevel_FogheDiploma);
                months.Add(3, Resources.Words.EduLevel_Bachelor);
                months.Add(4, Resources.Words.EduLevel_FogheBachelor);
                months.Add(5, Resources.Words.EduLevel_DrAndHigher);
                return months;
            }
        }
        public static Dictionary<int, string> Heights
        {
            get
            {
                Dictionary<int, string> heights = new Dictionary<int, string>();
                heights.Add(1, Resources.Words.LessThan + "135");
                heights.Add(2, "135-140");
                heights.Add(3, "140-145");
                heights.Add(4, "145-150");
                heights.Add(5, "150-155");
                heights.Add(6, "155-160");
                heights.Add(7, "160-165");
                heights.Add(8, "165-170");
                heights.Add(9, "170-175");
                heights.Add(10, "175-180");
                heights.Add(11, "180-185");
                heights.Add(12, "185-190");
                heights.Add(13, "190-195");
                heights.Add(14, "195-200");
                heights.Add(15, Resources.Words.MoreThan + "200");
                return heights;
            }
        }
        public static Dictionary<int, string> Weights
        {
            get
            {
                Dictionary<int, string> weights = new Dictionary<int, string>();
                weights.Add(1, Resources.Words.LessThan + "40");
                weights.Add(2, "40-45");
                weights.Add(3, "45-50");
                weights.Add(4, "50-55");
                weights.Add(5, "55-60");
                weights.Add(6, "60-65");
                weights.Add(7, "65-70");
                weights.Add(8, "70-75");
                weights.Add(9, "75-80");
                weights.Add(10, "80-85");
                weights.Add(11, "85-90");
                weights.Add(12, "90-95");
                weights.Add(13, "95-100");
                weights.Add(14, Resources.Words.MoreThan + "100");
                return weights;
            }
        }
        public static Dictionary<int, string> Fashions
        {
            get
            {
                Dictionary<int, string> fashions = new Dictionary<int, string>();
                fashions.Add(0, Resources.Words.Comfortable);
                fashions.Add(1, Resources.Words.Jean);
                fashions.Add(2, Resources.Words.Suit);
                fashions.Add(3, Resources.Words.Usual);
                fashions.Add(4, Resources.Words.Modern);
                fashions.Add(5, Resources.Words.UpToDate);
                fashions.Add(6, Resources.Words.Variable);
                return fashions;
            }
        }
        public static Dictionary<string, string> Emoticons
        {
            get
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                //angel:
                list.Add(":angel:", "angel");
                list.Add(":agl:", "angel");
                list.Add(":angl:", "angel");
                list.Add(":angle:", "angel");
                list.Add("O:)", "angel");
                list.Add("(:O", "angel");
                //angry:
                list.Add(":angry:", "angry");
                list.Add(":ang:", "angry");
                list.Add(">:O", "angry");
                list.Add("O:<", "angry");
                //smile:
                list.Add(":big-smile:", "big-smile");
                list.Add(":bsm:", "big-smile");
                list.Add(":bsmile:", "big-smile");
                list.Add(":D", "big-smile");
                list.Add("D:", "big-smile");
                // broken heart:
                list.Add(":broken-heart:", "broken-heart");
                list.Add(":bht:", "broken-heart");
                //confused:
                list.Add(":confused:", "confused");
                list.Add(":con:", "confused");
                list.Add("o.O", "confused");
                list.Add("O.o", "confused");
                //cry:
                list.Add(":crying:", "crying");
                list.Add(":cry:", "crying");
                list.Add(":'(", "crying");
                list.Add(")':", "crying");
                // curly lips:
                list.Add(":curly-lips:", "curly-lips");
                list.Add(":cur:", "curly-lips");
                list.Add(":3", "curly-lips");
                list.Add("3:", "curly-lips");
                //devil:
                list.Add(":devil:", "devil");
                list.Add(":dev:", "devil");
                list.Add("3:)", "devil");
                list.Add("(:3", "devil");
                //dislike
                list.Add(":dislike:", "dislike");
                list.Add(":dis:", "dislike");
                //eyes-wide:
                list.Add(":eyes-wide:", "eyes-wide-open");
                list.Add(":eyw:", "eyes-wide-open");
                //gift:
                list.Add(":gift:", "gift");
                list.Add(":gif:", "gift");
                //glasses:
                list.Add(":glasses:", "glasses");
                list.Add(":gls:", "glasses");
                list.Add("B)", "glasses");
                list.Add("(B", "glasses");
                //heart:
                list.Add(":heart:", "heart");
                list.Add(":hrt:", "hrt");
                list.Add("<3", "hrt");
                list.Add("3>", "hrt");
                //heart eyes:
                list.Add(":heart-eyes:", "heart-eyes");
                list.Add(":hty:", "heart-eyes");
                //heart gift:
                list.Add(":heart-gift:", "heart-gift");
                list.Add(":hgf:", "heart-gift");
                //heart stabbed:
                list.Add(":heart-stab:", "heart-stabbed");
                list.Add(":hst:", "heart-stabbed");
                //kiki:
                list.Add(":kiki:", "kiki");
                list.Add(":kik:", "kiki");
                list.Add("^_^", "kiki");
                //kiss:
                list.Add(":kiss:", "kiss");
                list.Add(":kis:", "kiss");
                list.Add(":*", "kiss");
                list.Add("*:", "kiss");
                //like:
                list.Add(":like:", "like");
                list.Add(":lik:", "like");
                // mocking tongue:
                list.Add(":moc-tongue:", "mocking-tongue-out");
                list.Add(":mtn:", "mocking-tongue-out");
                // pacman:
                list.Add(":pacman:", "pacman");
                list.Add(":pac:", "pacman");
                list.Add(":v", "pacman");
                list.Add("v:", "pacman");
                // phone:
                list.Add(":phone:", "phone");
                list.Add(":pho:", "phone");
                // poop:
                list.Add(":poop:", "poop");
                list.Add(":pop:", "poop");
                // sad:
                list.Add(":sad:", "sad");
                list.Add(":(", "sad");
                list.Add("):", "sad");
                // shark:
                list.Add(":shark:", "shark");
                list.Add(":shk:", "shark");
                list.Add("(^^^)", "shark");
                // skull:
                list.Add(":skull:", "skull");
                list.Add(":skl:", "skull");
                //smile:
                list.Add(":smile:", "smiley");
                list.Add(":sml:", "smiley");
                list.Add(":)", "smiley");
                list.Add("(:", "smiley");
                //squint:
                list.Add(":squint:", "squint");
                list.Add(":sqn:", "squint");
                //sun:
                list.Add(":sun:", "sun");
                //sunglasses:
                list.Add(":sunglasses:", "sunglasses");
                list.Add(":sng:", "sunglasses");
                list.Add("B|", "sunglasses");
                list.Add("|B", "sunglasses");
                //surprised:
                list.Add(":surprised:", "surprised");
                list.Add(":sur:", "surprised");
                list.Add(":O", "surprised");
                list.Add("O:", "surprised");
                //tngue out:
                list.Add(":tongue:", "tongue-out");
                list.Add(":tng:", "tongue-out");
                list.Add(":P", "tongue-out");
                list.Add("P:", "tongue-out");
                //unsure
                list.Add(":unsure:", "unsure");
                list.Add(":uns:", "unsure");
                list.Add("/:", "unsure");
                //wink:
                list.Add(":wink:", "wink");
                list.Add(":wnk:", "wink");
                list.Add(";)", "wink");
                list.Add("(;", "wink");
                //winking-tongue-out:
                list.Add(":wink-tongue:", "winking-tongue-out");
                list.Add(":wtg:", "winking-tongue-out");
                list.Add(";P", "winking-tongue-out");
                list.Add("P;", "winking-tongue-out");
                return list;
            }
        }
    }
}