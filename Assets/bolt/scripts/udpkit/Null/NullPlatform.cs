﻿#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_PS4 || UNITY_PSM || UNITY_WP8
#define USE_DOTNET
#endif

using UnityEngine;
using System.Collections;
using UdpKit;

﻿#if USE_DOTNET
using System.Diagnostics;
#endif

public class NullPlatform : UdpPlatform {
﻿#if USE_DOTNET
  class PrecisionTimer {
    static readonly long start = Stopwatch.GetTimestamp();
    static readonly double freq = 1.0 / (double)Stopwatch.Frequency;

    internal static uint GetCurrentTime() {
      long diff = Stopwatch.GetTimestamp() - start;
      double seconds = (double)diff * freq;
      return (uint)(seconds * 1000.0);
    }
  }
#elif (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
  readonly object timeLock = new object();
#endif

  public NullPlatform() {
    GetPrecisionTime();
  }

  public override UdpPlatformSocket CreateSocket() {
    return new NullSocket(this);
  }

  public override UdpIPv4Address GetBroadcastAddress() {
    return UdpIPv4Address.Broadcast;
  }

  public override System.Collections.Generic.List<UdpPlatformInterface> GetNetworkInterfaces() {
    return new System.Collections.Generic.List<UdpPlatformInterface>();
  }

  public override uint GetPrecisionTime() {
#if USE_DOTNET
    return PrecisionTimer.GetCurrentTime();
#elif (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
    lock (timeLock) {
      return NativePInvoke.GetPrecisionTime();
    }
#endif
  }

  public override UdpIPv4Address[] ResolveHostAddresses(string host) {
    return new UdpIPv4Address[0];
  }

  public override bool SupportsBroadcast {
    get { return false; }
  }

  public override bool SupportsMasterServer {
    get { return false; }
  }

  public override bool IsNull { 
    get { return true; } 
  }
}
