#if UNITY_WP8 && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class Wp8Interface : UdpKit.UdpPlatformInterface {
  readonly UdpKit.UdpIPv4Address[] unicast;
  readonly UdpKit.UdpIPv4Address[] gateway;
  readonly UdpKit.UdpIPv4Address[] multicast;

  public Wp8Interface(UdpKit.UdpIPv4Address address) {
    unicast = new UdpKit.UdpIPv4Address[1] { address };

    address.Byte0 = 1;
    gateway = new UdpKit.UdpIPv4Address[1] { address };

    address.Byte0 = 255;
    multicast = new UdpKit.UdpIPv4Address[2] { UdpKit.UdpIPv4Address.Broadcast, address };
  }

  public override UdpKit.UdpIPv4Address[] GatewayAddresses {
    get { return gateway; }
  }

  public override UdpKit.UdpLinkType LinkType {
    get { return UdpKit.UdpLinkType.Unknown; }
  }

  public override UdpKit.UdpIPv4Address[] MulticastAddresses {
    get { return multicast; }
  }

  public override string Name {
    get { return "UNKNOWN"; }
  }

  public override byte[] PhysicalAddress {
    get { return new byte[0]; }
  }

  public override UdpKit.UdpIPv4Address[] UnicastAddresses {
    get { return unicast; }
  }
}
#endif