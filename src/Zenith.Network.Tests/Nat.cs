using Microsoft.VisualStudio.TestTools.UnitTesting;
using Open.Nat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Network.Core.Nat;

namespace Zenith.Network.Tests
{
    [TestClass]
    public class Nat
    {
        [TestMethod]
        public void Run()
        {
            NatUtils natUtils = new NatUtils(true);
            Task<IEnumerable<Mapping>> mappings = natUtils.GetAllMappings();

            mappings.Wait();
            IEnumerable<Mapping> mappings1 = mappings.Result;

            foreach (Mapping mapping in mappings1)
            {

            }
        }
    }
}
