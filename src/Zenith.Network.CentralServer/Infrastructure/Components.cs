using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.AspNet.SignalR.Tracing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Servers.Infrastructure
{
    //public class DefaultAssemblyLocator : IAssemblyLocator
    //{
    //    private static readonly string AssemblyRoot = typeof(Hub).GetTypeInfo().Assembly.GetName().Name;
    //    private readonly Assembly _entryAssembly;
    //    private readonly DependencyContext _dependencyContext;

    //    public DefaultAssemblyLocator(IHostingEnvironment environment)
    //    {
    //        _entryAssembly = Assembly.Load(new AssemblyName(environment.ApplicationName));
    //        _dependencyContext = DependencyContext.Load(_entryAssembly);
    //    }

    //    public virtual IList<Assembly> GetAssemblies()
    //    {
    //        if (_dependencyContext == null)
    //        {
    //            // Use the entry assembly as the sole candidate.
    //            return new[] { _entryAssembly };
    //        }

    //        return _dependencyContext
    //            .RuntimeLibraries
    //            .Where(IsCandidateLibrary)
    //            .SelectMany(l => l.GetDefaultAssemblyNames(_dependencyContext))
    //            .Select(assembly => Assembly.Load(new AssemblyName(assembly.Name)))
    //            .ToArray();
    //    }

    //    private bool IsCandidateLibrary(RuntimeLibrary library)
    //    {
    //        return library.Dependencies.Any(dependency => string.Equals(AssemblyRoot, dependency.Name, StringComparison.Ordinal));
    //    }
    //}

    public class OwinTraceManager : ITraceManager
    {
        private readonly ConcurrentDictionary<string, TraceSource> _sources = new ConcurrentDictionary<string, TraceSource>(StringComparer.OrdinalIgnoreCase);

        private readonly TextWriter _hostWriter;

        public OwinTraceManager(TextWriter hostWriter)
        {
            Switch = new SourceSwitch("SignalRSwitch");
            _hostWriter = hostWriter;
        }

        public SourceSwitch Switch { get; private set; }

        public TraceSource this[string name]
        {
            get
            {
                return _sources.GetOrAdd(name, key => CreateTraceSource(key));
            }
        }

        private TraceSource CreateTraceSource(string name)
        {
            var traceSource = new TraceSource(name, SourceLevels.Off)
            {
                Switch = Switch
            };

            traceSource.Listeners.Add(new TextWriterTraceListener(_hostWriter));

            return traceSource;
        }
    }

    public class DefaultParameterResolver : IParameterResolver
    {
        /// <summary>
        /// Resolves a parameter value based on the provided object.
        /// </summary>
        /// <param name="descriptor">Parameter descriptor.</param>
        /// <param name="value">Value to resolve the parameter value from.</param>
        /// <returns>The parameter value.</returns>
        public virtual object ResolveParameter(ParameterDescriptor descriptor, IJsonValue value)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            //if(descriptor.ParameterType == typeof(HubRequestMessageBase))
            //{
            //    NodeRegistration reg = new NodeRegistration();
            //    return value.ConvertTo(reg.GetType());
            //}

            //return value;

            if (value.GetType() == descriptor.ParameterType)
            {
                return value;
            }

            return value.ConvertTo(descriptor.ParameterType);
        }

        /// <summary>
        /// Resolves method parameter values based on provided objects.
        /// </summary>
        /// <param name="method">Method descriptor.</param>
        /// <param name="values">List of values to resolve parameter values from.</param>
        /// <returns>Array of parameter values.</returns>
        public virtual IList<object> ResolveMethodParameters(MethodDescriptor method, IList<IJsonValue> values)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            return method.Parameters.Zip(values, ResolveParameter).ToArray();
        }
    }
}
