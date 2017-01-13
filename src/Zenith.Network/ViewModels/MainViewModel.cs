using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Zenith.Client.Shared.Util;
using Zenith.Network.Servers.Infrastructure;

namespace Zenith.Network.ServerManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private InfoPanelViewModel _info = null;
        private ActionsViewModel _manageModel = null;
        private ConnectionsViewModel _connections = null;
        private StatusBarViewModel _statusBar = null;
        private object _syncObj = new object();
        private PreferencesViewModel _preferences = null;
        private SystemTray _tray = null;
        private IServerController _serverController = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(IServerController serverController)
        {
            this._serverController = serverController;

            _info = new InfoPanelViewModel();
            _info.Initialize();

            _manageModel = new ActionsViewModel(this._serverController);

            _statusBar = new StatusBarViewModel();
            _statusBar.CurrentStatus = "Ready.";

            _connections = new ConnectionsViewModel(this._serverController);

            _preferences = new PreferencesViewModel();
            _preferences.PropertyChanged += _preferences_PropertyChanged;

            ServerEventHandlers handlers = new ServerEventHandlers(this);
            handlers.Register(ServerEventsGateway.Instance);

            _tray = new SystemTray(Application.Current.MainWindow, Zenith.Assets.Properties.Resources.chart_bubble);
        }

        private void _preferences_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("MinimizeInTrayOnClose"))
            {
                if (_preferences.MinimizeInTrayOnClose)
                    _tray.Activate();
                else
                    _tray.Deactivate();
            }
        }

        public string Title
        {
            get { return string.Format("Zenith central node server (on {0})", Environment.MachineName); }
        }

        public InfoPanelViewModel Info
        {
            get
            {
                return _info;
            }
        }

        public ActionsViewModel Actions
        {
            get
            {
                return _manageModel;
            }
        }

        public ConnectionsViewModel Connections
        {
            get
            {
                return _connections;
            }
        }

        public StatusBarViewModel StatusBar
        {
            get
            {
                return _statusBar;
            }
        }

        public PreferencesViewModel UserPreferences
        {
            get
            {
                return _preferences;
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
