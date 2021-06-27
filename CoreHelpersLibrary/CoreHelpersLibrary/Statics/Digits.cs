using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public class Digits
    {
        public static int BeginYear
        {
            get
            {
                if (Configs.DateType == DateType.Shamsi)
                    return 1300;
                else
                    return 1920;
            }
        }
        public static int EndYear
        {
            get
            {
                if (Configs.DateType == DateType.Shamsi)
                    return ShamsiDateTime.MiladyToShamsi(MyHelper.Now).Year;
                else
                    return MyHelper.Now.Year;
            }
        }
        public static int EndYearForFuture
        {
            get
            {
                if (Configs.DateType == DateType.Shamsi)
                    return ShamsiDateTime.MiladyToShamsi(MyHelper.Now).Year + 10;
                else
                    return MyHelper.Now.Year + 10;
            }
        }
        public static int LockedOutHours
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "LockedOutHours"));
            }
        }
        public static int CoverWidth
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "CoverWidth"));
            }
        }
        public static int CoverHeight
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "CoverHeight"));
            }
        }
        public static int PhotoFixedWidth
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PhotoFixedWidth"));
            }
        }
        public static int LargeThumbSides
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "LargeThumbSides"));
            }
        }
        public static int SmallThumbSides
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "SmallThumbSides"));
            }
        }
        public static int MaximumPhotoBytes
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaximumPhotoBytes"));
            }
        }
        public static int MaximumVideoBytes
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaximumVideoBytes"));
            }
        }
        public static int PostsLargeThumbWidth
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PostsLargeThumbWidth"));
            }
        }
        public static int PostsLargeThumbHeight
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PostsLargeThumbHeight"));
            }
        }
        public static int PostsSmallThumbSides
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PostsSmallThumbSides"));
            }
        }
        public static int MaxPostPhotos
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxPostPhotos"));
            }
        }
        public static int MaxPostTextLength
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxPostTextLength"));
            }
        }
        public static int MaxCommentTextLength
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxCommentTextLength"));
            }
        }
        public static int ActivitiesPageSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "ActivitiesPageSize"));
            }
        }
        public static int ShowMoreLength
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "ShowMoreLength"));
            }
        }
        public static int MaxCommentsToShow
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxCommentsToShow"));
            }
        }
        public static int CommentsPageSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "CommentsPageSize"));
            }
        }
        public static int MaxPhotoActivityHeight
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxPhotoActivityHeight"));
            }
        }
        public static int MaxDaysOfAccountDeleteCanceling
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxDaysOfAccountDeleteCanceling"));
            }
        }
        public static int MaxHmiItems
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxHmiItems"));
            }
        }
        public static int MaxAutoCompleteItems
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxAutoCompleteItems"));
            }
        }
        public static int ProfilePageFriendsCount
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "ProfilePageFriendsCount"));
            }
        }
        public static int ListingItemsPageSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "ListingItemsPageSize"));
            }
        }
        public static int PixelOfHomeHeight
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PixelOfHomeHeight"));
            }
        }
        public static int PixelOfHomePageHeight
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PixelOfHomePageHeight"));
            }
        }
        public static int PixelOfProfileHeight
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PixelOfProfileHeight"));
            }
        }
        public static int MaxRecommendationsCount
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxRecommendationsCount"));
            }
        }
        public static int OnlineWindowMinutes
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "OnlineWindowMinutes"));
            }
        }
        public static int ChatSessionWindowMinutes
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "ChatSessionWindowMinutes"));
            }
        }
        public static int MaxChatMessageLength
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxChatMessageLength"));
            }
        }
        public static int MaxMessageLength
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxMessageLength"));
            }
        }
        public static int HomeMemberCollectionSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "HomeMemberCollectionSize"));
            }
        }
        public static int WelcomeMessageWindowDays
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "WelcomeMessageWindowDays"));
            }
        }
        public static int SearchMembersPageSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "SearchMembersPageSize"));
            }
        }
        public static int SearchMembersDefaultSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "SearchMembersDefaultSize"));
            }
        }
        public static int PeoplePageSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PeoplePageSize"));
            }
        }
        public static int MaxChatSessionMessages
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxChatSessionMessages"));
            }
        }
        public static int PixelAdvBlockSides
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "PixelAdvBlockSides"));
            }
        }
        public static int MaximumAlsoCommentReceivers
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaximumAlsoCommentReceivers"));
            }
        }
        public static int ConversationsPageSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "ConversationsPageSize"));
            }
        }
        public static int ConversationMessagesPageSize
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "ConversationMessagesPageSize"));
            }
        }
        public static int MaximumAlbumCount
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaximumAlbumCount"));
            }
        }
        public static int MaxVideosPerPost
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "MaxVideosPerPost"));
            }
        }
        public static int VideosFixedWidth
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "VideosFixedWidth"));
            }
        }
        public static int VideosFixedHeight
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "VideosFixedHeight"));
            }
        }
        public static int VideosDisplayWidth
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "VideosDisplayWidth"));
            }
        }
        public static int EmailNotificationWindowHours
        {
            get
            {
                return Convert.ToInt32(Statics.GetValue("numbers", "EmailNotificationWindowHours"));
            }
        }
    }
}