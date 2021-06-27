using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;
using SocialNetApp.Models;
using System.Dynamic;

namespace CoreHelpers
{
    public static class FacebookHelper
    {
        public static JsonObject ShareAsTextPost(string txt, FacebookVisibleTo vt, string user_id, string access_token)
        {
            //--- facebook client configuration:
            var fbClient = InitFacebookClient(access_token);
            //--- gather parameters data:
            dynamic parameters = new ExpandoObject();
            // set fields:
            parameters.message = txt;
            parameters.privacy = new
            {
                value = (vt == FacebookVisibleTo.EveryOne ? "EVERYONE" :
                (vt == FacebookVisibleTo.Friends ? "ALL_FRIENDS" :
                (vt == FacebookVisibleTo.FriendsOfFriends ? "FRIENDS_OF_FRIENDS" :
                "SELF")))
            };
            var result = (JsonObject)fbClient.Post(user_id + "/feed", parameters);
            //var target = GetSharedObjectFacebookAddress("feed/" + result["id"]);
            return result;
        }

        public static JsonObject ShareAsPhotoPost(Photo photo, string fb_post_text, 
            FacebookVisibleTo vt, string user_id, string access_token)
        {
            //--- facebook client configuration:
            var fbClient = InitFacebookClient(access_token);
            // photo parameters:
            var phFileName = System.IO.Path.GetFileName(HttpContext.Current.Server.MapPath(photo.Url));
            var phBytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(photo.Url));
            var phSource = new FacebookMediaObject
            {
                ContentType = "image/jpeg",
                FileName = phFileName
            };
            phSource.SetValue(phBytes);
            // parameters:
            dynamic parameters = new ExpandoObject();
            parameters.message = fb_post_text;
            parameters.source = phSource;
            parameters.privacy = new {
                value = (vt == FacebookVisibleTo.EveryOne ? "EVERYONE" :
                (vt == FacebookVisibleTo.Friends ? "ALL_FRIENDS" :
                (vt == FacebookVisibleTo.FriendsOfFriends ? "FRIENDS_OF_FRIENDS" :
                "SELF")))
            };
            var result = fbClient.Post(user_id + "/photos", parameters);
            //var target = GetSharedObjectFacebookAddress("photo.php?fbid=" + result["id"]);
            return result;
        }

        public static JsonObject ShareAsAlbumPost(List<Photo> photos, string fb_post_text, 
            FacebookVisibleTo vt, string user_id, string access_token)
        {
            string target = "";
            //--- facebook client configuration:
            var fbClient = InitFacebookClient(access_token);
            #region create new album on facebook:
            dynamic parameters = new ExpandoObject();
            parameters.name = Resources.Words.FacebookSharedAlbumDefaultName;
            parameters.message = fb_post_text;
            parameters.privacy = new {
                value = (vt == FacebookVisibleTo.EveryOne ? "EVERYONE" :
                    (vt == FacebookVisibleTo.Friends ? "ALL_FRIENDS" :
                    (vt == FacebookVisibleTo.FriendsOfFriends ? "FRIENDS_OF_FRIENDS" :
                    "SELF")))
            };
            var albumCreationResult = fbClient.Post(user_id + "/albums", parameters);
            #endregion
            #region upload photos to newly created album:
            if (!String.IsNullOrEmpty(albumCreationResult.id))
            {
                var albumId = albumCreationResult.id;
                foreach (var ph in photos)
                {
                    // photo parameters:
                    var phFileName = System.IO.Path.GetFileName(HttpContext.Current.Server.MapPath(ph.Url));
                    var phBytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(ph.Url));
                    var phSource = new FacebookMediaObject {
                        ContentType = "image/jpeg",
                        FileName = phFileName
                    };
                    phSource.SetValue(phBytes);
                    dynamic phParameters = new ExpandoObject();
                    parameters.message = fb_post_text;
                    parameters.source = phSource;
                    // post:
                    fbClient.Post("/" + albumId + "/photos", parameters);
                }
                target = GetSharedObjectFacebookAddress("albums/" + albumId);
            }
            #endregion
            return albumCreationResult;
        }

        public static JsonObject ShareAsVideoPost(PostVideo video, string fb_post_text, 
            FacebookVisibleTo vt, string user_id, string access_token)
        {
            //--- facebook client configuration:
            var fbClient = InitFacebookClient(access_token);
            // photo parameters:
            var vidFileName = System.IO.Path.GetFileName(HttpContext.Current.Server.MapPath(video.UrlOfVideoFile));
            var vidBytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(video.UrlOfVideoFile));
            var vidSource = new FacebookMediaObject
            {
                ContentType = "video/mp4",
                FileName = vidFileName
            };
            vidSource.SetValue(vidBytes);
            // parameters:
            dynamic parameters = new ExpandoObject();
            parameters.message = fb_post_text;
            parameters.source = vidSource;
            parameters.privacy = new
            {
                value = (vt == FacebookVisibleTo.EveryOne ? "EVERYONE" :
                (vt == FacebookVisibleTo.Friends ? "ALL_FRIENDS" :
                (vt == FacebookVisibleTo.FriendsOfFriends ? "FRIENDS_OF_FRIENDS" :
                "SELF")))
            };
            var result = fbClient.Post(user_id + "/videos", parameters);
            return result;
        }

        private static FacebookClient InitFacebookClient(string access_token)
        {
            //var appId = System.Configuration.ConfigurationManager.AppSettings["FbAppId"];
            //var appSec = System.Configuration.ConfigurationManager.AppSettings["FbAppSec"];
            //var userId = user_id;
            var fbClient = new FacebookClient();
            fbClient.AccessToken = access_token;
            return fbClient;
        }
        private static string GetSharedObjectFacebookAddress(string path)
        {
            return String.Format(@"http://www.facebook.com/{0}", path);
        }
    }
}