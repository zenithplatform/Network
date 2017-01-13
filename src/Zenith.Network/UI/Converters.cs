using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Client.Shared.Converters;
using Zenith.Network.Api;
using Zenith.Network.ServerManager.ViewModels;

namespace Zenith.Network.ServerManager.UI
{
    [System.Windows.Markup.MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }

        #endregion
    }

    [System.Windows.Markup.MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BooleanYesNoConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString() == "Yes";
        }

        #endregion
    }

    public class StatusToImageConverter : EnumToImageConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.OnConvert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected override string ResourceFromEnum(object enumValue)
        {
            NodeStatus nodeStatus = (NodeStatus)enumValue;
            string result = "";

            switch (nodeStatus)
            {
                case NodeStatus.Connected:
                    result = "node_connected";
                    break;
                case NodeStatus.Disconnected:
                    result = "node_disconnected";
                    break;
                case NodeStatus.Unknown:
                    result = "appbar_question";
                    break;
            }

            return result;
        }
    }

    public class ActivityToImageConverter : EnumToImageConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.OnConvert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected override string ResourceFromEnum(object enumValue)
        {
            NodeActivity activity = (NodeActivity)enumValue;
            string result = "";

            switch (activity)
            {
                case NodeActivity.Sending:
                case NodeActivity.Receiving:
                    result = "appbar_camera_flash";
                    break;
                case NodeActivity.Idle:
                    result = "node_idle";
                    break;
                case NodeActivity.Closing:
                    result = "connection_closing";
                    break;
            }

            return result;
        }
    }
}
