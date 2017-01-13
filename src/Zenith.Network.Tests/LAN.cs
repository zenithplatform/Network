using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Core.LAN;

namespace Zenith.Network.Tests
{
    [TestClass]
    public class LAN
    {
        [TestMethod]
        public void Run()
        {
            LocalNetworkBrowser browser = new LocalNetworkBrowser();
            List<LocalNetworkMachine> machines = browser.GetLocalNetworkComputers();
        }
    }
}
