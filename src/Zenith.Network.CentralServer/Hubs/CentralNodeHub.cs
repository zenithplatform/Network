using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Zenith.Network.Servers.Infrastructure;
using Zenith.Network.Servers.Interfaces;
using Zenith.Network.Api;
using Zenith.Network.Api.Helpers;
using Zenith.Network.Api.Messages;
using Zenith.Network.Api.Serialization;
using Zenith.Network.Api.Interfaces;
using Zenith.Network.Api.Storage;
using Zenith.Core.DataProviders.Rethink;

namespace Zenith.Network.Servers
{
    [HubName("CentralHub")]
    public class CentralNodeHub : Hub
    {
        private ConcurrentDictionary<string, NodeMetadata> _activeNodes = null;
        private IHubRequestHandler _requestHandler = null;

        public CentralNodeHub(IHubRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void Push<T>(string identifier, T message) where T : HubResponseMessageBase
        {
            Clients.Client(identifier).Push(MessageSerializer.Serialize(message));
        }

        public void BroadcastExcept<T>(IEnumerable<string> identifiers, T message) where T : HubResponseMessageBase
        {
            Clients.AllExcept(identifiers.ToArray()).BroadcastExcept(MessageSerializer.Serialize(message));
        }

        public void Broadcast<T>(T message) where T : HubResponseMessageBase
        {
            Clients.All.Broadcast(MessageSerializer.Serialize(message));
        }

        public void DisconnectNode(NodeIdentifier identifier)
        {
            //Clients.Client(identifier).Disconnect();

            //try
            //{
            //    RequestHandlerResult result = _requestHandler.Execute(request.ConnectionId, request);
            //}
            //catch (Exception exc)
            //{

            //}
        }

        //public void RegisterBaseNode(HubRequestMessageBase message)
        //{

        //}

        public void RegisterNode(NodeRegistrationRequest request)
        {
            try
            {
                NodeConnectedEvent ev = new NodeConnectedEvent()
                {
                    Id = request.NodeMetadata.Identifier.NodeId,
                    RemoteEndpoint = request.NodeMetadata.AddressingInfo.PublicEndpoint.ToIPEndPoint(),
                    Timestamp = DateTime.Now,
                    Name = request.NodeMetadata.Identifier.Name
                };

                ServerEventsGateway.Instance.Trigger(ev);
                RequestHandlerResult result = _requestHandler.Execute(request.ConnectionId, request);

                ActiveNodesCache.ActiveNodes.TryAdd(request.ConnectionId, request.NodeMetadata);
                //this.Push<HubNotification>(request.ConnectionId, new HubNotification());
            }
            catch (Exception exc)
            {

            }
        }

        public void UnregisterNode(NodeRegistrationRequest request)
        {
            try
            {
                RequestHandlerResult result = _requestHandler.Execute(request.ConnectionId, request);
                NodeMetadata value = null;
                ActiveNodesCache.ActiveNodes.TryRemove(request.ConnectionId, out value);
            }
            catch (Exception exc)
            {

            }

            //if (!request.Unregister)
            //    return;

            //NodeMetadata removedNode = null;
            //_activeNodes.TryRemove(request.NodeMetadata.Identifier.NodeId, out removedNode);
        }

        public QueryNodesResponse QueryNodes(QueryNodesRequest request)
        {
            RequestHandlerResult result = null;

            try
            {
                result = _requestHandler.Execute(request.ConnectionId, request);
            }
            catch (Exception exc)
            {

            }

            return result.ReplyMessage as QueryNodesResponse;
        }

        public void RequestCommunication(CommunicationRequest request)
        {
            RequestHandlerResult origin = _requestHandler.Execute(request.ConnectionId, request); ;

            try
            {
                Parallel.Invoke(
                    () => this.Push<CommunicationStart>(ActiveNodesCache.GetConnectionId(request.TargetNodeId), new CommunicationStart() { Node = ActiveNodesCache.GetMetadata(request.OriginNodeId)}),
                    () => this.Push<CommunicationStart>(ActiveNodesCache.GetConnectionId(request.OriginNodeId), new CommunicationStart() { Node = ActiveNodesCache.GetMetadata(request.TargetNodeId) })
                );
            }
            catch (Exception exc)
            {

            }
        }

        //public void RequestCommunicationReply(CommunicationRequestCheckReply request)
        //{
        //    RequestHandlerResult result = null;

        //    try
        //    {
        //        if(request.Ready)
        //        {
        //            //KeyValuePair<string, NodeMetadata> pair = ActiveNodesCache.ActiveNodes
        //            //                                                      .SingleOrDefault(p => p.Value
        //            //                                                                             .Identifier
        //            //                                                                             .NodeId
        //            //                                                                             .Equals(request.TargetNodeId));
        //        }

        //        //this.Push<CommunicationAck>(request.ConnectionId, new CommunicationAck());
        //    }
        //    catch (Exception exc)
        //    {

        //    }
        //}
    }
}
