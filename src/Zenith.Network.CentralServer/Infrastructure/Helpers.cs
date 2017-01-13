using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Servers.Infrastructure
{
    internal static class Helpers
    {
        internal static string GetIpAddress(HubCallerContext context)
        {
            string ipAddress;
            object tempObject;

            context.Request.Environment.TryGetValue("server.RemoteIpAddress", out tempObject);

            if (tempObject != null)
            {
                ipAddress = (string)tempObject;
            }
            else
            {
                ipAddress = "";
            }

            return ipAddress;
        }

        //internal static string GetIpAddress(HubCallerContext context)
        //{
        //    var env = Get<IDictionary<string, object>>(context.Request., "owin.environment");
        //    if (env == null)
        //    {
        //        return null;
        //    }
        //    var ipAddress = Get<string>(env, "server.RemoteIpAddress");
        //    return ipAddress;
        //}

        //private static T Get<T>(IDictionary<string, object> env, string key)
        //{
        //    object value;
        //    return env.TryGetValue(key, out value) ? (T)value : default(T);
        //}
    }
}
