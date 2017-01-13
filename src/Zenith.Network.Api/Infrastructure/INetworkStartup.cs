namespace Zenith.Network.Api.Infrastructure
{
    public interface INetworkStartup
    {
        void Initialize();
        IComponentContainer Container { get; }
    }
}
