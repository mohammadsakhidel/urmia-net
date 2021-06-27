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
using System.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetApp.Controllers
{
    [Authorize(Roles = MyRoles.Member)]
    public class PostsController : MainController
    {
        #region Post Actions:
        [HttpPost]
        [AcceptAjax]
        public ActionResult UploadPostAttachment(HttpPostedFileBase posted_file, FormCollection form)
        {
            try
            {
                // is maximum post photo count exceeded?
                var attachmentType = form["AttachmentsType"];
                if (attachmentType == "photo")
                {
                    var tempFileNames = MyHelper.SplitString(form["TempFileNames"], ";");
                    if (tempFileNames.Count >= Digits.MaxPostPhotos)
                        throw new Exception(Resources.Messages.ExceedMaxPostPhotos);
                    // is valid photo format?
                    bool hasPhoto = !(posted_file == null);
                    bool isValidPhoto = hasPhoto && MyHelper.IsSupportedPhotoExtension(MyHelper.GetExtension(posted_file.FileName));
                    if (!isValidPhoto)
                        throw new Exception(Resources.Messages.NotValidPhotoFormat);
                    // is valid file size?
                    if (posted_file.ContentLength > Digits.MaximumPhotoBytes)
                        throw new Exception(Resources.Messages.LargerThanMaximumUpload);
                    // create image objects:
                    var ext = "." + MyHelper.GetExtension(posted_file.FileName).ToLower();
                    var rawImage = System.Drawing.Image.FromStream(posted_file.InputStream);
                    // create temp small thumb for display:
                    var smallThumbResizer = new Resizer(rawImage.Width, rawImage.Height, ResizeType.ShorterFix, Digits.PostsSmallThumbSides);
                    var temp_small = MyHelper.GetThumbnailImage(rawImage, smallThumbResizer.NewWidth, smallThumbResizer.NewHeight);
                    var small_thumb = MyHelper.GetRectangleFromBitmapImage(temp_small, Digits.PostsSmallThumbSides, Digits.PostsSmallThumbSides);
                    // save raw image and display thumb to temp folder:
                    var randomName = MyHelper.GetRandomString(20, true);
                    var tempfileName = randomName + ext;
                    var tempthumbFileName = randomName + "_thumb" + ext;
                    var format = (ext == ".jpg" || ext == ".jpeg" ? System.Drawing.Imaging.ImageFormat.Jpeg : (ext == ".png" ? System.Drawing.Imaging.ImageFormat.Png : (ext == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg)));
                    rawImage.Save(Server.MapPath(Urls.TempFolder + tempfileName), format);
                    small_thumb.Save(Server.MapPath(Urls.TempFolder + tempthumbFileName), format);
                    // return:
                    return Json(new { Done = true, Path = Url.Content(Urls.TempFolder + tempthumbFileName), TempFileName = tempfileName });
                }
                else if (attachmentType == "video")
                {
                    var tempFileNames = MyHelper.SplitString(form["TempFileNames"], ";");
                    if (tempFileNames.Count >= Digits.MaxVideosPerPost)
                        throw new Exception(Resources.Messages.ExceedMaxPostVideos);
                    // is valid video format?
                    bool hasVideo = !(posted_file == null);
                    bool isValidVideo = hasVideo && Configs.SupportedVideoFormats.Contains(MyHelper.GetExtension(posted_file.FileName.ToLower()));
                    if (!isValidVideo)
                        throw new Exception(Resources.Messages.NotValidVideoFormat);
                    // is valid file size?
                    if (posted_file.ContentLength > Digits.MaximumVideoBytes)
                        throw new Exception(Resources.Messages.LargerThanMaximumUpload);
                    // save raw video to temps file folder:
                    var ext = string.Format(".{0}", MyHelper.GetExtension(posted_file.FileName).ToLower());
                    var fileName = MyHelper.GetRandomString(20, true);
                    var tempRawPath = Server.MapPath(string.Format("{0}{1}{2}", Urls.TempFolder, fileName, ext));
                    posted_file.SaveAs(tempRawPath);
                    // generate video thumbnails:
                    var thumbRaw = VideoHelper.GetVideoThumbnail(tempRawPath);
                    var thumbActualSizeResizer = new Resizer(thumbRaw.Width, thumbRaw.Height, ResizeType.WidthFix, Digits.VideosFixedWidth);
                    var thumbActualSize = MyHelper.GetThumbnailImage(thumbRaw, thumbActualSizeResizer.NewWidth, thumbActualSizeResizer.NewHeight);
                    var thumbSmallResizer = new Resizer(thumbActualSize.Width, thumbActualSize.Height, ResizeType.ShorterFix, Digits.PostsSmallThumbSides);
                    var thumbSmallTemp = MyHelper.GetThumbnailImage(thumbActualSize, thumbSmallResizer.NewWidth, thumbSmallResizer.NewHeight);
                    var thumbSmall = MyHelper.GetRectangleFromBitmapImage(thumbSmallTemp, Digits.PostsSmallThumbSides, Digits.PostsSmallThumbSides);
                    // save thumbs to temp:
                    var thumbSmallFileName = fileName + "_sthumb.jpg";
                    var thumbActualSizeFileName = fileName + "_athumb.jpg";
                    thumbSmall.Save(Server.MapPath(Urls.TempFolder + thumbSmallFileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                    thumbActualSize.Save(Server.MapPath(Urls.TempFolder + thumbActualSizeFileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                    // return:
                    return Json(new { Done = true, Path = Url.Content(Urls.TempFolder + thumbSmallFileName), TempFileName = fileName + ext });
                }
                else
                {
                    return Json(new { Done = false, Errors = new string[] { Resources.Messages.NotValidFileFormat } });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter(DelayRequestInSeconds = 60, ErrorMessageKey = "SpamPostMessage")]
        public ActionResult SendPost(FormCollection form)
        {
            try
            {
                using (var pr = new PostsRepository())
                using (var ar = new ActivityRepository(pr.Context))
                {
                    var context = pr.Context;
                    // is allowet ro post on wall?
                    var postType = Convert.ToByte(form["PostType"]);
                    var postOwnerId = "";
                    if (postType == (byte)PostType.WallPost)
                    {
                        postOwnerId = form["Considerations"];
                        var mr = new MembersRepository();
                        var wallOwner = mr.Get(postOwnerId);
                        if (!wallOwner.IsPostingOnWallAvailableTo(User.Identity.Name, context))
                            throw new Exception(Resources.Messages.NotAllowedToPostOnWall);
                    }
                    // save post:
                    Post post = new Post();
                    using (var ts = new TransactionScope())
                    {
                        post = getPostInfoFromForm(form);
                        // is valid:
                        if (!post.IsValid)
                            return Json(new { Done = false, Errors = post.ValidationErrors.Select(ve => ve.Value).ToArray() });
                        //post tags:
                        var tagsTxt = form["AddedTags"];
                        var tags = MyHelper.SplitString(tagsTxt, ";");
                        foreach (var tag in tags.Distinct())
                        {
                            var tagObj = new SharedObjectTag
                            {
                                SharedObjectId = post.Id,
                                Tag = tag
                            };
                            post.SharedObjectTags.Add(tagObj);
                        }
                        //--
                        pr.Add(post);
                        pr.Save();
                        // attachment type?
                        var attType = !String.IsNullOrEmpty(form["AttachmentsType"]) ? form["AttachmentsType"] : "";
                        if (attType == "photo")
                        {
                            // add post photos:
                            var postPhotos = new List<PostPhoto>();
                            foreach (var tempFileName in post.TempFilesToBeAdded)
                            {
                                var postPhoto = getPostPhotoInfo(post, tempFileName);
                                pr.Add(postPhoto);
                                postPhotos.Add(postPhoto);
                            }
                            pr.Save();
                            // save post photo files:
                            savePostPhotoFiles(postPhotos);
                        }
                        else if (attType == "video")
                        {
                            // add post photos:
                            var postVideos = new List<PostVideo>();
                            foreach (var tempFileName in post.TempFilesToBeAdded)
                            {
                                var metadata = VideoHelper.GetVideoMetadata(Server.MapPath(Urls.TempFolder + tempFileName), Server.MapPath(Urls.VideoProcessor));
                                var postVideo = getPostVideoInfo(post, tempFileName, metadata);
                                pr.Add(postVideo);
                                postVideos.Add(postVideo);
                            }
                            pr.Save();
                            // save post photo files:
                            savePostVideoFiles(postVideos);
                        }
                        ts.Complete();
                    }
                    // get sent post activity:
                    Activity act = null;
                    string partial_view_name = "";
                    if (post.Type == (byte)PostType.NormalPost)
                    {
                        act = ar.FindPostActivity(post.Id);
                        partial_view_name = "_PostActivity.cshtml";
                    }
                    else if (post.Type == (byte)PostType.WallPost)
                    {
                        act = ar.FindPostOnWallActivity(post.Id);
                        partial_view_name = "_PostOnWallActivity.cshtml";
                    }
                    var param = new Dictionary<string, object>();
                    param["AVCL"] = ActivityVisibilityCheckLevel.None;
                    var actModel = ActivityModel.Create(act, param, pr.Context);
                    string partial_view = RenderViewToString(Urls.ModuleViews + partial_view_name, actModel);
                    //
                    return Json(new { Done = true, Message = Resources.Messages.SuccessfullySent, PartialView = partial_view });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.InnerException != null ? ex.InnerException.Message : ex.Message } });
            }
        }
        #endregion

        #region Private methods:
        private Post getPostInfoFromForm(FormCollection form)
        {
            var post = new Post();
            post.MemberId = User.Identity.Name;
            post.Text = form["Text"];
            post.Status = (byte)PostStatus.Visible;
            post.Type = Convert.ToByte(form["PostType"]);
            post.Considerations = (post.Type == (byte)PostType.WallPost ? form["Considerations"] : "");
            post.VisibleTo = Convert.ToByte(form["VisibleTo"]);
            post.AllowCommentTo = Convert.ToByte(form["AllowCommentTo"]);
            post.DateOfAdd = MyHelper.Now;
            post.VisitsCounter = 0;
            var tempFileNames = MyHelper.SplitString(form["TempFileNames"], ";");
            post.TempFilesToBeAdded = tempFileNames;
            return post;
        }

        private PostPhoto getPostPhotoInfo(Post post, string tempFileName)
        {
            var postPhoto = new PostPhoto();
            var rawImage = System.Drawing.Image.FromFile(Server.MapPath(Urls.TempFolder + tempFileName));
            var photoResizer = new Resizer(rawImage.Width, rawImage.Height, ResizeType.LongerFix, Digits.PhotoFixedWidth);
            rawImage.Dispose();
            postPhoto.PostId = post.Id;
            postPhoto.FileName = tempFileName;
            postPhoto.Width = (short)photoResizer.NewWidth;
            postPhoto.Height = (short)photoResizer.NewHeight;
            postPhoto.MemberId = User.Identity.Name;
            postPhoto.VisibleTo = post.VisibleTo;
            postPhoto.AllowCommentTo = post.AllowCommentTo;
            postPhoto.DateOfAdd = MyHelper.Now;
            postPhoto.VisitsCounter = 0;
            return postPhoto;
        }

        private void savePostPhotoFiles(List<PostPhoto> postPhotos)
        {
            foreach (PostPhoto p in postPhotos)
            {
                var rawImage = System.Drawing.Image.FromFile(Server.MapPath(Urls.TempFolder + p.FileName));
                var photo = MyHelper.GetThumbnailImage(rawImage, p.Width, p.Height);
                // large thumb:
                var largeThumbResizer = new Resizer(rawImage.Width, rawImage.Height, ResizeType.WidthFix, Digits.PostsLargeThumbWidth);
                var temp_large = MyHelper.GetThumbnailImage(rawImage, largeThumbResizer.NewWidth, largeThumbResizer.NewHeight);
                var large_thumb = MyHelper.GetRectangleFromBitmapImage(temp_large, Digits.PostsLargeThumbWidth, (temp_large.Height < Digits.PostsLargeThumbHeight ? temp_large.Height : Digits.PostsLargeThumbHeight));
                // small thumb:
                var smallThumbResizer = new Resizer(rawImage.Width, rawImage.Height, ResizeType.ShorterFix, Digits.PostsSmallThumbSides);
                var temp_small = MyHelper.GetThumbnailImage(rawImage, smallThumbResizer.NewWidth, smallThumbResizer.NewHeight);
                var small_thumb = MyHelper.GetRectangleFromBitmapImage(temp_small, Digits.PostsSmallThumbSides, Digits.PostsSmallThumbSides);
                var ext = "." + MyHelper.GetExtension(p.FileName).ToLower();
                var format = (ext == ".jpg" || ext == ".jpeg" ? System.Drawing.Imaging.ImageFormat.Jpeg : (ext == ".png" ? System.Drawing.Imaging.ImageFormat.Png : (ext == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg)));
                //save files:
                photo.Save(Server.MapPath(Urls.PostPhotos + p.FileName), format);
                large_thumb.Save(Server.MapPath(Urls.PostLargeThumbnails + p.FileName), format);
                small_thumb.Save(Server.MapPath(Urls.PostSmallThumbnails + p.FileName), format);
                // delete temp files:
                rawImage.Dispose();
                System.IO.File.Delete(Server.MapPath(Urls.TempFolder + p.FileName));
                System.IO.File.Delete(Server.MapPath(Urls.TempFolder + p.FileName.Insert(p.FileName.LastIndexOf('.'), "_thumb")));
            }
        }

        private PostVideo getPostVideoInfo(Post post, string tempFileName, VideoMetadata metadata)
        {
            var postVideo = new PostVideo();
            var rawWidth = metadata.Size.HasValue ? (short)metadata.Size.Value.Width : (short?)null;
            var rawHeight = metadata.Size.HasValue ? (short)metadata.Size.Value.Height : (short?)null;
            var resizer = metadata.Size.HasValue ? new Resizer(rawWidth.Value, rawHeight.Value, ResizeType.WidthFix, Digits.VideosFixedWidth) : null;
            // postvideo fields:
            postVideo.PostId = post.Id;
            // video fields:
            postVideo.FileName = string.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(tempFileName), Configs.ConvertedVideosFormat);
            postVideo.Width = resizer != null ? (short)resizer.NewWidth : (short)Digits.VideosFixedWidth;
            postVideo.Height = resizer != null ? (short)resizer.NewHeight : (short)Digits.VideosFixedHeight;
            postVideo.DurationSeconds = metadata.Duration.HasValue ? (short)metadata.Duration.Value.TotalSeconds : (short?)null;
            postVideo.InputFormat = MyHelper.GetExtension(tempFileName);
            postVideo.ConvertedFormat = Configs.ConvertedVideosFormat;
            postVideo.ProcessResult = (byte)VideoProcessResult.NotReported;
            postVideo.ProcessResultMessage = "";
            // sharedobject fields:
            postVideo.MemberId = User.Identity.Name;
            postVideo.VisibleTo = post.VisibleTo;
            postVideo.AllowCommentTo = post.AllowCommentTo;
            postVideo.DateOfAdd = MyHelper.Now;
            postVideo.VisitsCounter = 0;
            return postVideo;
        }

        private void savePostVideoFiles(List<PostVideo> postVideos)
        {
            foreach (var vid in postVideos)
            {
                // get thumbnails:
                var fileNameBase = System.IO.Path.GetFileNameWithoutExtension(vid.FileName);
                var actualSizeThumbPath = Server.MapPath(string.Format("{0}{1}_athumb.jpg", Urls.TempFolder, fileNameBase));
                var smallThumbPath = Server.MapPath(string.Format("{0}{1}_sthumb.jpg", Urls.TempFolder, fileNameBase));
                var actualSizeThumb = System.Drawing.Image.FromFile(actualSizeThumbPath);
                var smallThumb = System.Drawing.Image.FromFile(smallThumbPath);
                // save thumbnails:
                actualSizeThumb.Save(Server.MapPath(string.Format("{0}{1}.jpg", Urls.PostVideosActualSizeThumbs, fileNameBase)), System.Drawing.Imaging.ImageFormat.Jpeg);
                smallThumb.Save(Server.MapPath(string.Format("{0}{1}.jpg", Urls.PostVideosSmallThumbs, fileNameBase)), System.Drawing.Imaging.ImageFormat.Jpeg);
                actualSizeThumb.Dispose();
                smallThumb.Dispose();
                // delete temp thumbnails:
                System.IO.File.Delete(actualSizeThumbPath);
                System.IO.File.Delete(smallThumbPath);
                // convert and save video file (long running) then delete temp video:
                var svcClient = new CoreServices.LongRunningProcesses.VideoProcessorServiceClient();
                svcClient.ProcessPostVideo(
                    Server.MapPath(string.Format("{0}{1}.{2}", Urls.TempFolder, fileNameBase, vid.InputFormat)),
                    Server.MapPath(string.Format("{0}{1}.{2}", Urls.PostVideos, fileNameBase, vid.ConvertedFormat)),
                    Server.MapPath(Urls.VideoProcessor),
                    Digits.VideosFixedWidth,
                    Digits.VideosFixedHeight,
                    vid.Id);
            }
        }
        #endregion
    }
}
