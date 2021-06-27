using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using CoreHelpers;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Controllers
{
    public class CommentsController : MainController
    {
        #region GET Actions:
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult ShowDeleteCommentDialog(int id)
        {
            try
            {
                using (var cr = new CommentsRepository())
                {
                    var comment = cr.Get(id);
                    var model = DeleteCommentDialogModel.Create(comment, cr.Context);
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_DeleteCommentDialog.cshtml", model);
                }
            }
            catch(Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ex);
            }
        }

        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult ShowEditCommentDialog(int id)
        {
            try
            {
                using (var cr = new CommentsRepository())
                {
                    var comment = cr.Get(id);
                    var model = EditCommentDialogModel.Create(comment, cr.Context);
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_EditCommentDialog.cshtml", model);
                }
            }
            catch(Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ex);
            }
        }

        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult ShowCommentLikesDialog(int id)
        {
            try
            {
                using (var cr = new CommentsRepository())
                {
                    var likes = cr.GetLikes(id);
                    var model = LikesDialogModel.Create(likes, cr.Context);
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_LikesDialog.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ex);
            }
        }
        #endregion

        #region POST Actions:
        [AcceptAjax]
        [HttpPost]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult AddComment(FormCollection form)
        {
            var randomId = form["RandomId"];
            try
            {
                using (var cr = new CommentsRepository())
                using (var sr = new SharedObjectsRepository(cr.Context))
                {
                    var objId = Convert.ToInt32(form["SharedObjectId"]);
                    // is allowed to comment?
                    var objOwner = sr.GetOwner(objId);
                    if (!objOwner.IsAllowedToLeaveComment(User.Identity.Name))
                        throw new Exception(Resources.Messages.NotAllowedToLeaveComment);
                    //
                    var comment = new Comment();
                    comment.SharedObjectId = objId;
                    comment.MemberId = User.Identity.Name;
                    comment.Text = form["Comment"];
                    comment.DateOfComment = MyHelper.Now;
                    // is valid?
                    if (!comment.IsValid)
                        return Json(new { Done = false, SharedObjectId = objId, Errors = comment.ValidationErrors.Select(ve => ve.Value).ToArray() });
                    // save:
                    cr.Insert(comment);
                    // return:
                    var count = cr.GetCommentsCount(objId);
                    return Json(new
                    {
                        Done = true,
                        Info = randomId,
                        Message = Resources.Messages.SuccessfullyAddedComment,
                        CommentsCount = count,
                        PartialView = RenderViewToString(CoreHelpers.Urls.ModuleViews + "_Comment.cshtml", CommentModel.Create(comment, cr.Context))
                    });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Info = randomId, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult GetMoreComments(FormCollection form)
        {
            var randomId = form["RandomId"];
            try
            {
                using (var cr = new CommentsRepository())
                {
                    var objId = Convert.ToInt32(form["SharedObjectId"]);
                    var dateOfComment = Convert.ToDateTime(form["DateOfComment"]);
                    // get next page comments:
                    var comments = cr.GetComments(objId, dateOfComment);
                    // get html to return:
                    var comments_html = "";
                    foreach (var c in comments)
                    {
                        comments_html += RenderViewToString(Urls.ModuleViews + "_Comment.cshtml", CommentModel.Create(c, cr.Context));
                    }
                    return Json(new { Done = true, RandomId = randomId, PartialView = comments_html, DateOfFirstComment = comments.First().DateOfComment.ToString("yyyy-MM-dd HH:mm:ss.fff") });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, RandomId = randomId, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult LikeComment(int id)
        {
            try
            {
                using (var cr = new CommentsRepository())
                {
                    cr.Like(id, User.Identity.Name);
                    return Json(new { Done = true });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult DeleteComment(int id)
        {
            try
            {
                using (var cr = new CommentsRepository())
                {
                    cr.Delete(id);
                    return Json(new { Done = true, CommentId = id });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult EditComment(FormCollection form)
        {
            try
            {
                using (var cr = new CommentsRepository())
                using (var sr = new SharedObjectsRepository(cr.Context))
                {
                    var id = Convert.ToInt32(form["CommentId"]);
                    var comment = cr.Get(id);
                    // is allowed to comment?
                    var objOwner = sr.GetOwner(comment.SharedObjectId);
                    if (!objOwner.IsAllowedToLeaveComment(User.Identity.Name))
                        throw new Exception(Resources.Messages.NotAllowedToLeaveComment);
                    //
                    comment.Text = form["CommentText"];
                    cr.Save();
                    return Json(new { Done = true, CommentId = id, CommentText = TextsProcessor.RenderText(Server.HtmlEncode(comment.Text)) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }
        #endregion

        #region PRIVATE METHODS:
        #endregion
    }
}
