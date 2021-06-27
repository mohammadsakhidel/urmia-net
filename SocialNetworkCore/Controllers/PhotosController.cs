using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using System.IO;
using SocialNetApp.Views.Models.Modules;

namespace SocialNetApp.Controllers
{
    [Authorize(Roles = MyRoles.Member)]
    public class PhotosController : MainController
    {
        #region Get Actions:
        [HttpGet]
        public ActionResult CreateAlbum()
        {
            using (var mr = new MembersRepository())
            {
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_AlbumEditor.cshtml", AlbumEditorModel.Create(null, FormAction.New, mr.Context));
                }
                else
                {
                    // ************** get current member:
                    var member = mr.Get(User.Identity.Name);
                    var aeModel = AlbumEditorModel.Create(null, FormAction.New, mr.Context);
                    // ************** gather homepage modules and return view:
                    var model = SocialNetApp.Views.Models.HomePage.IndexViewModel.Create(member, "CreateAlbum", aeModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        [HttpGet]
        public ActionResult EditAlbum(int id)
        {
            using (var mr = new MembersRepository())
            using (var pr = new AlbumsRepository(mr.Context))
            {
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_AlbumEditor.cshtml", 
                        AlbumEditorModel.Create(pr.GetAlbum(id), FormAction.Edit, mr.Context));
                }
                else
                {
                    // ************** get current member:
                    var member = mr.Get(User.Identity.Name);
                    var aeModel = AlbumEditorModel.Create(pr.GetAlbum(id), FormAction.Edit, mr.Context);
                    // ************** gather homepage modules and return view:
                    var model = SocialNetApp.Views.Models.HomePage.IndexViewModel.Create(member, "EditAlbum", aeModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }

        [HttpGet]
        public ActionResult AlbumPhotos(int id)
        {
            using (var mr = new MembersRepository())
            using (var pr = new AlbumsRepository(mr.Context))
            {
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    var albumPhotosModel = AlbumPhotosModel.Create(pr.GetAlbum(id), TempData, mr.Context);
                    return PartialView(Urls.ModuleViews + "_AlbumPhotos.cshtml", albumPhotosModel);
                }
                else
                {
                    // ************** get current member:
                    var member = mr.Get(User.Identity.Name);
                    var albumPhotosModel = AlbumPhotosModel.Create(pr.GetAlbum(id), TempData, mr.Context);
                    // ************** gather homepage modules and return view:
                    var model = SocialNetApp.Views.Models.HomePage.IndexViewModel.Create(member, "AlbumPhotos", albumPhotosModel, mr.Context);
                    return View("~/Views/HomePage/Index.cshtml", model);
                }
            }
        }
        #endregion

        #region Post Actions:
        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public ActionResult CreateAlbum(Album model)
        {
            try
            {
                using (var pr = new AlbumsRepository())
                {
                    //is valid?
                    if (!ModelState.IsValid)
                    {
                        var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();
                        return Json(new { Done = false, Errors = errorList });
                    }
                    //exceed maximum albums?
                    if (!Member.IsInAdminGroup(User.Identity.Name, string.Empty) && pr.GetAlbumsCount(model.MemberId) >= Digits.MaximumAlbumCount)
                        throw new Exception(Resources.Messages.ExceedMaxAlbums);
                    // save:
                    model.VisibleTo = (byte)VisibleTo.Members;
                    model.AllowCommentTo = (byte)AllowCommentTo.Members;
                    model.DateOfAdd = MyHelper.Now;
                    model.VisitsCounter = 0;
                    pr.Insert(model);
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullyCreatedAlbum });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.InnerException.ToString() } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public ActionResult EditAlbum(Album model)
        {
            try
            {
                using (var pr = new AlbumsRepository())
                {
                    //is valid?
                    if (!ModelState.IsValid)
                    {
                        var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();
                        return Json(new { Done = false, Errors = errorList });
                    }
                    // save:
                    var albumToEdit = pr.GetAlbum(model.Id);
                    albumToEdit.Name = model.Name;
                    albumToEdit.Description = model.Description;
                    pr.Save();
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        public ActionResult UploadAlbumPhoto(HttpPostedFileBase File, FormCollection form)
        {
            try
            {
                using (var pr = new AlbumsRepository())
                {
                    var albumId = Convert.ToInt32(form["AlbumId"]);
                    bool hasPhoto = !(File == null);
                    // is valid photo format?
                    bool isValidPhoto = hasPhoto && MyHelper.IsSupportedPhotoExtension(MyHelper.GetExtension(File.FileName));
                    if (!isValidPhoto)
                    {
                        throw new Exception(Resources.Messages.NotValidPhotoFormat);
                    }
                    // is valid file size?
                    if (File.ContentLength > Digits.MaximumPhotoBytes)
                    {
                        throw new Exception(Resources.Messages.LargerThanMaximumUpload);
                    }
                    // local variables:
                    var album = pr.GetAlbum(albumId);
                    var ext = "." + MyHelper.GetExtension(File.FileName).ToLower();
                    var fileName = MyHelper.GetRandomString(20, true) + ext;
                    var rawImage = System.Drawing.Image.FromStream(File.InputStream);
                    var photoResizer = new Resizer(rawImage.Width, rawImage.Height, ResizeType.LongerFix, Digits.PhotoFixedWidth);
                    var thumbResizer = new Resizer(rawImage.Width, rawImage.Height, ResizeType.ShorterFix, Digits.LargeThumbSides);
                    var photo = MyHelper.GetThumbnailImage(rawImage, photoResizer.NewWidth, photoResizer.NewHeight);
                    var temp = MyHelper.GetThumbnailImage(rawImage, thumbResizer.NewWidth, thumbResizer.NewHeight);
                    var large_thumb = MyHelper.GetRectangleFromBitmapImage(temp, Digits.LargeThumbSides, Digits.LargeThumbSides);
                    var small_thumb = MyHelper.GetThumbnailImage(large_thumb, Digits.SmallThumbSides, Digits.SmallThumbSides);
                    var format = (ext == ".jpg" || ext == ".jpeg" ? System.Drawing.Imaging.ImageFormat.Jpeg : (ext == ".png" ? System.Drawing.Imaging.ImageFormat.Png : (ext == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg)));
                    // create photo object:
                    var p = new AlbumPhoto();
                    p.AlbumId = albumId;
                    p.Description = "";
                    p.FileName = fileName;
                    p.Width = (short)photo.Width;
                    p.Height = (short)photo.Height;
                    p.IsAlbumCover = (album.Photos.Any() ? false : true);
                    p.IsProfilePhoto = false;
                    p.MemberId = User.Identity.Name;
                    p.VisibleTo = (byte)VisibleTo.EveryOne;
                    p.AllowCommentTo = (byte)AllowCommentTo.Members;
                    p.DateOfAdd = MyHelper.Now;
                    p.VisitsCounter = 0;
                    // save files and data:
                    var url_photo = Urls.AlbumPhotos + fileName;
                    var url_large_thumb = Urls.AlbumLargeThumbnails + fileName;
                    var url_small_thumb = Urls.AlbumSmallThumbnails + fileName;
                    try
                    {
                        photo.Save(Server.MapPath(url_photo), format);
                        small_thumb.Save(Server.MapPath(url_small_thumb), format);
                        large_thumb.Save(Server.MapPath(url_large_thumb), format);
                        pr.Insert(p);
                    }
                    catch (Exception exc)
                    {
                        if (System.IO.File.Exists(Server.MapPath(url_photo)))
                            System.IO.File.Delete(Server.MapPath(url_photo));
                        if (System.IO.File.Exists(Server.MapPath(url_small_thumb)))
                            System.IO.File.Delete(Server.MapPath(url_small_thumb));
                        if (System.IO.File.Exists(Server.MapPath(url_large_thumb)))
                            System.IO.File.Delete(Server.MapPath(url_large_thumb));

                        TempData["done"] = false;
                        TempData["message"] = exc.Message;
                        return RedirectToAction("AlbumPhotos", new { id = Convert.ToInt32(form["AlbumId"]) });
                    }
                    //
                    TempData["done"] = true;
                    TempData["message"] = Resources.Messages.SuccessfullyUploadedAlbumPhoto;
                    return RedirectToAction("AlbumPhotos", new { id = albumId });
                }
            }
            catch(Exception ex)
            {
                TempData["done"] = false;
                TempData["message"] = ex.Message;
                return RedirectToAction("AlbumPhotos", new { id = Convert.ToInt32(form["AlbumId"]) });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult EditPhoto(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                using (var pr = new AlbumsRepository(mr.Context))
                {
                    var photoId = Convert.ToInt32(form["Id"]);
                    var p = pr.GetPhoto(photoId);
                    var member = mr.Get(User.Identity.Name);
                    // set fields:
                    p.Description = form["Description"];
                    if (!String.IsNullOrEmpty(form["IsAlbumCover"]) && form["IsAlbumCover"] == "on")
                    {
                        pr.ClearAlbumCover(p.AlbumId);
                        p.IsAlbumCover = true;
                    }
                    else
                    {
                        p.IsAlbumCover = false;
                    }
                    if (!String.IsNullOrEmpty(form["IsProfilePhoto"]) && form["IsProfilePhoto"] == "on")
                    {
                        // add change cover activity:
                        if (member.ProfilePhoto != p.FileName)
                        {
                            mr.AddChangePPActivity(member.Email, p.Id);
                        }
                        // clear profile photo:
                        pr.ClearProfilePhoto(User.Identity.Name);
                        //
                        p.IsProfilePhoto = true;
                        member.ProfilePhoto = p.FileName;
                    }
                    else if (p.IsProfilePhoto)
                    {
                        member.ProfilePhoto = "";
                        p.IsProfilePhoto = false;
                    }
                    p.VisibleTo = Convert.ToByte(form["VisibleTo"]);
                    p.AllowCommentTo = Convert.ToByte(form["AllowCommentTo"]);
                    // is valid model:
                    if (!p.IsValid)
                        return Json(new { Done = false, Id = photoId, Errors = p.ValidationErrors.Select(ve => ve.Value).ToArray() });
                    // edit thumbnails:
                    if (Convert.ToInt32(form["ThumbW"]) > 0 && Convert.ToInt32(form["ThumbH"]) > 0)
                    {
                        var scale = Convert.ToDouble(form["Scale"], System.Globalization.CultureInfo.InvariantCulture);
                        var multiple = 1 / scale;
                        var x = Convert.ToInt32(form["ThumbX"]) * multiple;
                        var y = Convert.ToInt32(form["ThumbY"]) * multiple;
                        var w = Convert.ToInt32(form["ThumbW"]) * multiple;
                        var h = Convert.ToInt32(form["ThumbH"]) * multiple;
                        var pImage = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Server.MapPath(p.Url));
                        var tempLargeThumb = MyHelper.GetRectangleFromBitmapImage(pImage, (int)x, (int)y, (int)w, (int)h);
                        var newLargeThumb = MyHelper.GetThumbnailImage(tempLargeThumb, Digits.LargeThumbSides, Digits.LargeThumbSides);
                        var newSmallThumb = MyHelper.GetThumbnailImage(newLargeThumb, Digits.SmallThumbSides, Digits.SmallThumbSides);
                        // save files:
                        var ext = "." + MyHelper.GetExtension(p.FileName);
                        var format = (ext == ".jpg" || ext == ".jpeg" ? System.Drawing.Imaging.ImageFormat.Jpeg : (ext == ".png" ? System.Drawing.Imaging.ImageFormat.Png : (ext == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg)));
                        newLargeThumb.Save(Server.MapPath(Urls.AlbumLargeThumbnails + p.FileName), format);
                        newSmallThumb.Save(Server.MapPath(Urls.AlbumSmallThumbnails + p.FileName), format);
                        pImage.Dispose();
                    }
                    // save data:
                    pr.Save();
                    mr.Save();
                    //
                    return Json(new { Done = true, Id = photoId });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Id = Convert.ToInt32(form["Id"]), Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        public ActionResult DeletePhoto(FormCollection form)
        {
            try
            {
                using (var pr = new AlbumsRepository())
                {
                    var albumId = Convert.ToInt32(form["AlbumId"]);
                    var photoId = Convert.ToInt32(form["Id"]);
                    pr.DeletePhoto(photoId);
                    TempData["done"] = true;
                    TempData["message"] = Resources.Messages.SuccessfullyDeleted;
                    return RedirectToAction("AlbumPhotos", new { id = albumId });
                }
            }
            catch
            {
                TempData["done"] = false;
                TempData["message"] = Resources.Messages.ExceptionTryAgain;
                return RedirectToAction("AlbumPhotos", new { id = Convert.ToInt32(form["AlbumId"]) });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeleteAlbum(int id)
        {
            using (var mr = new MembersRepository())
            using (var pr = new AlbumsRepository(mr.Context))
            {
                pr.DeleteAlbum(id);
                return PartialView(Urls.ModuleViews + "_Photos.cshtml", PhotosModel.Create(mr.Get(User.Identity.Name), mr.Context));
            }
        }

        [HttpPost]
        public ActionResult UploadProfileCover(HttpPostedFileBase File)
        {
            try
            {
                bool hasPhoto = !(File == null);
                // is valid photo format?
                bool isValidPhoto = hasPhoto && MyHelper.IsSupportedPhotoExtension(MyHelper.GetExtension(File.FileName));
                if (!isValidPhoto)
                    throw new Exception(Resources.Messages.NotValidPhotoFormat);
                // is valid file size?
                if (File.ContentLength > Digits.MaximumPhotoBytes)
                    throw new Exception(Resources.Messages.LargerThanMaximumUpload);
                // process image:
                var ext = "." + MyHelper.GetExtension(File.FileName).ToLower();
                var fileName = MyHelper.GetRandomString(20, true) + ext;
                var rawImage = System.Drawing.Image.FromStream(File.InputStream);
                var coverResizer = new Resizer(rawImage.Width, rawImage.Height, ResizeType.WidthFix, Digits.CoverWidth);
                var cover_base = MyHelper.GetThumbnailImage(rawImage, coverResizer.NewWidth, coverResizer.NewHeight);
                var cover = MyHelper.GetRectangleFromBitmapImage(cover_base, Digits.CoverWidth, Digits.CoverHeight);
                // save data and files:
                using (var ts = new System.Transactions.TransactionScope())
                using (var mr = new MembersRepository())
                {
                    var context = mr.Context;
                    // delete current cover:
                    var member = mr.Get(User.Identity.Name);
                    ProfileCoverPhoto cur_pcp = null;
                    if (member.HasProfileCover(context))
                    {
                        cur_pcp = mr.GetMemberProfileCoverPhoto(member.Email);
                        mr.Delete(cur_pcp);
                    }
                    // create profile photo cover obj:
                    var pcp = new ProfileCoverPhoto();
                    //sharedobject fields:
                    pcp.MemberId = User.Identity.Name;
                    pcp.VisibleTo = (byte)VisibleTo.EveryOne;
                    pcp.AllowCommentTo = (byte)AllowCommentTo.Members;
                    pcp.DateOfAdd = MyHelper.Now;
                    pcp.VisitsCounter = 0;
                    //photo fields:
                    pcp.FileName = fileName;
                    pcp.Width = (short)cover.Width;
                    pcp.Height = (short)cover.Height;
                    //profilecoverphoto fields:
                    pcp.XOnBase = 0;
                    pcp.YOnBase = 0;
                    //save data and files:
                    mr.Add(pcp);
                    mr.AddChangeCoverActivity(member.Email, pcp.Id);
                    mr.Save();
                    // delete old files:
                    if (cur_pcp != null)
                    {
                        System.IO.File.Delete(Server.MapPath(Urls.ProfileCovers + cur_pcp.FileName));
                        System.IO.File.Delete(Server.MapPath(Urls.ProfileCoverBases + cur_pcp.FileName));
                    }
                    // add new files:
                    var format = (ext == ".jpg" || ext == ".jpeg" ? System.Drawing.Imaging.ImageFormat.Jpeg : (ext == ".png" ? System.Drawing.Imaging.ImageFormat.Png : (ext == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg)));
                    cover.Save(Server.MapPath(Urls.ProfileCovers + fileName), format);
                    cover_base.Save(Server.MapPath(Urls.ProfileCoverBases + fileName), format);
                    //commit:
                    ts.Complete();
                }
                // return:
                TempData["done"] = true;
                return RedirectToAction("ProfileCover", "HomePage");
            }
            catch(Exception ex)
            {
                TempData["done"] = false;
                TempData["error"] = ex.Message;
                return RedirectToAction("ProfileCover", "HomePage");
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        public ActionResult EditCover(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var scale = Convert.ToDouble(form["Scale"], System.Globalization.CultureInfo.InvariantCulture);
                    var multiple = 1 / scale;
                    var x = Convert.ToDouble(form["X"], System.Globalization.CultureInfo.InvariantCulture) * multiple;
                    var y = Convert.ToDouble(form["Y"], System.Globalization.CultureInfo.InvariantCulture) * multiple;
                    var w = Convert.ToDouble(form["W"], System.Globalization.CultureInfo.InvariantCulture) * multiple;
                    var h = Convert.ToDouble(form["H"], System.Globalization.CultureInfo.InvariantCulture) * multiple;
                    //get cover data:
                    var pcp = mr.GetMemberProfileCoverPhoto(User.Identity.Name);
                    //save new cover file:
                    var cover_base = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Server.MapPath(Urls.ProfileCoverBases + pcp.FileName));
                    var cover = MyHelper.GetRectangleFromBitmapImage(cover_base, (int)x, (int)y, Digits.CoverWidth, Digits.CoverHeight);
                    var ext = "." + MyHelper.GetExtension(pcp.FileName);
                    var format = (ext == ".jpg" || ext == ".jpeg" ? System.Drawing.Imaging.ImageFormat.Jpeg : (ext == ".png" ? System.Drawing.Imaging.ImageFormat.Png : (ext == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg)));
                    cover.Save(Server.MapPath(Urls.ProfileCovers + pcp.FileName), format);
                    cover_base.Dispose();
                    // save data: 
                    pcp.XOnBase = (short)x;
                    pcp.YOnBase = (short)y;
                    pcp.Width = (short)cover.Width;
                    pcp.Height = (short)cover.Height;
                    mr.Save();
                    // return:
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySaved });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        public ActionResult DeleteCoverPhoto()
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    mr.DeleteProfileCover(User.Identity.Name);
                    TempData["deleted"] = true;
                    return RedirectToAction("ProfileCover", "HomePage");
                }
            }
            catch(Exception ex)
            {
                TempData["deleted"] = false;
                TempData["error"] = ex.Message;
                return RedirectToAction("ProfileCover", "HomePage");
            }
        }
        #endregion
    }
}
