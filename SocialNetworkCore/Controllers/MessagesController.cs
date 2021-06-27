using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreHelpers;
using System.IO;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;
using SocialNetApp.Views.Models.Modules;


namespace SocialNetApp.Controllers
{
    [Authorize(Roles = MyRoles.Member)]
    public class MessagesController : MainController
    {
        #region GET ACTIONS:
        [AcceptAjax]
        public ActionResult ShowDeleteConversationDialog(string id)
        {
            try
            {
                using (var mr = new MessagesRepository())
                {
                    var conv = mr.FindConversation(User.Identity.Name, id);
                    var model = DeleteConversationDialogModel.Create(conv, mr.Context);
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_DeleteConversationDialog.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ErrorDialogModel.Create(ex.Message, null));
            }
        }

        [AcceptAjax]
        public ActionResult ShowNewMessageDialog(string to)
        {
            try
            {
                using (var mr = new MembersRepository())
                {
                    var reciever_ids = MyHelper.SplitString(to, ";");
                    var recievers = mr.GetByUserName(reciever_ids);
                    var model = NewMessageDialogModel.Create(recievers.ToList(), mr.Context);
                    Response.CacheControl = "no-cache";
                    return PartialView(Urls.ModuleViews + "_NewMessageDialog.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                return PartialView(Urls.ModuleViews + "_ErrorDialog.cshtml", ex);
            }
        }
        #endregion

        #region Post ACTIONS:
        [HttpPost]
        [AcceptAjax]
        public ActionResult LoadConversation(FormCollection form)
        {
            try
            {
                using (var mr = new MessagesRepository())
                {
                    var convId = form["ConversationId"].ToString();
                    var conv = mr.FindConversation(User.Identity.Name, convId);
                    string html = "";
                    foreach (var msg in conv.Messages)
                    {
                        var msgModel = MessageModel.Create(msg, mr.Context);
                        html += RenderViewToString(Urls.ModuleViews + "_Message.cshtml", msgModel);
                        if (msg is InboxMessage)
                        {
                            // set inbox msg as read:
                            msg.Status = (byte)InboxMessageStatus.Read;
                            msg.StatusInfo = MyHelper.Now.ToString();
                            // set associated outbox msg as delivered:
                            var associated_outbox_msg = mr.GetOutboxMessage(((InboxMessage)msg).AssociatedOutboxMsgId.Value);
                            if (associated_outbox_msg != null)
                            {
                                associated_outbox_msg.Status = (byte)OutboxMessageStatus.Delivered;
                                associated_outbox_msg.StatusInfo = MyHelper.Now.ToString();
                            }
                        }
                    }
                    mr.Save();
                    return Json(new 
                    {
                        Done = true, 
                        PartialView = html, 
                        ConversationId = convId,
                        UpdatedUnreadsCount = mr.GetUnreadMessagesCount(User.Identity.Name)
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }

        #region Send Msssage:
        [HttpPost]
        [AcceptAjax]
        public ActionResult SendMessage(string Text, string OpponentId, bool ReturnSentMessage)
        {
            try
            {
                using (var mr = new MembersRepository())
                using (var msgRep = new MessagesRepository(mr.Context))
                {
                    var context = mr.Context;
                    var oppIds = MyHelper.SplitString(OpponentId, ";").ToArray();
                    // create obj for tracking sending results:
                    var sendingResults = new List<MessageSendingResult>();
                    foreach (var oppId in oppIds)
                    {
                        var opponent = GetMemberByIdOrUserName(oppId, context);
                        // is allowed to send message:
                        if (!opponent.IsAllowedToSendMessage(User.Identity.Name, context))
                        {
                            sendingResults.Add(new MessageSendingResult
                            {
                                SendStatus = false,
                                Message = Resources.Messages.NotAllowedToSendMessage,
                                Receiver = opponent
                            });
                            continue;
                        }
                        var res = msgRep.SendAMessage(User.Identity.Name, opponent.Email, Text);
                        sendingResults.Add(new MessageSendingResult()
                        {
                            SendStatus = res.Item1,
                            Receiver = opponent,
                            Message = (!res.Item1 ? res.Item4.FirstOrDefault() : Resources.Messages.MessageSent),
                            SentMessage = res.Item3
                        });
                    }
                    return Json(new
                    {
                        Done = true,
                        Messages = sendingResults.Select(res => new
                        {
                            FullName = res.Receiver.FullName,
                            SendingResult = res.SendStatus,
                            Message = res.Message,
                            SentMessageView = (ReturnSentMessage && res.SentMessage != null ? 
                                RenderViewToString(Urls.ModuleViews + "_Message.cshtml", MessageModel.Create(res.SentMessage, context)) : 
                                "")
                        }).ToArray()
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.Message } });
            }
        }
        #endregion

        [HttpPost]
        [AcceptAjax]
        public ActionResult DeleteConversation(string id)
        {
            try
            {
                using (var mr = new MessagesRepository())
                {
                    var conv = mr.FindConversation(User.Identity.Name, id);
                    // delete messages:
                    foreach (var msg in conv.Messages)
                    {
                        mr.Remove(msg);
                    }
                    mr.Save();
                    // return:
                    return Json(new { Done = true, ConversationId = id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Done = false, Errors = new string[] { ex.InnerException.Message } });
            }
        }
        #endregion

        #region PRIVATE METHODS:
        private Member GetMemberByIdOrUserName(string EmailUserName, System.Data.Objects.ObjectContext context)
        {
            var mr = new MembersRepository(context);
            if (!MyHelper.IsEmailAddress(EmailUserName))
            {
                return mr.GetByUserName(EmailUserName);
            }
            else
            {
                Member m = mr.Get(EmailUserName);
                if (m != null)
                    return m;
                else
                {
                    m = mr.GetByUserName(EmailUserName);
                    return m;
                }
            }
        }
        #endregion
    }
}
