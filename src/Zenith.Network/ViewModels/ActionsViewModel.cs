using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zenith.Client.Shared.Commands;

namespace Zenith.Network.ServerManager.ViewModels
{
    public class ActionsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IServerController _serverController = null;
        private ICommand _startCommand, _stopCommand, _restartCommand;
        bool _isRunning = false;

        public ActionsViewModel(IServerController serverController)
        {
            this._serverController = serverController;
        }

        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                    _startCommand = new RelayCommand<object>(OnStartClick);

                return _startCommand;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                    _stopCommand = new RelayCommand<object>(OnStopClick);

                return _stopCommand;
            }
        }

        public ICommand RestartCommand
        {
            get
            {
                if (_restartCommand == null)
                    _restartCommand = new RelayCommand<object>(OnRestartClick);

                return _restartCommand;
            }
        }

        public void OnStartClick(object parameters)
        {
            this._serverController.Start();
            IsRunning = true;
        }

        public void OnStopClick(object parameters)
        { 
            this._serverController.Stop();
            IsRunning = false;
        }

        public void OnRestartClick(object parameters)
        {
            IsRunning = false;
            this._serverController.Restart();
            IsRunning = true;
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                NotifyPropertyChanged("IsRunning");
                NotifyPropertyChanged("IsReady");
            }
        }

        public bool IsReady
        {
            get { return !_isRunning; }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum UserAction
    {
        Start,
        Stop,
        Restart
    }
}
