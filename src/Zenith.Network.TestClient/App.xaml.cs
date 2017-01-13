using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Zenith.Network.TestClient.ViewModels;

namespace Zenith.Network.TestClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
            NetworkJoinModel joinModel = container.Resolve<NetworkJoinModel>();
            StartPage startPage = container.Resolve<StartPage>();
            
            //mainWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Zenith.Assets;component/Resources/chart-bubble.png"));
            

            
            //joinModel.Username = Environment.MachineName;
            //startPage.DataContext = joinModel;
            //mainModel.CurrentView = startPage;
            //mainWindow.DataContext = mainModel;

            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            //Settings.Default.Save();
        }
    }
}
