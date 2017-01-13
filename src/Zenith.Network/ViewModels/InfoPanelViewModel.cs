using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Zenith.Network.Core;
using Zenith.Client.Shared.Util;
using Zenith.Network.Core.Nat;

namespace Zenith.Network.ServerManager.ViewModels
{
    public class InfoPanelViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<InfoItem> _items = null;
        private DateTime _startTime = DateTime.Now;
        private string _uptimeStr = "";
        private readonly DispatcherTimer _timer;
        public event PropertyChangedEventHandler PropertyChanged;
        private StringBuilder _uptimeFormatBuffer = new StringBuilder();
        NatUtils _natUtils = null;

        public InfoPanelViewModel()
        {
            _items = new ObservableCollection<InfoItem>();
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);

            _natUtils = new NatUtils(false);
        }

        public async void Initialize()
        {
            IPAddress external = await _natUtils.GetExternalIPAddress();
            IPAddress local = await _natUtils.GetLocalIPAddress();

            this.Add(new InfoItem() { Key = "Host", Title = "Host name :", Value = Dns.GetHostName() });
            this.Add(new InfoItem() { Key = "IPv4", Title = "IP Address :", Value = (local != null)?(local.ToString()):("Unknown.") });
            this.Add(new InfoItem() { Key = "PublicIPv4", Title = "Public IP Address :", Value = (external != null) ? (external.ToString()) : ("Unknown.") });
            this.Add(new InfoItem() { Key = "ServerPort", Title = "Port :", Value = "9999" });
            this.Add(new InfoItem() { Key = "Uptime", Title = "Uptime :", Value = _uptimeStr });
        }

        public void StopUptimeCounter()
        {
            _timer.Stop();
            _timer.Tick -= _timer_Tick;
            _uptimeStr = "Not running.";
            this.UpdateByKey("Uptime", _uptimeStr);
        }

        public void StartUptimeCounter()
        {
            _startTime = DateTime.Now;
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var delta = DateTime.Now - _startTime;
            FormatUptime(delta);
            //_uptimeStr = string.Format("{0} days {1} hours {2} minutes {3} seconds", delta.Days, delta.Hours, delta.Minutes, delta.Seconds);
            this.UpdateByKey("Uptime", _uptimeStr);
        }

        private void FormatUptime(TimeSpan deltaTime)
        {
            _uptimeFormatBuffer.AppendIf(deltaTime.Days > 0, string.Format("{0} day(s) ", deltaTime.Days));
            _uptimeFormatBuffer.AppendIf(deltaTime.Hours > 0, string.Format("{0} hour(s) ", deltaTime.Hours));
            _uptimeFormatBuffer.AppendIf(deltaTime.Minutes > 0, string.Format("{0} minute(s) ", deltaTime.Minutes));
            _uptimeFormatBuffer.AppendIf(deltaTime.Seconds > 0, string.Format("{0} second(s) ", deltaTime.Seconds));

            _uptimeStr = _uptimeFormatBuffer.ToString();
            _uptimeFormatBuffer.Clear();
        }

        public void Add(InfoItem item)
        {
            _items.Add(item);
            NotifyPropertyChanged("ServerInfo");
        }

        public void UpdateByKey(string key, string newValue)
        {
            IEnumerable<InfoItem> items = _items.Where(it => it.Key.Equals(key));

            if (items != null && items.Count() > 0)
            {
                InfoItem item = items.First();
                item.Value = newValue;
            }
        }

        public void Clear()
        {
            _items.Clear();
            NotifyPropertyChanged("ServerInfo");
        }

        public ObservableCollection<InfoItem> ServerInfo
        {
            get { return _items; }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class InfoItem : INotifyPropertyChanged
    {
        private string _key = "";
        private string _title = "";
        private string _value = "";

        public InfoItem ()
        {

        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
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
