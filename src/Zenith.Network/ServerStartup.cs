using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Tracing;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Servers.Infrastructure;

namespace Zenith.Network.ServerManager
{
    public class ServerStartup
    {
        public void Configuration(IAppBuilder app)
        {
            HubConfiguration config = new HubConfiguration();
            config.EnableDetailedErrors = true;
            config.Resolver = new AutofacDependencyResolver(App.Container);

            //AddPipelineModule(new ErrorHandlingPipelineModule());
            app.UseCors(CorsOptions.AllowAll);
            //EnablePolymoprhicMessages();
            //EnableTracing(app);
            //GlobalHost.DependencyResolver.Register(typeof(IParameterResolver), () => new DefaultParameterResolver());
            app.MapSignalR(config);
        }

        private void AddPipelineModule<T>(T instance) where T : HubPipelineModule
        {
            GlobalHost.HubPipeline.AddModule(instance);
            //GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
        }

        private void EnablePolymoprhicMessages()
        {
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer),
                                                    () => JsonSerializer.Create(new JsonSerializerSettings
                                                    {
                                                        TypeNameHandling = TypeNameHandling.All,
                                                        //ContractResolver = new 
                                                    }));
            //var serializer = GlobalHost.DependencyResolver.GetService(typeof(JsonSerializer)) as JsonSerializer;
            //serializer.TypeNameHandling = TypeNameHandling.Auto;
            //serializer.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            //serializer.ObjectCreationHandling = ObjectCreationHandling.Auto;
            //GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            //var service = (JsonSerializer)GlobalHost.DependencyResolver.GetService(typeof(Newtonsoft.Json.JsonSerializer));
            //service.TypeNameHandling = TypeNameHandling.All;
        }

        private void EnableTracing(IAppBuilder app)
        {
            var writer = (TextWriter)app.Properties["host.TraceOutput"];

            var traceManager = new OwinTraceManager(writer);
            GlobalHost.DependencyResolver.Register(typeof(ITraceManager), () => traceManager);
            traceManager.Switch.Level = SourceLevels.Error;
        }
    }
}
