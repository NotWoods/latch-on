#if UNITY_WP8 && !UNITY_EDITOR
using System.Diagnostics;

public class Wp8Platform : UdpKit.UdpPlatform {
  class PrecisionTimer {
    static readonly long start = Stopwatch.GetTimestamp();
    static readonly double freq = 1.0 / (double)Stopwatch.Frequency;

    internal static uint GetCurrentTime() {
      long diff = Stopwatch.GetTimestamp() - start;
      double seconds = (double)diff * freq;
      return (uint)(seconds * 1000.0);
    }
  }

  public Wp8Platform() {
    GetPrecisionTime();
  }

  public override UdpKit.UdpPlatformSocket CreateSocket() {
    return new Wp8Socket(this);
  }

  public override UdpKit.UdpIPv4Address GetBroadcastAddress() {
    return UdpKit.UdpIPv4Address.Broadcast;
  }

  public override System.Collections.Generic.List<UdpKit.UdpPlatformInterface> GetNetworkInterfaces() {
    var result = new System.Collections.Generic.List<UdpKit.UdpPlatformInterface>();

    foreach (var addr in UdpKit.Wp8Platform.GetIpAddresses()) {
      try {
        var ipv4 = UdpKit.UdpIPv4Address.Parse(addr);

        if (ipv4.IsPrivate) {
          result.Add(new Wp8Interface(ipv4));
        }
      }
      catch { }
    }

    return result; 
  }

  public override uint GetPrecisionTime() {
    return PrecisionTimer.GetCurrentTime();
  }

  public override UdpKit.UdpIPv4Address[] ResolveHostAddresses(string host) {
    return new UdpKit.UdpIPv4Address[0];
  }

  public override bool SupportsBroadcast {
    get { return false; }
  }

  public override bool SupportsMasterServer {
    get { return true; }
  }

  public override bool IsNull {
    get { return false; }
  }
}
#endif