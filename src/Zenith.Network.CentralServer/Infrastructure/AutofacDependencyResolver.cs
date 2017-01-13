using Autofac;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Servers.Infrastructure
{
    public class AutofacDependencyResolver : DefaultDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            object instance;
            if (_container.TryResolve(serviceType, out instance))
            {
                return instance;
            }
            return base.GetService(serviceType);
        }
    }
}
