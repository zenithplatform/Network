using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Zenith.Network.ServerManager.ViewModels;

namespace Zenith.Network.ServerManager.UI
{
    public class ConnectionIndicatorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            ConnectionItem connItem = item as ConnectionItem;

            if (connItem.IsConnected)
                return ConnectedTemplate;
            else
                return DisconnectedTemplate;
        }

        public DataTemplate ConnectedTemplate { get; set; }
        public DataTemplate DisconnectedTemplate { get; set; }
    }
}
