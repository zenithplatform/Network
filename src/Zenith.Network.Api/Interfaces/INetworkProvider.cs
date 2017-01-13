using System;

namespace Zenith.Network.Api.Interfaces
{
    public interface INetworkProvider
    {
        void Initialize(INodeMetadata metadata);
        void StartUp();
        void Shutdown();

        IServiceContainer ServiceContainer { get; }
    }

    public interface IServiceContainer
    {
        object GetService(Type serviceType);
        void Register(Type serviceType, Func<object> activator);
        void Initialize();
    }
}
