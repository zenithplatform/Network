using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.DataProviders;
using Zenith.Network.Api;
using Zenith.Network.Api.Interfaces;
using Zenith.Network.Api.Messages;
using Zenith.Network.Api.Storage;
using Zenith.Network.Servers.Interfaces;

namespace Zenith.Network.Servers.Infrastructure
{
    public class HubRequestHandler : IHubRequestHandler
    {
        INodeStorage _storage = null;
        //INodeServer _nodeServer = null;
        private static object _syncLock = new object();

        public HubRequestHandler(INodeStorage storage)
        {
            this._storage = storage;
        }
        //public HubRequestHandler(INodeServer nodeServer)
        //{
        //    _nodeServer = nodeServer;
        //}

        //public HubRequestHandler(INodeServer server, INodeStorage storage)
        //    : this(server)
        //{
        //    this._storage = storage;
        //}

        public RequestHandlerResult Execute<T>(string id, T request) where T : class
        {
            RequestHandlerResult result = null;

            try
            {
                result = ExecuteInternal(id, request);

                //result.PreReplyActions.ExecuteAll();

                //if (result.ShouldReply)
                //    _nodeServer.Push(id, result.ReplyMessage);

                //result.PostReplyActions.ExecuteAll();
            }
            catch (Exception exc)
            {

            }

            return result;
        }

        private RequestHandlerResult ExecuteInternal<T>(string callerId, T request) where T : class
        {
            RequestHandlerResult handlerResult = null;

            #region Handler logic

            try
            {
                lock (_syncLock)
                {
                    handlerResult = new RequestHandlerResult();

                    if (request != null)
                    {
                        if (request is NodeRegistrationRequest)
                        {
                            DataStorageResult result = null;
                            NodeRegistrationRequest regRq = request as NodeRegistrationRequest;

                            if (_storage != null)
                            {
                                if (regRq.Unregister)
                                {
                                    result = _storage.DeleteNode<NodeMetadata>(regRq.NodeMetadata.Identifier.NodeId);
                                }
                                else
                                {
                                    result = _storage.SaveNode(regRq.NodeMetadata);

                                    if (result.Success)
                                    {
                                        //handlerResult.PreReplyActions.Add(() => _nodeServer.RegisterNode(callerId, regRq.NodeMetadata));
                                        //TODO : Define better type for notifications
                                        //handlerResult.PostReplyActions.Add(() => _nodeServer.Broadcast(new HubNotification()));
                                    }
                                }

                                handlerResult.ReplyMessage = new NodeRegistrationResponse() { Success = result.Success, Error = result.Error };
                                handlerResult.ShouldReply = true;
                                handlerResult.HandlerExecutedSuccessfully = true;
                            }
                        }
                        else if (request is QueryNodesRequest)
                        {
                            DataStorageResult<IEnumerable<NodeMetadata>> result = null;
                            List<NodeMetadata> nodeList = new List<NodeMetadata>();
                            QueryNodesRequest queryRq = request as QueryNodesRequest;

                            if(_storage != null)
                            {
                                result = this._storage.QueryNodes<IEnumerable<NodeMetadata>>(queryRq.Identifiers, queryRq.All);
                                nodeList.AddRange(result.Context);
                            }

                            handlerResult.ReplyMessage = new QueryNodesResponse() { Success = result.Success, Error = result.Error, NodesMetadata = nodeList };
                            handlerResult.ShouldReply = true;
                            handlerResult.HandlerExecutedSuccessfully = true;
                        }
                        //else if(request is CommunicationRequest)
                        //{
                        //    DataStorageResult<NodeMetadata> result = null;
                        //    List<NodeMetadata> nodeList = new List<NodeMetadata>();
                        //    CommunicationRequest commRequest = request as CommunicationRequest;

                        //    if (_storage != null)
                        //    {
                        //        result = this._storage.QueryNode<NodeMetadata>(commRequest.TargetNodeId);
                        //        nodeList.Add(result.Context);
                        //    }

                        //    handlerResult.ReplyMessage = new QueryNodesResponse() { Success = result.Success, Error = result.Error, NodesMetadata = nodeList };
                        //    handlerResult.ShouldReply = true;
                        //    handlerResult.HandlerExecutedSuccessfully = true;
                        //}
                    }
                    else
                    {
                        handlerResult.HandlerExecutedSuccessfully = false;
                    }
                }
            }
            catch (Exception exc)
            {
                handlerResult.ExceptionDetails = exc;
                handlerResult.HandlerExecutedSuccessfully = false;
            }

            #endregion

            return handlerResult;
        }
    }
}
