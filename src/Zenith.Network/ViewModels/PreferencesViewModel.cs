using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zenith.Client.Shared.Commands;
using Zenith.Network.ServerManager.Properties;

namespace Zenith.Network.ServerManager.ViewModels
{
    public class PreferencesViewModel : INotifyPropertyChanged
    {
        private bool _minimizeInTrayOnClose = false;
        private bool _displayDisconnectedNodes = false;
        private string _settingsMessage = "";
        private ICommand _savePreferencesCommand, _settingsCloseCommand = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public PreferencesViewModel()
        {
            if (Settings.Default.Preferences == null)
            {
                Settings.Default.Preferences = new UserPreferences();
            }

            _minimizeInTrayOnClose = Settings.Default.Preferences.MinimizeInTrayOnClose;
            _displayDisconnectedNodes = Settings.Default.Preferences.DisplayDisconnectedNodes;
            _settingsMessage = "";
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_savePreferencesCommand == null)
                    _savePreferencesCommand = new RelayCommand<object>(OnSaveClick);

                return _savePreferencesCommand;
            }
        }

        public void OnSaveClick(object parameter)
        {
            Settings.Default.Save();
            SettingsMessage = "Preferences has been saved.";
        }

        public ICommand SettingsCloseCommand
        {
            get
            {
                if (_settingsCloseCommand == null)
                    _settingsCloseCommand = new RelayCommand<object>(OnSettingsClosed);

                return _settingsCloseCommand;
            }
        }

        public void OnSettingsClosed(object parameter)
        {   
            SettingsMessage = "";
        }

        public bool MinimizeInTrayOnClose
        {
            get { return _minimizeInTrayOnClose; }
            set
            {
                _minimizeInTrayOnClose = value;
                Settings.Default.Preferences.MinimizeInTrayOnClose = _minimizeInTrayOnClose;                
                NotifyPropertyChanged("MinimizeInTrayOnClose");
            }
        }

        public bool DisplayDisconnectedNodes
        {
            get { return _displayDisconnectedNodes; }
            set
            {
                _displayDisconnectedNodes = value;
                Settings.Default.Preferences.DisplayDisconnectedNodes = _displayDisconnectedNodes;
                NotifyPropertyChanged("DisplayDisconnectedNodes");
            }
        }

        public string SettingsMessage
        {
            get { return _settingsMessage; }
            set
            {
                _settingsMessage = value;
                NotifyPropertyChanged("SettingsMessage");
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
