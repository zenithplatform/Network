using Autofac;
using Zenith.Core.Shared.Configuration;
using Zenith.Network.Api.Infrastructure;
using Zenith.Network.TestClient.ViewModels;

namespace Zenith.Network.TestClient
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterType<NetworkNodeCoordinator>().As<INetworkNodeCoordinator>().SingleInstance();

            //Network config
            //builder.RegisterModule(new JsonConfigModule("config.json"));
            //builder.RegisterType<NetworkConfig>();
            // UI
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<NetworkJoinModel>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<StartPage>().SingleInstance();

            return builder.Build();
        }
    }
}
