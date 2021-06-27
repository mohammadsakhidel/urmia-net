using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using System.Web.Script.Serialization;
using SocialNetApp.Views.Models.Modules;
using System.IO;

namespace SocialNetApp.Controllers
{
    public class MembersController : MainController
    {
        #region GET ACTIONS:
        public ActionResult Index()
        {
            var model = SocialNetApp.Views.Models.Members.IndexViewModel.Create(null);
            return View(model);
        }

        public ActionResult Search()
        {
            var model = SocialNetApp.Views.Models.Members.SearchViewModel.Create(null);
            return View(model);
        }

        public ActionResult ProfilePage(string username, string content)
        {
            using (var mr = new MembersRepository())
            {
                var context = mr.Context;
                var member = mr.GetByUserName(username);
                if (member == null || member.StatusCode != (byte)MemberStatus.Active)
                    throw new HttpException(404, "Page Not Found.");
                if (Request.IsAjaxRequest())
                {
                    Response.CacheControl = "no-cache";
                    var viewer = (User.Identity.IsAuthenticated ? mr.Get(User.Identity.Name) : null);
                    switch (content)
                    {
                        case "timeline":
                            return PartialView(Urls.ModuleViews + "_ProfilePageTimeLine.cshtml",
                                ProfilePageTimeLineModel.Create(member, viewer, context));
                        case "friends":
                            return PartialView(Urls.ModuleViews + "_ProfilePageFriends.cshtml", 
                                ProfilePageFriendsModel.Create(member, viewer, context));
                        case "info":
                            return PartialView(Urls.ModuleViews + "_ProfilePageInfo.cshtml",
                                ProfilePageInfoModel.Create(member, viewer, context));
                        case "photos":
                            return PartialView(Urls.ModuleViews + "_ProfilePagePhotos.cshtml",
                                ProfilePagePhotosModel.Create(member, viewer, context));
                        case "activities":
                            return PartialView(Urls.ModuleViews + "_ProfilePageActivities.cshtml",
                                ProfilePageActivitiesModel.Create(member, viewer, context));
                        default:
                            return PartialView(Urls.ModuleViews + "_ProfilePageTimeLine.cshtml",
                                ProfilePageTimeLineModel.Create(member, viewer, context));
                    }
                }
                else
                {
                    // add prof view:
                    if (member.Email != User.Identity.Name)
                    {
                        var pv = new ProfileView();
                        pv.ProfileOwnerId = member.Email;
                        pv.ViewerId = User.Identity.Name;
                        pv.DateOfView = MyHelper.Now;
                        pv.Reference = (Request.QueryString["ref"] != null ? Request.QueryString["ref"].ToString() : "");
                        mr.Add(pv);
                    }
                    //******************
                    member.ProfileVisitCounter++;
                    mr.Save();
                    //******************
                    var model = SocialNetApp.Views.Models.Members.ProfilePageViewModel.Create(member, RouteData.Values, mr.Context);
                    return View(model);
                }
            }
        }
        #endregion

        #region POST ACTIONS:
        [HttpPost]
        [AcceptAjax]
        public ActionResult GetMoreProfilePageActivities(FormCollection form)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    // get acts:
                    var last_act_date = Convert.ToDateTime(form["CurrentPageIndex"]);
                    var email = form["Parameters"];
                    var viewer = (User.Identity.IsAuthenticated ? User.Identity.Name : "");
                    var acts = ar.FindProfilePageActivitis(email, viewer, last_act_date);
                    var last_act_date_new = (acts.Any() ? acts.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
                    var is_there_more = String.IsNullOrEmpty(last_act_date_new) ? false : ar.IsThereMoreProfilePageActivitis(email, viewer, Convert.ToDateTime(last_act_date_new));
                    // create partial view:
                    var parameters = new Dictionary<string, object>();
                    parameters.Add("AVCL", ActivityVisibilityCheckLevel.None);
                    var partial_view = "";
                    foreach (var act in acts)
                    {
                        partial_view += RenderViewToString(act.PartialViewUrl, ActivityModel.Create(act, parameters, ar.Context));
                    }
                    // return:
                    return Json(new
                    {
                        Done = true,
                        RandomId = form["RandomId"],
                        IsThereMore = is_there_more,
                        PartialView = partial_view,
                        NextPageIndex = last_act_date_new
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message },
                    RandomId = form["RandomId"]
                });
            }
        }

        [AcceptAjax]
        [HttpPost]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult FindInAssociatedMembers(string fullName)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var members = mr.FindAssociatedUsers(User.Identity.Name, fullName);
                    var ser = new JavaScriptSerializer();
                    var members_json = ser.Serialize(members.Select(m => new
                    {
                        m.Email,
                        m.FullName,
                        PathOfThumb = Url.Content(m.UrlOfThumb),
                        PathOfProfilePage = Url.Content(m.UrlOfProfilePage)
                    }));
                    return Json(new { Done = true, Members = members_json });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult GetMoreFriends(FormCollection form)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var current_pg_index = Convert.ToInt32(form["CurrentPageIndex"]);
                    var next_page_index = current_pg_index + 1;
                    var email = form["Parameters"];
                    var more_friends = mr.FindPagedFriends(email, next_page_index);
                    var is_there_more = mr.GetFriendsCount(email) > Digits.ListingItemsPageSize * (next_page_index + 1);
                    // create partial view:
                    string partial_view = "";
                    foreach (var f in more_friends)
                    {
                        partial_view +=
                            "<div class=\"pp_friend_item\">" +
                                RenderViewToString(Urls.ModuleViews + "_UserIdentity.cshtml", 
                                UserIdentityModel.Create(f, 70, UserIdentityType.ThumbAndFullName, "", "pp_friends_frnd_fullname", "", "", mr.Context)) +
                            "</div>";
                    }
                    return Json(new
                    {
                        Done = true,
                        RandomId = form["RandomId"],
                        IsThereMore = is_there_more,
                        PartialView = partial_view,
                        NextPageIndex = next_page_index
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message }, RandomId = form["RandomId"] });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult GetMoreTimelineActivities(FormCollection form)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    // get acts:
                    var last_act_date = Convert.ToDateTime(form["CurrentPageIndex"]);
                    var email = form["Parameters"];
                    var viewer_email = User.Identity.IsAuthenticated ? User.Identity.Name : "";
                    var acts = ar.FindMemberActivitiesForHisWall(email, viewer_email, last_act_date);
                    var last_act_date_new = (acts.Any() ? acts.Last().TimeOfAct.ToString("yyyy-MM-dd HH:mm:ss.fff") : "");
                    var is_there_more = String.IsNullOrEmpty(last_act_date_new) ? false : ar.IsThereMoreWallActivities(email, viewer_email, Convert.ToDateTime(last_act_date_new));
                    // create partial view:
                    var parameters = new Dictionary<string, object>();
                    parameters.Add("AVCL", ActivityVisibilityCheckLevel.None);
                    var partial_view = "";
                    foreach (var act in acts)
                    {
                        partial_view += RenderViewToString(act.PartialViewUrl, ActivityModel.Create(act, parameters, ar.Context));
                    }
                    // return:
                    return Json(new
                    {
                        Done = true,
                        RandomId = form["RandomId"],
                        IsThereMore = is_there_more,
                        PartialView = partial_view,
                        NextPageIndex = last_act_date_new
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message },
                    RandomId = form["RandomId"]
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult CacelFriendshipRequest(int requestId)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                using (var mr = new MembersRepository())
                {
                    mr.DeleteFriendshipRequest(requestId);
                    return Json(new { Done = true });
                }
            }
            catch
            {
                return Json(new { Done = false });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [SpamFilter]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult SendFriendshipRequest(string from, string to)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                using (var mr = new MembersRepository())
                {
                    var context = mr.Context;
                    var from_member = mr.GetByUserName(from);
                    var to_member = mr.GetByUserName(to);
                    // create object:
                    var friendshipReq = new FriendshipRequest();
                    friendshipReq.From = from_member.Email;
                    friendshipReq.To = to_member.Email;
                    friendshipReq.DateOfCreate = MyHelper.Now;
                    friendshipReq.Status = (byte)FriendshipStatus.Requested;
                    // is pending request?

                    if (mr.PendingFriendshipRequestExists(friendshipReq.From, friendshipReq.To))
                        throw new Exception(Resources.Messages.FriendshipRequestExists);

                    // is allowed?
                    if (!to_member.IsAllowedToSendFriendshipRequest(from_member.Email, context))
                        throw new Exception(Resources.Messages.NotAllowedToSendFriendshipRequest);
                    // save:
                    mr.Insert(friendshipReq);
                    //
                    return Json(new { Done = true, CancelUrl = Url.Action("CacelFriendshipRequest", "Members", new { requestId = friendshipReq.Id }) });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult AcceptFriendshipRequest(int requestId)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var friendshipReq = mr.GetFriendshipRequest(requestId);
                    // has access?
                    if (friendshipReq.To != User.Identity.Name)
                        throw new Exception(Resources.Messages.DontHaveAccess);
                    // are friends?
                    if (mr.AreFriends(friendshipReq.From, friendshipReq.To))
                        throw new Exception(Resources.Messages.YouAreFriendsNow);
                    // add friendship relation:
                    friendshipReq.Status = (byte)FriendshipStatus.Confirmed;
                    friendshipReq.ResponseDate = MyHelper.Now;
                    var rel = new FriendshipRelation();
                    rel.MemberOneId = friendshipReq.From;
                    rel.MemberTwoId = friendshipReq.To;
                    rel.DateOfAdd = MyHelper.Now;
                    rel.AssociatedRequestId = friendshipReq.Id;
                    mr.Add(rel);
                    //add followings:
                    if (!mr.IsFollowing(friendshipReq.From, friendshipReq.To))
                    {
                        var flwForFrom = new Following
                        {
                            FollowerId = friendshipReq.From,
                            FollowedId = friendshipReq.To,
                            DateOfBeginFollowing = MyHelper.Now
                        };
                        mr.Add(flwForFrom);
                    }
                    if (!mr.IsFollowing(friendshipReq.To, friendshipReq.From))
                    {
                        var flwForTo = new Following
                        {
                            FollowerId = friendshipReq.To,
                            FollowedId = friendshipReq.From,
                            DateOfBeginFollowing = MyHelper.Now
                        };
                        mr.Add(flwForTo);
                    }
                    // add acceptrequest notification:
                    mr.AddAcceptRequestNotification(friendshipReq);
                    // save:
                    mr.Save();
                    return Json(new { Done = true, RequestId = requestId });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, RequestId = requestId, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult RejectFriendshipRequest(int requestId)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var req = mr.GetFriendshipRequest(requestId);
                    // has access?
                    if (req.To != User.Identity.Name)
                        throw new Exception(Resources.Messages.DontHaveAccess);
                    req.Status = (byte)FriendshipStatus.Rejected;
                    req.ResponseDate = MyHelper.Now;
                    mr.Save();
                    return Json(new { Done = true, RequestId = requestId });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, RequestId = requestId, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult SearchMember(SearchMemberInfo info)
        {
            try
            {
                System.Threading.Thread.Sleep(3000);
                using (var mr = new MembersRepository())
                {
                    var members = mr.SearchMembers(info);
                    return Json(new { Done = true, PartialView = RenderViewToString(Urls.ModuleViews + "_SearchResult.cshtml", SearchResultModel.Create(members.ToList(), mr.Context)) });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [HttpPost]
        [AcceptAjax]
        [Authorize(Roles = MyRoles.Member)]
        public ActionResult ToggleFollowing(string id)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                using (var mr = new MembersRepository())
                {
                    var follower = mr.Get(User.Identity.Name);
                    var followed = mr.GetByUserName(id);
                    var isFollowing = mr.IsFollowing(follower.Email, followed.Email);
                    #region //************************* FOLLOW:
                    if (!isFollowing)
                    {
                        if (!mr.IsFollowingAllowed(follower, followed))
                            throw new Exception(Resources.Messages.NotAllowedToFollow);
                        var flw = new Following
                        {
                            FollowerId = follower.Email,
                            FollowedId = followed.Email,
                            DateOfBeginFollowing = MyHelper.Now
                        };
                        mr.Insert(flw);
                    }
                    #endregion
                    #region//************************* UNFOLLOW:
                    else
                    {
                        // delete following record(s):
                        mr.Delete(follower.Email, followed.Email);
                    }
                    #endregion
                    return Json(new { Done = true, ButtonText = isFollowing ? Resources.Words.Follow : Resources.Words.Unfollow });
                }
            }
            catch(Exception ex)
            {
                return Json(new { 
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }
        #endregion
    }
}
