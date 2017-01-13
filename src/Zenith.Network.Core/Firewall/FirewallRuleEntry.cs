using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Core.Firewall
{
    public struct FirewallRuleEntry
    {
        public string ApplicationPath { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public string RuleName { get; set; }
        public Action RuleAction { get; set; }
        public ConnectionDirection Direction { get; set; }
        public ProtocolType Protocol { get; set; }
        public bool EdgeTraversal { get; set; }
        public bool Enabled { get; set; }
    }

    public enum Action
    {
        Deny = 0,
        Allow = 1
    }

    public enum ConnectionDirection
    {
        In = 1,
        Out = 2
    }
}
