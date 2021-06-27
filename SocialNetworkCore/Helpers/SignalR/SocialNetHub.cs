using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using SocialNetApp.Models;
using SocialNetApp.Models.Repository;

namespace CoreHelpers
{
    public class SocialNetHub : Hub
    {
        public static void SendNotifySignals(IEnumerable<NotifySignal> signals)
        {
            foreach(var signal in signals)
            {
                var cntx = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<SocialNetHub>();
                cntx.Clients.Client(signal.ConnectionId).notify((int)signal.Type);
            }
        }

        public static void SendChatMessageSignals(IEnumerable<ChatMessageSignal> signals)
        {
            foreach (var signal in signals)
            {
                var cntx = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<SocialNetHub>();
                cntx.Clients.Client(signal.ConnectionId).updateChat(signal);
            }
        }

        public static void SendChatMessageDeliverySignals(IEnumerable<ChatMessageDeliverySignal> signals)
        {
            foreach (var signal in signals)
            {
                var cntx = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<SocialNetHub>();
                cntx.Clients.Client(signal.ConnectionId).chatMessageDelivered(signal);
            }
        }

        public void SetMessageAsRead(ChatMessage msg, bool saveStatus)
        {
            if (saveStatus)
                ChatHelper.SetMessageAsRead(msg);
            // tell sender about message delivery:
            var cr = new ConnectionsRepository();
            var connections = cr.FindConnections(msg.From).ToList();
            var signals = connections.Select(con => new ChatMessageDeliverySignal { ConnectionId = con.Id, SessionId = msg.ChatSessionId, MessageId = msg.Id });
            if (connections.Any())
            {
                SocialNetHub.SendChatMessageDeliverySignals(signals);
            }
        }

        public override Task OnConnected()
        {
            var cr = new ConnectionsRepository();
            cr.Insert(new SignalConnection 
            { 
                Id = Context.ConnectionId, 
                MemberId = (!String.IsNullOrEmpty(Context.User.Identity.Name) ? Context.User.Identity.Name : ""), 
                DateOfEstablish = MyHelper.Now 
            });
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            var cr = new ConnectionsRepository();
            cr.Delete(Context.Request.GetHttpContext().ApplicationInstance.Context, Context.ConnectionId);
            return base.OnDisconnected();
        }
    }
}