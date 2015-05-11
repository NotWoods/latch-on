﻿#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_PS4 || UNITY_PSM
using System.Collections;
using UdpKit;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

#if !UNITY_WEBPLAYER
using System.Net.NetworkInformation;
#endif

using System.Diagnostics;

public class DotNetPlatform : UdpPlatform {
  class PrecisionTimer {
    static readonly long start = Stopwatch.GetTimestamp();
    static readonly double freq = 1.0 / (double)Stopwatch.Frequency;

    internal static uint GetCurrentTime() {
      long diff = Stopwatch.GetTimestamp() - start;
      double seconds = (double)diff * freq;
      return (uint)(seconds * 1000.0);
    }
  }

  public DotNetPlatform() {
    PrecisionTimer.GetCurrentTime();
  }

  public override bool SupportsBroadcast {
    get { return true; }
  }

  public override bool SupportsMasterServer {
    get { return true; }
  }

  public override uint GetPrecisionTime() {
    return PrecisionTimer.GetCurrentTime();
  }

#pragma warning disable 618
  public override UdpIPv4Address GetBroadcastAddress() {
#if UNITY_WEBPLAYER
    return new UdpIPv4Address(255, 255, 255, 255);
#else
    return new UdpIPv4Address(FindBroadcastAddress(true).Address);
#endif
  }
#pragma warning restore 618

  public override UdpPlatformSocket CreateSocket() {
    return new DotNetSocket(this);
  }

  public override List<UdpPlatformInterface> GetNetworkInterfaces() {
    return FindInterfaces();
  }

#if UNITY_WEBPLAYER
  public override UdpIPv4Address[] ResolveHostAddresses(string host) {
    throw new System.NotSupportedException("ResolveHostAddress is not supported in WebPlayer");
  }
#else
  public override UdpIPv4Address[] ResolveHostAddresses(string host) {
    if (host == null) {
      throw new System.ArgumentNullException("host", "argument was null");
    }

    if (host.Length == 0) {
      throw new System.ArgumentException("host name was empty", "host");
    }

    return Dns.GetHostAddresses(host).Select(x => ConvertAddress(x)).ToArray();
  }
#endif

  List<UdpPlatformInterface> FindInterfaces() {
#if UNITY_WEBPLAYER
    return new List<UdpPlatformInterface>();
#else
    List<UdpPlatformInterface> result = new List<UdpPlatformInterface>();

    try {
      if (NetworkInterface.GetIsNetworkAvailable()) {
        foreach (var n in NetworkInterface.GetAllNetworkInterfaces()) {
          try {
            if (n.OperationalStatus != OperationalStatus.Up && n.OperationalStatus != OperationalStatus.Unknown) {
              continue;
            }

            if (n.NetworkInterfaceType == NetworkInterfaceType.Loopback) {
              continue;
            }

            var iface = ParseInterface(n);
            if (iface != null) {
              result.Add(iface);
            }
          }
          catch (System.Exception exn) {
            UdpLog.Error(exn.Message);
          }
        }
      }
    }
    catch (System.Exception exn) {
      UdpLog.Error(exn.Message);
    }

    return result;
#endif
  }


#if !UNITY_WEBPLAYER
  DotNetInterface ParseInterface(NetworkInterface n) {
    HashSet<UdpIPv4Address> gateway = new HashSet<UdpIPv4Address>(UdpIPv4Address.Comparer.Instance);
    HashSet<UdpIPv4Address> unicast = new HashSet<UdpIPv4Address>(UdpIPv4Address.Comparer.Instance);
    HashSet<UdpIPv4Address> multicast = new HashSet<UdpIPv4Address>(UdpIPv4Address.Comparer.Instance);

    IPInterfaceProperties p = null;

    try {
      p = n.GetIPProperties();
    }
    catch { return null; }

    if (p != null) {

      try {
        foreach (var gw in p.GatewayAddresses) {
          try {
            if (gw.Address.AddressFamily == AddressFamily.InterNetwork) {
              gateway.Add(ConvertAddress(gw.Address));
            }
          }
          catch { }
        }
      }
      catch { }

      try {
        foreach (var addr in p.DnsAddresses) {
          try {
            if (addr.AddressFamily == AddressFamily.InterNetwork) {
              gateway.Add(ConvertAddress(addr));
            }
          }
          catch { }
        }
      }
      catch { }

      try {
        foreach (var uni in p.UnicastAddresses) {
          try {
            if (uni.Address.AddressFamily == AddressFamily.InterNetwork) {
              UdpIPv4Address ipv4 = ConvertAddress(uni.Address);

              unicast.Add(ipv4);
              gateway.Add(new UdpIPv4Address(ipv4.Byte3, ipv4.Byte2, ipv4.Byte1, 1));
            }
          }
          catch { }
        }
      }
      catch { }

      try {
        foreach (var multi in p.MulticastAddresses) {
          try {
            if (multi.Address.AddressFamily == AddressFamily.InterNetwork) {
              multicast.Add(ConvertAddress(multi.Address));
            }
          }
          catch { }
        }
      }
      catch { }

      if (unicast.Count == 0 || gateway.Count == 0) {
        return null;
      }
    }

    return new DotNetInterface(n, gateway.ToArray(), unicast.ToArray(), multicast.ToArray());
  }
#endif

#pragma warning disable 618
  public static UdpEndPoint ConvertEndPoint(EndPoint endpoint) {
    return ConvertEndPoint((IPEndPoint)endpoint);
  }

  public static UdpEndPoint ConvertEndPoint(IPEndPoint endpoint) {
    return new UdpEndPoint(new UdpIPv4Address(endpoint.Address.Address), (ushort)endpoint.Port);
  }

  public static UdpIPv4Address ConvertAddress(IPAddress address) {
    return new UdpIPv4Address(address.Address);
  }

  public static IPEndPoint ConvertEndPoint(UdpEndPoint endpoint) {
    return new IPEndPoint(new IPAddress(new byte[] { endpoint.Address.Byte3, endpoint.Address.Byte2, endpoint.Address.Byte1, endpoint.Address.Byte0 }), endpoint.Port);
  }
#pragma warning restore 618

#if !UNITY_WEBPLAYER
  static bool IsValidInterface(NetworkInterface nic, IPInterfaceProperties p) {
    foreach (var addr in p.GatewayAddresses) {
      byte[] bytes = addr.Address.GetAddressBytes();

      if (bytes.Length == 4 && bytes[0] != 0 && bytes[1] != 0 && bytes[2] != 0 && bytes[3] != 0) {
        return true;
      }
    }

    return false;
  }

  static IPAddress FindBroadcastAddress(bool strict) {
    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

    foreach (NetworkInterface nic in nics) {
      switch (nic.NetworkInterfaceType) {
        case NetworkInterfaceType.Ethernet:
        case NetworkInterfaceType.Ethernet3Megabit:
        case NetworkInterfaceType.FastEthernetFx:
        case NetworkInterfaceType.FastEthernetT:
        case NetworkInterfaceType.Wireless80211:
        case NetworkInterfaceType.GigabitEthernet:

          if (nic.OperationalStatus == OperationalStatus.Up || nic.OperationalStatus == OperationalStatus.Unknown) {
            IPInterfaceProperties p = nic.GetIPProperties();

            if ((strict == false) || IsValidInterface(nic, p)) {
              foreach (UnicastIPAddressInformation address in p.UnicastAddresses) {
                if (address.Address.AddressFamily == AddressFamily.InterNetwork) {
                  if ((p.DhcpServerAddresses.Count == 0) && (strict == false)) {
                    byte[] bytes = address.Address.GetAddressBytes();
                    bytes[3] = 255;
                    return new IPAddress(bytes);

                  }
                  else {
                    byte[] dhcp = p.DhcpServerAddresses[0].GetAddressBytes();
                    byte[] mask = address.IPv4Mask.GetAddressBytes();
                    byte[] addr = new byte[4];

                    for (int i = 0; i < dhcp.Length; ++i) {
                      addr[i] = (byte)((dhcp[i] & mask[i]) | ~mask[i]);
                    }
                    return new IPAddress(addr);
                  }

                }
              }
            }
          }
          break;
      }
    }

    if (strict) {
      return FindBroadcastAddress(false);
    }

    return IPAddress.Any;
  }
#endif
}
#endif