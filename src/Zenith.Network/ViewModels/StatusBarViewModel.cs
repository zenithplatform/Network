using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.ServerManager.ViewModels
{
    public class StatusBarViewModel : INotifyPropertyChanged
    {
        private string _currentStatus = "";
        private string _message = "";

        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                NotifyPropertyChanged("CurrentStatus");
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyPropertyChanged("Message");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
