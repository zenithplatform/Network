using System;
using System.Collections.Generic;
using Zenith.Core.DataProviders;
using Zenith.Core.DataProviders.Base;
using Zenith.Core.DataProviders.Enums;
using Zenith.Core.DataProviders.Metadata;
using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.Api.Storage
{
    public class NodeStorage : INodeStorage
    {
        private IDataProvider _dataProvider = null;

        public NodeStorage(IDataProvider provider)
        {
            _dataProvider = provider;
        }

        public DataPayload<T> CreatePayload<T>(T data) where T : class, new()
        {
            DataPayload<T> payload = new DataPayload<T>();
            payload.DbName = ObjectMetadataReader.GetDbName(data);
            payload.TableName = ObjectMetadataReader.GetTableName(data);
            payload.UniqueIdFieldName = ObjectMetadataReader.GetUniqueIdentifier(data);
            payload.Entity = data;

            return payload;
        }

        public DataPayload<T> CreatePayload<T>(T data, string targetDbName) where T : class, new()
        {
            DataPayload<T> payload = new DataPayload<T>();
            payload.DbName = targetDbName;
            payload.TableName = ObjectMetadataReader.GetTableName(data);
            payload.UniqueIdFieldName = ObjectMetadataReader.GetUniqueIdentifier(data);
            payload.Entity = data;

            return payload;
        }

        public DataStorageResult DeleteNode<T>(string identifier) where T : class, new()
        {
            return null;
            //return ExecuteOperation<T>(Operation.Delete, new object[] { identifier });
        }

        public DataStorageResult<T> QueryNode<T>(string identifier) where T : class, new()
        {
            return null;
            //return ExecuteOperation<T>(Operation.Query, new object[] { identifier }) as DataStorageResult<T>;
        }

        public DataStorageResult<T> QueryNodes<T>(IEnumerable<NodeIdentifier> identifiers, bool all) where T : class
        {
            return null;
            //return ExecuteOperation<T>(Operation.Query, new object[] { identifiers, all }) as DataStorageResult<T>;
        }

        public DataStorageResult SaveNode<T>(T data) where T : class, new()
        {
            DataPayload<T> payload = CreatePayload<T>(data);

            if (!_dataProvider.IsOpen)
                _dataProvider.Open();

            return _dataProvider.Execute<T>(Operation.Insert, payload);
        }

        //private StorageResult ExecuteOperation<T>(Operation operation, params object[] args) where T : class, new()
        //{
        //    StorageResult result = new StorageResult();

        //    if (!_dataProvider.IsOpen)
        //        _dataProvider.Open();

        //    try
        //    {
        //        //switch (operationType)
        //        //{
        //        //    case OperationType.Delete:
        //        //        break;
        //        //    case OperationType.Query:
        //        //        break;
        //        //    case OperationType.Save:
        //        //        _dataProvider.Execute<T>(operationType, null);
        //        //        break;
        //        //}

        //        DataPayload<T> payload = new DataPayload<T>();
        //        payload.
        //        _dataProvider.Execute<T>(operation, null);
        //        //switch (operationType)
        //        //{
        //        //    case OperationType.Delete:
        //        //        break;
        //        //    case OperationType.Query:
        //        //        break;
        //        //    case OperationType.Save:
        //        //        _dataProvider.Execute<T>(operationType, null);
        //        //        break;
        //        //}

        //        result.Success = true;
        //    }
        //    catch (Exception exc)
        //    {
        //        result.Success = false;
        //        result.Error = exc;
        //    }
        //    finally
        //    {

        //    }

        //    return result;
        //}
    }

    //public enum OperationType
    //{
    //    Save,
    //    Delete,
    //    Query,
    //    Unknown
    //}

    //public enum Operation
    //{
    //    Insert,
    //    Update,
    //    Delete,
    //    Query
    //}
}
