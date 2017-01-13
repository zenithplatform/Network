using System;

namespace Zenith.Network.Api.Infrastructure
{
    public interface IComponentContainer
    {
        T Get<T>();
        void Register<T>(Func<object> activator);
    }
}
