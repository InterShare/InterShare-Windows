// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using System.Collections;
using System.Runtime.InteropServices;

namespace InterShareWindows.Helper;

public enum NET_FW_ACTION_
{
    NET_FW_ACTION_BLOCK = 0,
    NET_FW_ACTION_ALLOW = 1
}

public enum NET_FW_RULE_DIRECTION_
{
    NET_FW_RULE_DIR_IN = 1,
    NET_FW_RULE_DIR_OUT = 2
}

public enum NET_FW_PROFILE_TYPE2_
{
    NET_FW_PROFILE2_DOMAIN = 1,
    NET_FW_PROFILE2_PRIVATE = 2,
    NET_FW_PROFILE2_PUBLIC = 4
}

[ComImport]
[Guid("AF230D27-BABA-4E42-ACED-F524F22CFCE2")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface INetFwRule
{
    string Name { get; set; }
    string Description { get; set; }
    string ApplicationName { get; set; }
    string ServiceName { get; set; }
    int Protocol { get; set; }
    string LocalPorts { get; set; }
    string RemotePorts { get; set; }
    string LocalAddresses { get; set; }
    string RemoteAddresses { get; set; }
    string IcmpTypesAndCodes { get; set; }
    NET_FW_RULE_DIRECTION_ Direction { get; set; }
    object Interfaces { get; set; }
    string InterfaceTypes { get; set; }
    bool Enabled { get; set; }
    int Grouping { get; set; }
    int Profiles { get; set; }
    bool EdgeTraversal { get; set; }
    NET_FW_ACTION_ Action { get; set; }
}

[ComImport]
[Guid("9C4C6277-5027-441E-AFAE-CA1F542DA009")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface INetFwRules : IEnumerable
{
    int Count { get; }
    void Add([In] INetFwRule rule);
    void Remove([In] string name);
    INetFwRule Item([In] string name);
    new IEnumerator GetEnumerator();
}

[ComImport]
[Guid("98325047-C671-4174-8D81-DEFCD3F03186")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface INetFwPolicy2
{
    int CurrentProfileTypes { get; }
    bool get_FirewallEnabled(int profileType);
    void put_FirewallEnabled(int profileType, bool enabled);

    bool get_ExcludedInterfaces(int profileType);
    void put_ExcludedInterfaces(int profileType, object interfaces);

    bool get_BlockAllInboundTraffic(int profileType);
    void put_BlockAllInboundTraffic(int profileType, bool block);

    bool get_NotificationsDisabled(int profileType);
    void put_NotificationsDisabled(int profileType, bool disabled);

    bool get_UnicastResponsesToMulticastBroadcastDisabled(int profileType);
    void put_UnicastResponsesToMulticastBroadcastDisabled(int profileType, bool disabled);

    INetFwRules Rules { get; }
}

[ComImport]
[Guid("E2B3C97F-6AE1-41AC-817A-F6F92166D7DD")]
public class NetFwPolicy2
{
}