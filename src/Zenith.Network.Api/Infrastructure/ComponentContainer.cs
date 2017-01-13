using System;
using System.Collections.Generic;

namespace Zenith.Network.Api.Infrastructure
{
    public class ComponentContainer : IComponentContainer
    {
        Dictionary<Type, object> _types = new Dictionary<Type, object>();

        public T Get<T>()
        {
            if (_types.ContainsKey(typeof(T)))
                return (T)_types[typeof(T)];

            return default(T);
        }

        public void Register<T>(Func<object> activator)
        {
            object instance = null;

            if (activator != null)
                instance = activator();

            if(!_types.ContainsKey(typeof(T)))
                _types.Add(typeof(T), instance);
        }
    }
}
