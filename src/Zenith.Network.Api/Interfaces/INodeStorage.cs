using System;
using System.Collections.Generic;
using Zenith.Core.DataProviders;
using Zenith.Core.DataProviders.Base;
using Zenith.Network.Api.Storage;

namespace Zenith.Network.Api.Interfaces
{
    public interface INodeStorage
    {
        DataStorageResult SaveNode<T>(T data) where T : class, new();
        DataStorageResult DeleteNode<T>(string identifier) where T : class, new();
        DataStorageResult<T> QueryNode<T>(string identifier) where T : class, new();
        DataStorageResult<T> QueryNodes<T>(IEnumerable<NodeIdentifier> identifiers, bool all) where T : class;
        DataPayload<T> CreatePayload<T>(T data) where T : class, new();
    }
}
