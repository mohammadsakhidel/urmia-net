using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreHelpers
{
    public enum HeaderType
    {
        Slim = 1,
        Thick = 2
    }

    public enum LoginType
    {
        OnHeader = 1,
        InContent = 2
    }

    public enum RegisterType
    {
        OnHome = 1,
        OnItsPage = 2
    }

    public enum DateType
    {
        Milady = 1,
        Shamsi = 2
    }

    public enum MaritalStatus
    {
        None = -1,
        Single = 1,
        Married = 2,
        InRelationship = 3,
        Divorced = 4
    }

    public enum VisibleTo
    {
        OnlyMe = 1,
        Friends = 2,
        Members = 3,
        EveryOne = 4
    }

    public enum AllowCommentTo
    {
        Friends = 1,
        Members = 2,
        OnlyMe = 3
    }

    public enum UploadStatus
    {
        Done = 1,
        NotSupportedFormat = 2,
        Exception = 3,
        LargerThanMaximum = 4
    }

    public enum PostStatus
    {
        InComplete = 1,
        Unvisible = 2,
        Visible = 3
    }

    public enum PostType
    {
        NormalPost = 1,
        WallPost = 2,
        SpecialPost = 3
    }

    public enum ActivityType
    {
        None = 0,
        PostActivity = 1,
        PhotoActivity = 2,
        ChangeInfoActivity = 3,
        ChangeCoverActivity = 4,
        //FriendshipActivity = 5,
        CommentActivity = 6,
        LikeActivity = 7,
        SharingActivity = 8,
        EducationActivity = 9,
        SkillActivity = 10,
        PostOnWallActivity = 11,
        ChangePPActivity = 12
    }

    public enum FriendshipStatus
    {
        Requested = 1,
        Confirmed = 2,
        Rejected = 3,
        Broken = 4,
        None = 5
    }

    public enum UserIdentityType
    {
        Thumb = 1,
        FullName = 2,
        ThumbAndFullName = 3
    }

    public enum SharedObjectType
    {
        None = 0,
        Post = 1,
        Album = 2,
        AlbumPhoto = 3,
        PostPhoto = 4,
        Sharing = 5,
        ProfileCoverPhoto = 6
    }

    public enum ChangeCoverActivityType
    {
        CoverChanged = 1,
        ProfilePhotoChanged = 2
    }

    public enum DeleteScheduleStatus
    {
        Scheduled = 1,
        Canceled = 2
    }

    public enum MemberStatus
    {
        RegisteredNotActivated = 1,
        Active = 2,
        ScheduledForDelete = 3
    }

    public enum LoginStatus
    {
        WrongPassword = 1,
        BlockedAccount = 2,
        UserNotApproved = 3,
        UserLockedOut = 4,
        DeletedAccount = 5,
        Successful = 6,
        UserNotExists = 7,
        NotValidAttempt = 8
    }

    public enum InboxMessageStatus
    {
        Unread = 1,
        Read = 2
    }

    public enum OutboxMessageStatus
    {
        Sent = 1,
        Delivered = 2
    }

    public enum ConversationVisibility
    {
        Visible = 1,
        VisibleForMember1 = 2,
        VisibleForMember2 = 3,
        Invisible = 4
    }

    public enum ChatMessageStatus
    {
        New = 1,
        ShowedToReciever = 2
    }

    public enum CommentNotificationType
    {
        Normal = 1,
        AlsoComment = 2
    }

    public enum NotificationStatus
    {
        Unread = 1,
        Read = 2
    }

    public enum SpecialPostShowMethod
    {
        Welcome = 1,
        Pinned = 2,
        RandomInActivities = 3
    }

    public enum PixelAdvType
    {
        Free = 1,
        Paid = 2
    }

    public enum PixelAdvPaymentStatus
    {
        PaidInCash = 1,
        PaidInCheque = 2,
        NotPaid = 3
    }

    public enum ActivityVisibilityCheckLevel
    {
        None = 1,
        CheckPrivacy = 2,
        CheckObject = 3,
        CheckBoth = 4
    }

    public enum NotifySignalType
    {
        NewMessage = 1,
        NewRequest = 2,
        NewNotification = 3
    }

    public enum SpamFilterResultTypes
    {
        Json = 1,
        ThrowException = 2
    }

    public enum FormAction
    {
        New = 1,
        Edit = 2
    }

    public enum LivingRegion
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4,
        Abroad = 5
    }

    public enum FacebookConnectStep
    {
        Begin = 1,
        LoginIfHasAccount = 2,
        ConnectAuthenticated = 3,
        ConnectAnonymous = 4
    }

    public enum FacebookVisibleTo
    {
        OnlyMe = 1,
        Friends = 2,
        FriendsOfFriends = 3,
        EveryOne = 4
    }

    public enum VideoProcessResult
    {
        NotReported = 1,
        Successfull = 2,
        Failed = 3
    }
}