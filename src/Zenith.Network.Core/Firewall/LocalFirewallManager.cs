using NetFwTypeLib;
using System;

namespace Zenith.Network.Core.Firewall
{
    public class LocalFirewallManager
    {
        INetFwRule _rule = null;
        INetFwPolicy2 _policy = null;
        const string INetFwRule_GUID = "{2C5BC43E-3369-4C33-AB0C-BE9469677AF4}";
        const string INetFwPolicy2_GUID = "{E2B3C97F-6AE1-41AC-817A-F6F92166D7DD}";
        Guid netFwRuleUuid, netFwPolicy2Uuid = Guid.Empty;

        public LocalFirewallManager()
        {
            netFwRuleUuid = new Guid(INetFwRule_GUID);
            netFwPolicy2Uuid = new Guid(INetFwPolicy2_GUID);
        }

        public void CreateNewRule(FirewallRuleEntry entry)
        {
            _rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromCLSID(netFwRuleUuid));
            _rule.Action = (NET_FW_ACTION_)((int)entry.RuleAction);//NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            _rule.ApplicationName = entry.ApplicationPath;
            _rule.Description = entry.Description;
            _rule.Direction = (NET_FW_RULE_DIRECTION_)((int)entry.Direction);//NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            _rule.EdgeTraversal = entry.EdgeTraversal;
            _rule.Enabled = entry.Enabled;
            _rule.Grouping = entry.GroupName;
            _rule.Name = entry.RuleName;
            _rule.Protocol = (int)entry.Protocol;

            _policy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromCLSID(netFwPolicy2Uuid));
            _policy.Rules.Add(_rule);
        }
    }
}
