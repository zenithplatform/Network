using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.Shared.Configuration;

namespace Zenith.Network.Api.Infrastructure
{
    public class HubClient
    {
        string _uri = "", _hubName = "";
        HubConnection _connection = null;
        IHubProxy _proxy = null;
        private readonly JsonSerializer _serializer = null;

        public HubClient()
        {
            HubConnectionInfo info = ConfigHelper.ObjectFromConfig<HubConnectionInfo>("HubConnectionInfo");

            _uri = info.ServerUri;
            _hubName = info.HubName;
            _serializer = new JsonSerializer();
        }

        public HubClient(string uri, string hubName)
        {
            this._uri = uri;
            this._hubName = hubName;
            _serializer = new JsonSerializer();
        }

        public async Task<bool> Connect()
        {
            bool result = true;
            try
            {
                PrepareConnection();
                await _connection.Start();

                result = true;
            }
            catch (HttpClientException httpExc)
            {
                result = false;
                throw;
            }
            catch (Exception exc)
            {
                result = false;
                throw;
            }
            finally
            {

            }

            return result;
        }

        protected virtual void PrepareConnection()
        {
            _connection = new HubConnection(_uri);

            _connection.Closed += OnConnectionClosed;
            _connection.Error += OnConnectionError;
            _connection.Received += OnConnectionReceived;
            _connection.Reconnecting += OnConnectionRecconecting;
            _connection.Reconnected += OnConnectionReconnected;
            _connection.StateChanged += OnConnectionStateChanged;

            _connection.JsonSerializer = _serializer;

            _proxy = _connection.CreateHubProxy(_hubName);
        }

        protected void AddCallback<T>(string eventName, Action<T> callback)
        {
            _proxy.On<T>(eventName, callback);
        }

        public Task<T> Call<T>(string method, params object[] args) where T : class
        {
            return _proxy.Invoke<T>(method, args);
        }

        public Task Call(string method, params object[] args)
        {
            return _proxy.Invoke(method, args);
        }

        protected virtual void OnConnectionClosed()
        {

        }

        protected virtual void OnConnectionError(Exception exc)
        {

        }

        protected virtual void OnConnectionReceived(string data)
        {

        }

        protected virtual void OnConnectionRecconecting()
        {

        }

        protected virtual void OnConnectionReconnected()
        {

        }

        protected virtual void OnConnectionStateChanged(StateChange state)
        {

        }

        protected IHubProxy HubProxy
        {
            get { return _proxy; }
        }

        protected HubConnection Connection
        {
            get { return _connection; }
        }

        public bool Connected
        {
            get { return _connection != null && _connection.State == ConnectionState.Connected; }
        }

        public string ConnectionId
        {
            get { return this._connection.ConnectionId; }
        }
    }

    internal class HubConnectionInfo
    {
        internal string ServerUri { get; set; }
        internal string HubName { get; set; }
    }
}
