using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFwTypeLib;

// ZEER mooie code op https://github.com/wokhansoft/WFN/blob/master/Common/Helpers/FirewallHelper.cs 


namespace AIT_LABO_006.LIB
{
    public class WinFireWall
    {


        public static bool CreateRule(NET_FW_RULE_DIRECTION_ richting, string name, string description, int profileValue, bool enabled,
            NET_FW_ACTION_ typeOfRule, string application, string localAdresses, string remoteAdresses, string localPorts, string remotePorts,
            int protocolValue)
        {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            INetFwRule firewallRule = firewallPolicy.Rules.OfType<INetFwRule>().Where(x => x.Name == name).FirstOrDefault();
            if (firewallRule != null)
            {
                firewallPolicy.Rules.Remove(firewallRule.Name);
            }
            try
            {
                firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Name = name;
                firewallPolicy.Rules.Add(firewallRule);
                firewallRule.Description = description;
                firewallRule.Grouping = "testomgeving .Net";
                firewallRule.Profiles = profileValue;
                firewallRule.Protocol = protocolValue;  // moet gezet worden vooraleer de poorten gezet worden
                firewallRule.Direction = richting;
                firewallRule.Action = typeOfRule;
                firewallRule.ApplicationName = application;
                firewallRule.LocalAddresses = localAdresses;
                if(localPorts != "")
                    firewallRule.LocalPorts = localPorts;
                firewallRule.RemoteAddresses = remoteAdresses;
                if(remotePorts != "")
                    firewallRule.RemotePorts = remotePorts;
                firewallRule.Enabled = enabled;
            }
            catch (Exception fout)
            {
                return false;
            }



            return true;
        }
        public static IList<INetFwRule> GetAllRules(NET_FW_RULE_DIRECTION_ richting)
        {
            IList<INetFwRule> rules = new List<INetFwRule>();
            INetFwPolicy2 fwpoliciy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            foreach (INetFwRule fwrule in fwpoliciy.Rules)
            {
                if (fwrule.Direction == richting)
                    rules.Add(fwrule);
            }
            return rules;

        }
        public static string getProtocolAsString(int protocol)
        {
            //These are the IANA protocol numbers.
            switch (protocol)
            {
                case 1:
                    return "ICMP";

                case 2:
                    return "IGMP"; //Used by OpenVPN, for example.

                case (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP: //6
                    return "TCP";

                case (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP: //17
                    return "UDP";

                case 40:
                    return "IL"; // IL Transport protocol
                case 42:
                    return "SDRP"; // Source Demand Routing Protocol

                case 47:
                    return "GRE"; //Used by PPTP, for example.

                case 58:
                    return "ICMPv6";

                case 136:
                    return "UDPLite";

                default:
                    return protocol >= 0 ? protocol.ToString() : "Unknown";
            }
        }
    }
    public class WinFireWallProtocols
    {
        public string ProtocolName { get; private set; }
        public int ProtocolValue { get; private set; }
        private WinFireWallProtocols(string protocolName, int protocolValue)
        {
            ProtocolName = protocolName;
            ProtocolValue = protocolValue;
        }

        public static List<WinFireWallProtocols> GetProtocols()
        {
            List<WinFireWallProtocols> wfwps = new List<WinFireWallProtocols>();
            wfwps.Add(new WinFireWallProtocols("ICMP", 1));
            wfwps.Add(new WinFireWallProtocols("IGMP", 2));
            wfwps.Add(new WinFireWallProtocols("TCP", 6));
            wfwps.Add(new WinFireWallProtocols("UDP", 17));
            wfwps.Add(new WinFireWallProtocols("IL", 40));
            wfwps.Add(new WinFireWallProtocols("SDRP", 42));
            wfwps.Add(new WinFireWallProtocols("GRE", 47));
            wfwps.Add(new WinFireWallProtocols("ICMPv6", 58));
            wfwps.Add(new WinFireWallProtocols("UDPLite", 136));
            return wfwps;

        }
    }
}
