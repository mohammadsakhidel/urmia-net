using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using SocialNetApp.Models.Repository;
using SocialNetApp.Models;

namespace SocialNetApp.Controllers
{
    [Authorize(Roles = MyRoles.Member)]
    public class ActivitiesController : MainController
    {
        #region POST Actions:
        [AcceptAjax]
        [HttpPost]
        public ActionResult HideActivity(int id)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    var act_setting = ar.GetActivitySetting(User.Identity.Name);
                    if (act_setting != null)
                    {
                        if (!act_setting.UnwantedActivityIds.Contains(id))
                        {
                            var unwanted_act_ids = act_setting.UnwantedActivityIds;
                            unwanted_act_ids.Add(id);
                            act_setting.UnwantedActivityIds = unwanted_act_ids;
                            ar.Save();
                        }
                        ar.Save();
                    }
                    else
                    {
                        act_setting = ActivitySetting.Default;
                        act_setting.MemberId = User.Identity.Name;
                        act_setting.UnwantedActivities = id.ToString();
                        ar.Insert(act_setting);
                    }
                    return Json(new { Done = true, ActivityId = id });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult HideMemberActivities(int id)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    var act = ar.Get(id);
                    var memberId = act.MemberId;
                    var act_setting = ar.GetActivitySetting(User.Identity.Name);
                    if (act_setting != null)
                    {
                        if (!act_setting.UnwantedActorIds.Contains(memberId))
                        {
                            var unwanted_actor_ids = act_setting.UnwantedActorIds;
                            unwanted_actor_ids.Add(memberId);
                            act_setting.UnwantedActorIds = unwanted_actor_ids;
                            ar.Save();
                        }
                        ar.Save();
                    }
                    else
                    {
                        act_setting = ActivitySetting.Default;
                        act_setting.MemberId = User.Identity.Name;
                        act_setting.UnwantedActors = memberId;
                        ar.Insert(act_setting);
                    }
                    return Json(new { Done = true, ActivityId = id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult LikeSharedObject(int id)
        {
            try
            {
                using (var sr = new SharedObjectsRepository())
                {
                    sr.Like(id, User.Identity.Name);
                    return Json(new { Done = true, SharedObjectId = id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult DeleteActivity(int id)
        {
            try
            {
                using (var ar = new ActivityRepository())
                {
                    var act = ar.Get(id);
                    if (act is PostActivity)
                    {
                        var pr = new PostsRepository(ar.Context);
                        pr.Delete(((PostActivity)act).PostId);
                    }
                    else if (act is PostOnWallActivity)
                    {
                        var pr = new PostsRepository(ar.Context);
                        pr.Delete(((PostOnWallActivity)act).PostId);
                    }
                    else if (act is PhotoActivity)
                    {
                        var alrep = new AlbumsRepository(ar.Context);
                        alrep.DeletePhoto(((PhotoActivity)act).PhotoId);
                    }
                    else if (act is SharingActivity)
                    {
                        var or = new SharedObjectsRepository(ar.Context);
                        or.DeleteSharing(((SharingActivity)act).SharingId);
                    }
                    else
                    {
                        ar.Delete(act);
                    }
                    return Json(new { Done = true, ActivityId = id });
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
