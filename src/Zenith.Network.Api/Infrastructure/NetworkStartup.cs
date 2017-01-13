using Zenith.Core.Shared.Configuration;
using Zenith.Core.Shared.EventAggregation;
using Zenith.Core.Shared.Logging;

namespace Zenith.Network.Api.Infrastructure
{
    public class NetworkStartup : INetworkStartup
    {
        IComponentContainer _container = null;

        public NetworkStartup()
        {
            _container = new ComponentContainer();
        }

        public void Initialize()
        {
            _container.Register<INetworkStack>(() => 
            {
                return new NetworkStack();
            });

            _container.Register<NetworkConfig>(() =>
            {
                JsonConfig config = new JsonConfig("network.json", "config");
                return config.LoadSection<NetworkConfig>();
            });

            _container.Register<IEventAggregator>(() =>
            {
                return new EventAggregator();
            });

            _container.Register<IDefaultLogger>(() =>
            {
                DefaultLogger logger = new DefaultLogger();
                logger.Create("NetworkLogger");

                return logger;
            });
        }

        public IComponentContainer Container
        {
            get { return _container; }
        }
    }
}
