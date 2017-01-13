using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.DataProviders;
using Zenith.Core.DataProviders.Interfaces;
using Zenith.Core.DataProviders.Rethink;
using Zenith.Network.Api;
using Zenith.Network.Api.Interfaces;
using Zenith.Network.Api.Storage;
using Zenith.Network.ServerManager.ViewModels;
using Zenith.Network.Servers;
using Zenith.Network.Servers.Infrastructure;

namespace Zenith.Network.ServerManager
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            // server
            builder.RegisterType<DefaultConnectionFactory>().As<IConnectionFactory>().SingleInstance();
            
     //       builder.RegisterAssemblyTypes(typeof(BaseDataProvider).Assembly)
     //.Where(t => t.IsSubclassOf(typeof(BaseDataProvider)))
     //.As<BaseDataProvider>().WithProperty("ConnectionParams", ConnectionParams.FromConfig());
            builder.RegisterType<RethinkDBDataProvider>().As<IDataProvider>().WithProperty("ConnectionParams", ConnectionParams.FromConfig("NodeStorage"));
            //builder.RegisterType<RethinkDBDataProvider>().WithProperty("ConnectionParams", ConnectionParams.FromConfig());
            builder.RegisterType<NodeStorage>().As<INodeStorage>().SingleInstance();
            builder.RegisterType<HubRequestHandler>().As<IHubRequestHandler>().SingleInstance();
            builder.RegisterType<ServerController>().As<IServerController>().SingleInstance();
            builder.RegisterType<CentralNodeHub>().SingleInstance();

            // UI
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();

            return builder.Build();
        }
    }
}
