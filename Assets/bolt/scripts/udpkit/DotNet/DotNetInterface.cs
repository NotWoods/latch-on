﻿#if !UNITY_WEBPLAYER && (UNITY_EDITOR || UNITY_STANDALONE || UNITY_PS4 || UNITY_PSM)
using System.Collections;
using System.Net.NetworkInformation;
using UdpKit;

public class DotNetInterface : UdpPlatformInterface {
  string name;
  UdpLinkType linkType;
  byte[] physicalAddress;

  UdpIPv4Address[] gatewayAddresses;
  UdpIPv4Address[] unicastAddresses;
  UdpIPv4Address[] multicastAddresses;

  public DotNetInterface(NetworkInterface n, UdpIPv4Address[] gw, UdpIPv4Address[] uni, UdpIPv4Address[] multi) {
    name = ParseName(n);
    linkType = ParseLinkType(n);
    physicalAddress = ParsePhysicalAddress(n);

    gatewayAddresses = gw;
    unicastAddresses = uni;
    multicastAddresses = multi;
  }

  public override string Name {
    get { return name; }
  }

  public override UdpLinkType LinkType {
    get { return linkType; }
  }

  public override byte[] PhysicalAddress {
    get { return physicalAddress; }
  }

  public override UdpIPv4Address[] GatewayAddresses {
    get { return gatewayAddresses; }
  }

  public override UdpIPv4Address[] UnicastAddresses {
    get { return unicastAddresses; }
  }

  public override UdpIPv4Address[] MulticastAddresses {
    get { return multicastAddresses; }
  }

  static string ParseName(NetworkInterface n) {
    try {
      return n.Description;
    }
    catch {
      return "UNKNOWN";
    }
  }

  static byte[] ParsePhysicalAddress(NetworkInterface n) {
    try {
      return n.GetPhysicalAddress().GetAddressBytes();
    }
    catch {
      return new byte[0];
    }
  }

  static UdpLinkType ParseLinkType(NetworkInterface n) {
    switch (n.NetworkInterfaceType) {
      case NetworkInterfaceType.Ethernet:
      case NetworkInterfaceType.Ethernet3Megabit:
      case NetworkInterfaceType.FastEthernetFx:
      case NetworkInterfaceType.FastEthernetT:
      case NetworkInterfaceType.GigabitEthernet:
        return UdpLinkType.Ethernet;

      case NetworkInterfaceType.Wireless80211:
        return UdpLinkType.Wifi;

      default:
        return UdpLinkType.Unknown;
    }
  }
}
#endif