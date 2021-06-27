using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using SocialNetApp.Views.Models.Modules;
using System.IO;

namespace SocialNetApp.Controllers
{
    [Authorize(Roles = MyRoles.Member)]
    public class ChatingController : MainController
    {
        #region Post Actions:
        [HttpPost]
        [AcceptAjax]
        public ActionResult BeginChat(string from, string to)
        {
            using (var mr = new MembersRepository())
            {
                var from_member = mr.GetByUserName(from);
                var to_member = mr.GetByUserName(to);
                var session = ChatHelper.BeginChat(new List<string> { from_member.Email, to_member.Email });
                Response.CacheControl = "no-cache";
                var model = ChatDialogModel.Create(session.Id, mr.Context);
                return PartialView(Urls.ModuleViews + "_ChatDialog.cshtml", model);
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult ContinueChat(string sessionId)
        {
            try
            {
                var model = ChatDialogModel.Create(sessionId, null);
                Response.CacheControl = "no-cache";
                return Json(new
                {
                    Done = true,
                    PartialView = RenderViewToString(Urls.ModuleViews + "_ChatDialog.cshtml", model)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [AcceptAjax]
        [HttpPost]
        public ActionResult GetOlderChatMessages(string sessionId, DateTime olderThanDate)
        {
            try
            {
                var messages = ChatHelper.GetSessionMessages(sessionId, olderThanDate);
                var isMore = ChatHelper.IsMoreMessages(sessionId, messages.First().DateOfSend);
                ///// get part view:
                var partialView = "";
                foreach (var msg in messages)
                {
                    var msgModel = ChatMessageModel.Create(msg, null);
                    partialView += RenderViewToString(Urls.ModuleViews + "_ChatMessage.cshtml", msgModel);
                }
                /////
                return Json(new
                {
                    Done = true,
                    PartialView = partialView,
                    IsMore = isMore,
                    OldestMessageDate = messages.First().DateOfSend.ToString("yyyy-MM-dd HH:mm:ss.fff")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPost]
        [AcceptAjax]
        public ActionResult SendChatMessage(FormCollection form)
        {
            try
            {
                var chat_msg = new ChatMessage { ChatSessionId = form["ChatSessionId"], Text = form["Text"], DateOfSend = MyHelper.Now, From = form["From"], Status = (byte)ChatMessageStatus.New };
                // validate input:
                if (!chat_msg.IsValid)
                {
                    return Json(new
                    {
                        Done = false,
                        Errors = chat_msg.ValidationErrors.Select(ve => ve.Value).ToArray()
                    });
                }
                // add msg:
                var result_msg = ChatHelper.AddChatMessage(chat_msg);
                // send signal:
                chat_msg.CssExtension = "_otherside";
                chat_msg.PartialView = RenderViewToString(Urls.ModuleViews + "_ChatMessage.cshtml", ChatMessageModel.Create(chat_msg, null));
                var session = ChatHelper.GetChatSession(chat_msg.ChatSessionId);
                var other_people_in_session = session.Participants.Except(new List<string>() { chat_msg.From });
                foreach (var memberId in other_people_in_session)
                {
                    // sessions voew:
                    var sessions = ChatHelper.GetChatSessions(memberId);
                    var sessions_view = RenderViewToString(Urls.ModuleViews + "_ChatSessions.cshtml", ChatSessionsModel.Create(sessions, "", other_people_in_session.First(), null));
                    //
                    var cr = new ConnectionsRepository();
                    var connections = cr.FindConnections(memberId).ToList();
                    var signals = connections.Select(con => new ChatMessageSignal { ConnectionId = con.Id, Message = chat_msg, SessionsView = sessions_view });
                    if (connections.Any())
                    {
                        SocialNetHub.SendChatMessageSignals(signals);
                    }
                }
                chat_msg.CssExtension = "";
                //
                Response.CacheControl = "no-cache";
                var resultMsgModel = ChatMessageModel.Create(result_msg, null);
                return Json(new
                {
                    Done = true,
                    PartialView = RenderViewToString(Urls.ModuleViews + "_ChatMessage.cshtml", resultMsgModel)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Done = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }
        #endregion

        #region PRIVATE METHODS:
        #endregion
    }
}
