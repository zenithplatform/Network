using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.TestClient.ViewModels
{
    public class ActiveUsersModel : INotifyPropertyChanged
    {
        List<string> _activeUsers = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public ActiveUsersModel()
        {
            _activeUsers = new List<string>();
            _activeUsers.Add("Test 1");
            _activeUsers.Add("Test 2");
            _activeUsers.Add("Test 3");
            _activeUsers.Add("Test 4");
        }

        //public void GetAllUsers()
        //{
        //    //_activeUsers = CommunicationCoordinator.GetAllUsers();
        //    //NotifyPropertyChanged("ActiveUsers");
        //}

        public List<string> ActiveUsers
        {
            get { return _activeUsers; }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
