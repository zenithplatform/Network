using Open.Nat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zenith.Network.Core.Nat
{
    public class NatUtils
    {
        bool _enableLogging = false;

        public NatUtils(bool enableLogging)
        {
            _enableLogging = enableLogging;

            if(_enableLogging)
            {
                NatDiscoverer.TraceSource.Switch.Level = SourceLevels.Verbose;
                NatDiscoverer.TraceSource.Listeners.Add(new System.Diagnostics.TextWriterTraceListener());
            }
        }

        internal async Task<bool> CreateNatTraversalEntry(NatMappingEntry entry)
        {
            try
            {
                var discoverer = new NatDiscoverer();
                var cts = new CancellationTokenSource(10000);
                var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                Mapping mapping = new Mapping((Protocol)((int)entry.Protocol), entry.PrivatePort, entry.PublicPort, entry.Description);
                await device.CreatePortMapAsync(mapping);

                return true;
            }
            catch (NatDeviceNotFoundException NfExc)
            {
                //log exc
                return false;
            }
            catch (MappingException MExc)
            {
                //log exc
                return false;
            }
        }

        internal async Task<bool> RemoveNatTraversalEntry(string identifier)
        {
            try
            {
                var discoverer = new NatDiscoverer();
                var cts = new CancellationTokenSource(10000);
                var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                foreach (var mapping in await device.GetAllMappingsAsync())
                {
                    if (mapping.Description.Contains(identifier))
                    {
                        await device.DeletePortMapAsync(mapping);
                        return true;
                    }
                }
            }
            catch (NatDeviceNotFoundException NfExc)
            {
                //log exc
                return false;
            }
            catch (MappingException MExc)
            {
                //log exc
                return false;
            }

            return false;
        }

        public async Task<IEnumerable<Mapping>> GetAllMappings()
        {
            IEnumerable<Mapping> mappings = null;

            try
            {
                var discoverer = new NatDiscoverer();
                var cts = new CancellationTokenSource(10000);

                var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
                mappings = await device.GetAllMappingsAsync();
            }
            catch (NatDeviceNotFoundException NfExc)
            {
                //log exc
                return null;
            }
            catch (MappingException MExc)
            {
                //log exc
                return null;
            }

            return mappings;
        }

        //public void WaitForConnection(int publicPort)
        //{
        //    // configure a TCP socket listening on port 1602
        //    var endPoint = new IPEndPoint(IPAddress.Any, publicPort);
        //    var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //    socket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
        //    socket.Bind(endPoint);

        //    socket.Listen(4);
        //}

        public async Task<IPAddress> GetExternalIPAddress()
        {
            IPAddress ip = null;

            try
            {
                var nat = new NatDiscoverer();
                var cts = new CancellationTokenSource(5000);
                var device = await nat.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                ip = await device.GetExternalIPAsync();
            }
            catch (AggregateException e)
            {
                if (e.InnerException is NatDeviceNotFoundException)
                {
                    
                }
            }

            return ip;
        }

        public async Task<IPAddress> GetLocalIPAddress()
        {
            var host = await Dns.GetHostEntryAsync(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }

            throw new Exception("Local IP Address Not Found!");
        }
    }
}
