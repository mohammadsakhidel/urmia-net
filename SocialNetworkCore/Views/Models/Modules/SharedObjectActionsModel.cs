using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;

namespace SocialNetApp.Views.Models.Modules
{
    public class SharedObjectActionsModel
    {
        #region Properties:
        public string RandomId { get; set; }
        public SharedObject SharedObject { get; set; }
        public List<CommentModel> LastCommentModels { get; set; }
        public bool HasUserLikedObject { get; set; }
        public bool IsCommentingAllowedForUser { get; set; }
        public int SharesCount { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public string LikesCountDisplayText { get; set; }
        #endregion

        #region Methods:
        public static SharedObjectActionsModel Create(SharedObject obj, System.Data.Objects.ObjectContext context)
        {
            var sr = new SharedObjectsRepository(context);
            context = sr.Context;
            var cr = new CommentsRepository(context);
            //---- increase shared object visit counter:
            sr.IncreaseVisitsCounter(obj.Id);
            //----
            var obj_owner = obj.Member;
            ///////////////////////////////////////////
            var model = new SharedObjectActionsModel();
            model.SharedObject = obj;
            model.RandomId = model.SharedObject.Id + "_" + MyHelper.GetRandomString(8, true);
            model.HasUserLikedObject = HttpContext.Current.User.Identity.IsAuthenticated ?
                sr.HasLiked(model.SharedObject.Id, HttpContext.Current.User.Identity.Name) : 
                false;
            model.IsCommentingAllowedForUser = HttpContext.Current.User.Identity.IsAuthenticated ?
                model.SharedObject.IsAllowedToComment(HttpContext.Current.User.Identity.Name, context) &&
                obj_owner.IsAllowedToLeaveComment(HttpContext.Current.User.Identity.Name) :
                false;
            model.SharesCount = HttpContext.Current.User.Identity.IsAuthenticated ?
                (model.SharedObject is Sharing ? ((Sharing)model.SharedObject).SharedObject.Sharings.Count() : model.SharedObject.Sharings.Count()) :
                0;
            model.CommentsCount = cr.GetCommentsCount(model.SharedObject.Id);
            model.LikesCount = model.SharedObject.Likes.Count();
            model.LikesCountDisplayText = (model.HasUserLikedObject ?
                (model.LikesCount <= 1 ? Resources.Words.YouLikeThis : Resources.Words.YouAnd + " " + (model.LikesCount - 1).ToString() + " " + Resources.Words.PeopleElseLikeThis) :
                model.LikesCount.ToString() + " " + Resources.Words.PeopleLikeThis);
            //comment module models:
            var lastComments = cr.FindLastComments(model.SharedObject.Id, model.CommentsCount);
            var lastCommentModels = new List<CommentModel>();
            foreach (var comm in lastComments)
            {
                var commentModel = CommentModel.Create(comm, context);
                lastCommentModels.Add(commentModel);
            }
            //
            model.LastCommentModels = lastCommentModels;
            return model;
        }
        #endregion
    }
}