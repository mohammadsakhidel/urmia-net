using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreHelpers;
using System.Data.Objects;
using System.Transactions;

namespace SocialNetApp.Models.Repository
{
    public class MessagesRepository : IDisposable
    {
        SocialNetDbEntities db = null;
        #region Constructors:
        public MessagesRepository()
        {
            db = new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }

        public MessagesRepository(System.Data.Objects.ObjectContext context)
        {
            db = context != null ? (SocialNetDbEntities)context : new SocialNetDbEntities(WebConfigs.EntitiesConnectionString);
        }
        #endregion

        #region Properties:
        public System.Data.Objects.ObjectContext Context
        {
            get
            {
                return db;
            }
        }
        #endregion
        //**********************************************************************************
        #region SELECT:
        public int GetUnreadMessagesCountGTDate(string memberId, DateTime dt)
        {
            return db.BoxMessages.OfType<InboxMessage>()
                .Where(msg => msg.MemberId == memberId && msg.Status == (byte)InboxMessageStatus.Unread && msg.DateOfSend > dt)
                .Count();
        }

        public IEnumerable<Conversation> FindConversations(string memberId)
        {
            var q = (from msg in db.BoxMessages.Include("MessageContent")
                     where msg.MemberId == memberId
                     orderby msg.DateOfSend descending
                     group msg by msg.OpponentId into g
                     let lastMsg = g.OrderByDescending(m => m.DateOfSend).FirstOrDefault()
                     select new Conversation() { 
                        ThisMemberId = memberId,
                        ThatMemberId = g.Key,
                        ThisMember = (from mem in db.Members where mem.Email == memberId select mem).FirstOrDefault(),
                        ThatMember = (from mem in db.Members where mem.Email == g.Key select mem).FirstOrDefault(),
                        UnreadsCount = g.OfType<InboxMessage>().Where(m => m.Status == (byte)InboxMessageStatus.Unread).Count(),
                        BriefText = lastMsg is OutboxMessage ?
                            Resources.Words.You + ": " + lastMsg.MessageContent.Text :
                            lastMsg.MessageContent.Text,
                        LastMessageDate = lastMsg.DateOfSend
                     })
                     .Take(Digits.ConversationsPageSize).OrderByDescending(c => c.LastMessageDate).ToList();
            return q;
        }

        public Conversation FindConversation(string memberId, string opponentAlias)
        {
            MembersRepository mr = new MembersRepository(db);
            var opponentMemberId = mr.GetByUserName(opponentAlias).Email;
            var msgs = (from msg in db.BoxMessages.Include("MessageContent")
                        where msg.MemberId == memberId && msg.OpponentId == opponentMemberId
                        orderby msg.DateOfSend descending
                        select msg).Take(Digits.ConversationMessagesPageSize).OrderBy(m => m.DateOfSend);
            if (msgs.Any())
            {
                var convLastMsg = msgs.OrderByDescending(m => m.DateOfSend).FirstOrDefault();
                return new Conversation()
                {
                    ThisMemberId = memberId,
                    ThatMemberId = opponentMemberId,
                    ThisMember = (from mem in db.Members where mem.Email == memberId select mem).FirstOrDefault(),
                    ThatMember = (from mem in db.Members where mem.Email == opponentMemberId select mem).FirstOrDefault(),
                    Messages = msgs.ToList(),
                    UnreadsCount = msgs.OfType<InboxMessage>().Where(m => m.Status == (byte)InboxMessageStatus.Unread).Count(),
                    BriefText = convLastMsg is OutboxMessage ?
                        String.Format("{0}: {1}", Resources.Words.You, convLastMsg.MessageContent.Text) :
                        convLastMsg.MessageContent.Text,
                    LastMessageDate = convLastMsg.DateOfSend
                };
            }
            else
            {
                return null;
            }
        }

        public int GetUnreadMessagesCount(string memberId)
        {
            return db.BoxMessages.OfType<InboxMessage>()
                .Where(msg => msg.MemberId == memberId && msg.Status == (byte)InboxMessageStatus.Unread)
                .Count();
        }

        public OutboxMessage GetOutboxMessage(int outMsgId)
        {
            try
            {
                return db.BoxMessages.OfType<OutboxMessage>()
                    .Single(m => m.Id == outMsgId);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<BoxMessage> GetUnreadMessages(string memberId, int count)
        {
            var q = (from msg in db.BoxMessages.OfType<InboxMessage>().Include("MessageContent")
                    where msg.MemberId == memberId && msg.Status == (byte)InboxMessageStatus.Unread
                    orderby msg.DateOfSend descending
                    select msg).Take(count);
            return q;
        }
        #endregion

        #region INSERT:
        public Tuple<bool, InboxMessage, OutboxMessage, string[]> SendAMessage(string frm, string to, string txt)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    // add message content:
                    var _messageContent = new MessageContent()
                    {
                        Text = txt
                    };
                    if (!_messageContent.IsValid)
                        return new Tuple<bool, InboxMessage, OutboxMessage, string[]>(false, null, null,
                            _messageContent.ValidationErrors.Select(ve => ve.Value).ToArray());
                    db.MessageContents.AddObject(_messageContent);
                    Save();
                    // add message to sender's outbox:
                    var _outboxMessage = new OutboxMessage()
                    {
                        MemberId = frm,
                        MessageContentId = _messageContent.Id,
                        OpponentId = to,
                        Status = (byte)OutboxMessageStatus.Sent,
                        StatusInfo = "",
                        DateOfSend = MyHelper.Now
                    };
                    db.BoxMessages.AddObject(_outboxMessage);
                    Save();
                    // add message to receiver's inbox:
                    var _inboxMessage = new InboxMessage()
                    {
                        MemberId = to,
                        MessageContentId = _messageContent.Id,
                        OpponentId = frm,
                        Status = (byte)InboxMessageStatus.Unread,
                        StatusInfo = "",
                        DateOfSend = MyHelper.Now,
                        AssociatedOutboxMsgId = _outboxMessage.Id
                    };
                    db.BoxMessages.AddObject(_inboxMessage);
                    Save();
                    // notify if online:
                    if (OnlinesHelper.IsOnline(to))
                    {
                        var cr = new ConnectionsRepository();
                        var connections = cr.FindConnections(to).ToList();
                        var signals = connections.Select(con => new NotifySignal { Type = NotifySignalType.NewMessage, ConnectionId = con.Id });
                        if (connections.Any())
                        {
                            SocialNetHub.SendNotifySignals(signals);
                        }
                    }
                    else //send email notification if needed:
                    {
                        var mr = new MembersRepository(db);
                        var reciever = mr.Get(to);
                        var sett = reciever.NotificationSetting ?? NotificationSetting.Default;
                        if (sett.OnNewMessage && reciever.LastEmailNotificationsDate.AddHours(Digits.EmailNotificationWindowHours) < MyHelper.Now)
                        {
                            EmailHelper.SendEmail(reciever.Email, Resources.Emails.ENotificationSubject, EmailHelper.GetGeneralEmailNotificationText(reciever));
                            reciever.LastEmailNotificationsDate = MyHelper.Now;
                            mr.Save();
                        }
                    }
                    ////////////
                    ts.Complete();
                    return new Tuple<bool, InboxMessage, OutboxMessage, string[]>(
                        true, _inboxMessage, _outboxMessage, new string[] { }
                        );
                }
            }
            catch (Exception ex)
            {
                return new Tuple<bool, InboxMessage, OutboxMessage, string[]>(
                    false,
                    null,
                    null,
                    new string[] { ex.Message }
                    );
            }
        }
        #endregion

        #region DELETE:
        public void Remove(BoxMessage msg)
        {
            var msgContent= msg.MessageContent;
            db.BoxMessages.DeleteObject(msg);
            /////// delete message content if not used any more:
            if (!msgContent.BoxMessages.Any())
            {
                db.MessageContents.DeleteObject(msgContent);
            }
        }
        #endregion

        #region UPDATE:

        #endregion

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}