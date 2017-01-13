using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zenith.Network.TestClient.Controls
{
    public class DynamicImageButton : Button
    {
        static DynamicImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DynamicImageButton),
            new FrameworkPropertyMetadata(typeof(DynamicImageButton)));
        }

        public string ImageResource
        {
            get { return (string)GetValue(ImageResourceProperty); }
            set { SetValue(ImageResourceProperty, (string)value); }
        }

        public static readonly DependencyProperty ImageResourceProperty =
            DependencyProperty.Register("ImageResource", typeof(string),
              typeof(DynamicImageButton), new PropertyMetadata(string.Empty, null));

        public Brush FillColor
        {
            get { return (Brush)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, (Brush)value); }
        }

        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register("FillColor", typeof(Brush),
              typeof(DynamicImageButton), new PropertyMetadata(Brushes.Black, null));
    }
}
