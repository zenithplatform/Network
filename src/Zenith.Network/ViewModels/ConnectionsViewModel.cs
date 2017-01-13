using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zenith.Client.Shared.Commands;
using Zenith.Client.Shared.Util;
using Zenith.Network.Api;
using Zenith.Network.Servers.Infrastructure;

namespace Zenith.Network.ServerManager.ViewModels
{
    public class ConnectionsViewModel:INotifyPropertyChanged
    {
        private IServerController _serverController = null;
        private ICommand _disconnectCommand, _copyCommand, _filterCommand;
        private static readonly object _syncObject = new object();
        ThreadSafeObservableCollection<ConnectionItem> _connections = null;
        //ThreadSafeObservableCollection<ConnectionItem> _orphanedConnections = null;
        bool _isEmpty = true;
        ConnectionItem _selectedItem = null;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _filterVisible = false;

        public ConnectionsViewModel(IServerController serverController)
        {
            this._serverController = serverController;
            _connections = new ThreadSafeObservableCollection<ConnectionItem>();
            _connections.CollectionChanged += _connections_CollectionChanged;
        }

        private void _connections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<ConnectionItem> coll = (ObservableCollection<ConnectionItem>)sender;

            if(coll.Count > 0)
                IsEmpty = false;
            else
                IsEmpty = true;
        }

        public ObservableCollection<ConnectionItem> Connections
        {
            get
            {
                //return new ObservableCollection<ConnectionItem> {   new ConnectionItem() { IsConnected= true, Time = DateTime.Now, Address = "192.168.0.1", Identifier = Guid.NewGuid().ToString(), Status = NodeStatus.Connected },
                //                                                    new ConnectionItem() { IsConnected= false, Time = DateTime.Now, Address = "192.168.0.1", Identifier = Guid.NewGuid().ToString(), Status = NodeStatus.Disconnected },
                //                                                    new ConnectionItem() { IsConnected= true, Time = DateTime.Now, Address = "192.168.0.1", Identifier = Guid.NewGuid().ToString(), Status = NodeStatus.Connected },
                //                                                    new ConnectionItem() { IsConnected= true, Time = DateTime.Now, Address = "192.168.0.1", Identifier = Guid.NewGuid().ToString() , Status = NodeStatus.Connected},
                //                                                    new ConnectionItem() { IsConnected= false, Time = DateTime.Now, Address = "192.168.0.1", Identifier = Guid.NewGuid().ToString() , Status = NodeStatus.Disconnected}
                //};

                return _connections;
            }
        }

        public void AddConnection(NodeConnectedEvent e)
        {
            ConnectionItem item = new ConnectionItem() {
                IsConnected = true,
                Time = DateTime.Now,
                Address = string.Format("{0} on port {1}", e.RemoteEndpoint.Address, e.RemoteEndpoint.Port),
                Identifier = e.Id,
                Name = e.Name,
                Status = NodeStatus.Connected
            };

            _connections.Add(item);
            NotifyPropertyChanged("Connections");
        }

        public void UpdateConnectionStatus(string identifier, NodeStatus status)
        {
            ConnectionItem item = null;

            lock (_syncObject)
            {
                try
                {
                    IEnumerable<ConnectionItem> _tempItems = _connections.Where(it => it.Identifier.Equals(identifier));

                    if(_tempItems != null && _tempItems.Count() > 0)
                    {
                        item = _tempItems.First();

                        if (item != null)
                        {
                            item.Status = status;

                            if (status == NodeStatus.Connected)
                                item.IsConnected = true;
                            else if (status == NodeStatus.Disconnected)
                            {
                                item.Identifier = string.Empty;
                                item.IsConnected = false;
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        public void UpdateConnectionActivity(string identifier, NodeActivity activity)
        {
            ConnectionItem item = null;

            lock (_syncObject)
            {
                try
                {
                    IEnumerable<ConnectionItem> _tempItems = _connections.Where(it => it.Identifier.Equals(identifier) && it.IsConnected == true);

                    if(_tempItems != null && _tempItems.Count() > 0)
                    {
                        item = _tempItems.First();

                        if (item != null)
                        {
                            item.NodeActivity = activity;
                        }
                    }
                }
                catch
                {

                }
            }
        }

        public void ClearAllConnections()
        {
            _connections.Clear();
            NotifyPropertyChanged("Connections");
        }

        public InvertableBool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                NotifyPropertyChanged("IsEmpty");
                NotifyPropertyChanged("SelectionNonEmpty");
                NotifyPropertyChanged("CanDisconnect");
            }
        }

        public bool FilterVisible
        {
            get { return _filterVisible; }
            set
            {
                _filterVisible = value;
                NotifyPropertyChanged("FilterVisible");
            }
        }

        public int RecordCount
        {
            get { return this.Connections.Count; }
        }

        public int ActiveNodesCount
        {
            get
            {
                lock(_syncObject)
                {
                    return _connections.Count(it => it.IsConnected == true);
                }
                
            }
        }

        public string EmptyGridMessage
        {
            get { return "No active connections yet."; }
        }

        public ConnectionItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged("SelectionNonEmpty");
                NotifyPropertyChanged("CanDisconnect");
            }
        }

        public ICommand DisconnectCommand
        {
            get
            {
                if (_disconnectCommand == null)
                    _disconnectCommand = new RelayCommand<object>(OnDisconnectClick);

                return _disconnectCommand;
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                    _copyCommand = new RelayCommand<object>(OnCopyClick);

                return _copyCommand;
            }
        }

        public ICommand FilterCommand
        {
            get
            {
                if (_filterCommand == null)
                    _filterCommand = new RelayCommand<object>(OnFilterClick);

                return _filterCommand;
            }
        }

        public void OnDisconnectClick(object parameter)
        {
            string identifier = this.SelectedItem.Identifier;
            this.UpdateConnectionActivity(identifier, NodeActivity.Closing);
                
            Task.Factory.StartNew(() => 
            {
                //this._serverController.DisconnectNode(identifier);
            },                
                TaskCreationOptions.LongRunning
            ).ContinueWith(task =>
            {
                NotifyPropertyChanged("Connections");
                NotifyPropertyChanged("CanDisconnect");
            });
        }

        public void OnCopyClick(object parameter)
        {
            ConnectionItem item = this.SelectedItem;
            Clipboard.SetText(item.ToString());
        }

        public void OnFilterClick(object parameter)
        {

        }

        public bool SelectionNonEmpty
        {
            get { return !_isEmpty && this.SelectedItem != null; }
        }

        public bool CanDisconnect
        {
            get
            {
                return (this.SelectedItem != null && !_isEmpty && this.SelectedItem.IsConnected == true);
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ConnectionItem : INotifyPropertyChanged
    {
        private bool _isConnected = false;
        private NodeStatus _status = NodeStatus.Unknown;
        private NodeActivity _activity = NodeActivity.Idle;
        private string _activityLegend = "", _identifier = "", _name = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectionItem()
        {
            
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }

        public DateTime Time { get; set; }

        public string Identifier
        {
            get { return _identifier; }
            set
            {
                _identifier = value;
                NotifyPropertyChanged("Identifier");
            }
        }

        public string Address { get; set; }

        public NodeStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public NodeActivity NodeActivity
        {
            get { return _activity; }
            set
            {
                _activity = value;
                NotifyPropertyChanged("NodeActivity");
                NotifyPropertyChanged("ActivityText");
            }
        }

        public string ActivityText
        {
            get
            {
                switch(this.NodeActivity)
                {
                    case NodeActivity.Sending:
                        _activityLegend = "Sending...";
                        break;
                    case NodeActivity.Receiving:
                        _activityLegend = "Receiving...";
                        break;
                    case NodeActivity.Idle:
                        _activityLegend = "Idle";
                        break;
                    case NodeActivity.Closing:
                        _activityLegend = "Closing...";
                        break;
                }

                return _activityLegend;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
