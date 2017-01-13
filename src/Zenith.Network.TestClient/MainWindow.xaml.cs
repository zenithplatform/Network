using MahApps.Metro.Controls;
using Autofac;
using System;
using System.Collections.Generic;
using Zenith.Network.TestClient.ViewModels;

namespace Zenith.Network.TestClient
{
    public partial class MainWindow : MetroWindow
    {
        MainViewModel _mainModel = null;
        StartPage _startPageView = null;
        ActiveUsersControl _activeUsersView = null;

        public MainWindow()
        {
            InitializeComponent();

            //_mainModel = new MainViewModel();

            //_startPageView = new StartPage();
            //_activeUsersView = new ActiveUsersControl();

            ShowStartPage();

            DataContext = this;
        }

        private void ShowStartPage()
        {
            StartPage startPage = App.Container.Resolve<StartPage>();
            NetworkJoinModel networkJoin = App.Container.Resolve<NetworkJoinModel>();
            MainViewModel mainViewModel = App.Container.Resolve<MainViewModel>();

            networkJoin.Username = Environment.MachineName;
            startPage.DataContext = networkJoin;
            mainViewModel.CurrentView = startPage;

            _mainModel = mainViewModel;
            //startPageModel.OnConnected += StartPageModel_OnConnected;
            //NetworkJoinModel model = new NetworkJoinModel();
            //model.Username = Environment.MachineName;
            //_startPageView.DataContext = model;

            //_mainModel.CurrentView = _startPageView;
        }

        //private void ShowActiveUsers()
        //{
        //    ActiveUsersModel activeUsersModel = new ActiveUsersModel();
        //    activeUsersModel.GetAllUsers();
        //    _activeUsersView.DataContext = activeUsersModel;

        //    _mainModel.CurrentView = _activeUsersView;
        //}

        private void StartPageModel_OnConnected(object sender, System.EventArgs e)
        {
            //StartPageModel model = (StartPageModel)sender;
            ////model.OnConnected -= StartPageModel_OnConnected;

            //ShowActiveUsers();
        }

        public MainViewModel MainViewModel
        {
            get { return _mainModel; }
        }
    }
}
