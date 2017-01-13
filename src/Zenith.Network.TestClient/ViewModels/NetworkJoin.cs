using System;
using Autofac;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Zenith.Client.Shared.Commands;
using Zenith.Client.Shared.ViewsModels;
using System.Windows;
using Zenith.Network.Client;
using Zenith.Network.Api.Interfaces;
using Zenith.Network.Api.Infrastructure;

namespace Zenith.Network.TestClient.ViewModels
{
    public class NetworkJoinModel : ViewModelBase
    {
        string _currentStatus = "";
        bool _progressVisible = false;
        State _currentState = State.Ready;
        ZenithNetworkNode _node = null;

        private ICommand _mainCommand;
        private string _username = "", _commandImage = "";
        public event EventHandler OnConnected;

        CancellationTokenSource cts = null;
        Visibility _mainCommandVisible = Visibility.Visible;
        Exception _lastError = null;

        public NetworkJoinModel()
        {
            UpdateState(State.Ready);

            _node = new ZenithNetworkNode();
            _node.Initialize();
        }

        public string CommandImage
        {
            get { return _commandImage; }
            set
            {
                _commandImage = value;
                NotifyPropertyChanged(this, new PropertyChangedEventArgs("CommandImage"));
            }
        }

        public Visibility MainCommandVisible
        {
            get { return _mainCommandVisible; }
            set
            {
                _mainCommandVisible = value;
                NotifyPropertyChanged(this, new PropertyChangedEventArgs("MainCommandVisible"));
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyPropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                NotifyPropertyChanged(this, new PropertyChangedEventArgs("CurrentStatus"));
            }
        }

        public bool ProgressVisible
        {
            get { return _progressVisible; }
            set
            {
                _progressVisible = value;
                NotifyPropertyChanged(this, new PropertyChangedEventArgs("ProgressVisible"));
            }
        }

        public ICommand MainCommand
        {
            get
            {
                if (_mainCommand == null)
                    _mainCommand = new RelayCommand<object>(OnMainCommandClick);

                return _mainCommand;
            }
        }

        public void OnMainCommandClick(object parameter)
        {
            if(_currentState == State.Ready || _currentState == State.Error)
            {
                Connect();
            }
            else if(_currentState == State.Connecting)
            {
                Cancel();
            }
        }

        private async void Connect()
        {
            UpdateState(State.Connecting);
            cts = new CancellationTokenSource();
            bool result = false;

            try
            {
                result = await _node.Join();

                if (!cts.Token.IsCancellationRequested)
                {
                    if (result)
                        UpdateState(State.Connected);
                }
                else
                    UpdateState(State.Ready);
            }
            catch (Exception exc)
            {
                _lastError = exc;
                UpdateState(State.Error);
                
            }
            finally
            {
                ProgressVisible = false;
            }
        }

        private async void Cancel()
        {
            cts.Cancel();
            CurrentStatus = "Cancelling...";

            await Task.Delay(2000).ContinueWith(_ =>
            {
                UpdateState(State.Ready);
            });
        }

        private async void UpdateState(State state)
        {
            _currentState = state;

            switch (state)
            {
                case State.Ready:
                    CurrentStatus = "";
                    ProgressVisible = false;
                    CommandImage = "arrow_right_bold";
                    break;
                case State.Connecting:
                    CurrentStatus = "Joining network...";
                    ProgressVisible = true;
                    CommandImage = "pause_circle_outline";
                    break;
                case State.Connected:
                    CurrentStatus = "Successfully joined.";
                    MainCommandVisible = Visibility.Hidden;

                    await Task.Delay(2000).ContinueWith(_ =>
                    {
                        CurrentStatus = "Getting users...";
                        INetworkStack stack = _node.Startup.Container.Get<INetworkStack>();
                        //stack.Get("Global").BeginDiscovery();
                    });

                    await Task.Delay(2000).ContinueWith(_ =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ActiveUsersControl ctrl = new ActiveUsersControl();
                            ctrl.DataContext = new ActiveUsersModel();
                            App.Container.Resolve<MainViewModel>().CurrentView = ctrl;

                        });
                    });

                    break;
                case State.Error:
                    CommandImage = "arrow_right_bold";

                    if (_lastError == null)
                        CurrentStatus = "Unspecified error.";
                    else
                        CurrentStatus = string.Format("Not able to join : {0}.", _lastError.Message);

                    break;
            }
        }
    }

    public enum State
    {
        Ready,
        Connecting,
        Connected,
        Error
    }
}
