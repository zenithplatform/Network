using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zenith.Network.TestClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        UserControl _currentView = null;

        public UserControl CurrentView
        {
            get
            {
                return _currentView;
            }

            set
            {
                _currentView = value;
                NotifyPropertyChanged("CurrentView");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
