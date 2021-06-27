using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetApp.Models.Repository;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Views.Models.Modules;
using System.Web.Script.Serialization;
using Facebook;
using System.Dynamic;

namespace SocialNetApp.Controllers
{
    public class ObjectsController : MainController
    {
        #region Get Actions:
        [HttpGet]
        [AcceptAjax]
        public ActionResult ShowObjectLikesDialog(int id)
        {
            using (var sr = new SharedObjectsRepository())
            {
                var likes = sr.GetLikes(id);
                var model = LikesDialogModel.Create(likes, sr.Context);
                Response.CacheControl = "no-cache";
                return PartialView(Urls.ModuleViews + "_LikesDialog.cshtml", model);
            }
        }

        [HttpGet]
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult ShowShareDialog(int id, string randomId)
        {
            using (var sr = new SharedObjectsRepository())
            {
                var obj = sr.Get(id);
                Response.CacheControl = "no-cache";
                var sharedObject = (obj is Sharing ? ((Sharing)obj).SharedObject : obj);
                var model = ShareDialogModel.Create(sharedObject, randomId, sr.Context);
                return PartialView(Urls.ModuleViews + "_ShareDialog.cshtml", model);
            }
        }

        [HttpGet]
        [AcceptAjax]
        public ActionResult ShowFacebookShareDialog(int id)
        {
            System.Threading.Thread.Sleep(2500);
            using (var sr = new SharedObjectsRepository())
            {
                var obj = sr.Get(id);
                Response.CacheControl = "no-cache";
                var sharedObject = (obj is Sharing ? ((Sharing)obj).SharedObject : obj);
                var model = FacebookShareDialogModel.Create(sharedObject, sr.Context);
                return PartialView(Urls.ModuleViews + "_FacebookShareDialog.cshtml", model);
            }
        }

        [HttpGet]
        public ActionResult ObjectDetails(int id)
        {
            using (var or = new SharedObjectsRepository())
            {
                var obj = or.Get(id);
                Response.CacheControl = "no-cache";
                var model = SocialNetApp.Views.Models.Objects.ObjectDetailsViewModel.Create(obj, or.Context);
                return View("ObjectDetails", model);
            }
        }

        [HttpGet]
        [AcceptAjax]
        public ActionResult GetSharedObjectDetails(int id, int def)
        {
            using (var sr = new SharedObjectsRepository())
            {
                var obj = sr.Get(id);
                Response.CacheControl = "no-cache";
                return PartialView(Urls.ModuleViews + "_SharedObjectDetails.cshtml", 
                    SharedObjectDetailsModel.Create(obj, def, sr.Context));
            }
        }
        #endregion

        #region POST Actions:
        [HttpPost]
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult ReshareObject(FormCollection form)
        {
            try
            {
                using (var or = new SharedObjectsRepository())
                {
                    var objId = Convert.ToInt32(form["SharedObjectId"]);
                    var obj = or.Get(objId);
                    // has user shared it before?
                    if (or.HasShared(User.Identity.Name, objId))
                        throw new Exception(Resources.Messages.HasSharedBefore);
                    // is sharer the same as owner?
                    if (User.Identity.Name == obj.MemberId)
                        throw new Exception(Resources.Messages.YouCantShareYourObjects);
                    // create object:
                    var sharing = new Sharing();
                    sharing.SharedObjectId = objId;
                    sharing.MemberId = User.Identity.Name; //shared by person
                    sharing.VisibleTo = Convert.ToByte(form["VisibleTo"]);
                    sharing.AllowCommentTo = Convert.ToByte(form["AllowCommentTo"]);
                    sharing.DateOfAdd = MyHelper.Now;
                    sharing.VisitsCounter = 0;
                    // save:
                    or.Insert(sharing);
                    //
                    return Json(new { Done = true, SharesCount = or.GetSharesCount(objId) });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult ShareObjectOnFacebook(FormCollection form)
        {
            try
            {
                using (var or = new SharedObjectsRepository())
                {
                    JsonObject res = null;
                    var userId = form["FacebookUserId"];
                    var accessToken = form["FacebookAccessToken"];
                    var visibleTo = (FacebookVisibleTo)Convert.ToByte(form["VisibleTo"]);
                    //--- share:
                    var obj = or.Get(Convert.ToInt32(form["SharedObjectId"]));
                    #region Post sharing with fb:
                    if (obj is Post)
                    {
                        var post = (Post)obj;
                        if (post.Photos.Any())
                        {
                            if (post.Photos.Count == 1)
                            {
                                res = FacebookHelper.ShareAsPhotoPost(
                                    (Photo)post.Photos.First(),
                                    post.Text,
                                    visibleTo,
                                    userId,
                                    accessToken);
                            }
                            else if (post.Photos.Count > 1)
                            {
                                res = FacebookHelper.ShareAsAlbumPost(
                                    post.Photos.Cast<Photo>().ToList(), 
                                    post.Text,
                                    visibleTo,
                                    userId, 
                                    accessToken);
                            }
                        }
                        else if (post.PostVideos.Any())
                        {
                            res = FacebookHelper.ShareAsVideoPost(
                                    (PostVideo)post.PostVideos.First(),
                                    post.Text,
                                    visibleTo,
                                    userId,
                                    accessToken);
                        }
                        else
                        {
                            res = FacebookHelper.ShareAsTextPost(
                                post.Text,
                                visibleTo,
                                userId,
                                accessToken);
                        }
                    }
                    #endregion
                    #region photo sharing:
                    if (obj is Photo)
                    {
                        var photo = (Photo)obj;
                        res = FacebookHelper.ShareAsPhotoPost(photo,
                            (photo is AlbumPhoto ? ((AlbumPhoto)photo).Description : ""),
                            visibleTo,
                            userId,
                            accessToken);
                    }
                    #endregion
                    //---
                    return Json(new { 
                        Done = true,
                        Message = Resources.Messages.FacebookShareSuccessfullMessage
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }
        #endregion
    }
}
