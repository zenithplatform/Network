using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zenith.Client.Shared.Commands;
using Zenith.Network.Api;
using Zenith.Network.Api.Interfaces;

namespace Zenith.Network.TestClient.ViewModels
{
    public class StartPageModel //: INotifyPropertyChanged
    {
        //INetworkConnector<Udt.Socket> _connector = null;

        public StartPageModel()
        {
            //_connector = new UdtConnector(new IPEndPoint(IPAddress.Any, 7777), new IPEndPoint(IPAddress.Parse("192.168.1.1"), 9999));
            //NetworkFileReceiver receiver = new NetworkFileReceiver(_connector);
            //NetworkFileSender sender = new NetworkFileSender(_connector);
            //NetworkTextMessageReceiver textReceiver = new NetworkTextMessageReceiver(_connector);
            //NetworkTextMessageSender textSender = new NetworkTextMessageSender(_connector);

            //textReceiver.Receive();
            //textSender.Send("Hi! How are you?");

            //sender.Send("D:\\test.txt");
            //receiver.Receive();

        }
        //private ICommand _connectCommand, _registerCommand;
        //private string _username = "";
        //public event PropertyChangedEventHandler PropertyChanged;
        //public event EventHandler OnConnected;
        //CommunicationCoordinator _coordinator = new CommunicationCoordinator();

        //public ICommand ConnectCommand
        //{
        //    get
        //    {
        //        if (_connectCommand == null)
        //            _connectCommand = new RelayCommand<object>(OnConnectClick);

        //        return _connectCommand;
        //    }
        //}

        //public ICommand RegisterCommand
        //{
        //    get
        //    {
        //        if (_registerCommand == null)
        //            _registerCommand = new RelayCommand<object>(OnRegisterClick);

        //        return _registerCommand;
        //    }
        //}

        //public string Username
        //{
        //    get { return _username; }
        //    set
        //    {
        //        _username = value;
        //        NotifyPropertyChanged("Username");
        //    }
        //}

        //public void OnConnectClick(object parameter)
        //{
        //    _coordinator.Connect();

        //    //if(result)
        //    //{
        //    //    if (OnConnected != null)
        //    //        OnConnected(this, EventArgs.Empty);
        //    //}
        //}

        //public void OnRegisterClick(object parameter)
        //{
        //    _coordinator.Register();

        //    //if(result)
        //    //{
        //    //    if (OnConnected != null)
        //    //        OnConnected(this, EventArgs.Empty);
        //    //}
        //}

        //public void NotifyPropertyChanged(string propertyName)
        //{
        //    if(PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
    }
}
