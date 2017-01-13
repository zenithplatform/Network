using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Zenith.Network.ServerManager.Properties;
using Zenith.Network.ServerManager.ViewModels;
using Microsoft.Owin;
using Zenith.Network.ServerManager;

[assembly: OwinStartup(typeof(ServerStartup))]
namespace Zenith.Network.ServerManager
{
    public partial class App : Application
    {
        public static IContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Build();

            Container = container;

            MainWindow mainWindow = container.Resolve<MainWindow>();
            MainViewModel mainModel = container.Resolve<MainViewModel>();

            mainWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Zenith.Assets;component/Resources/chart-bubble.png"));
            mainWindow.DataContext = mainModel; 

            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Settings.Default.Save();
        }
    }
}
