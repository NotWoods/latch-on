using System;
using System.Collections.Generic;
using System.Reflection;
using UdpKit;
using UnityEngine;

public static class BoltLauncher {
  static UdpPlatform UserAssignedPlatform;

  public static void StartSinglePlayer() {
    StartSinglePlayer(BoltRuntimeSettings.instance.GetConfigCopy());
  }

  public static void StartSinglePlayer(BoltConfig config) {
    // set null platform
    SetUdpPlatform(new NullPlatform());

    // init server
    Initialize(BoltNetworkModes.Host, UdpEndPoint.Any, config);
  }

  public static void StartServer() {
    StartServer(UdpEndPoint.Any);
  }

  public static void StartServer(int port) {
    if (port >= 0 && port <= ushort.MaxValue) {
      StartServer(new UdpEndPoint(UdpIPv4Address.Any, (ushort)port));
    }
    else {
      throw new ArgumentOutOfRangeException(string.Format("'port' must be >= 0 and <= {0}", ushort.MaxValue));
    }
  }

  public static void StartServer(BoltConfig config) {
    StartServer(UdpEndPoint.Any, config);
  }

  public static void StartServer(UdpEndPoint endpoint) {
    StartServer(endpoint, BoltRuntimeSettings.instance.GetConfigCopy());
  }

  public static void StartServer(UdpEndPoint endpoint, BoltConfig config) {
    Initialize(BoltNetworkModes.Host, endpoint, config);
  }

  public static void StartClient() {
    StartClient(UdpEndPoint.Any);
  }

  public static void StartClient(BoltConfig config) {
    StartClient(UdpEndPoint.Any, config);
  }

  public static void StartClient(UdpEndPoint endpoint) {
    StartClient(endpoint, BoltRuntimeSettings.instance.GetConfigCopy());
  }

  public static void StartClient(UdpEndPoint endpoint, BoltConfig config) {
    Initialize(BoltNetworkModes.Client, endpoint, config);
  }

  public static void StartClient(int port) {
    if (port >= 0 && port <= ushort.MaxValue) {
      StartClient(new UdpEndPoint(UdpIPv4Address.Any, (ushort)port));
    }
    else {
      throw new ArgumentOutOfRangeException(string.Format("'port' must be >= 0 and <= {0}", ushort.MaxValue));
    }
  }

  public static void Shutdown() {
    BoltNetworkInternal.__Shutdown();
  }

  static void Initialize(BoltNetworkModes modes, UdpEndPoint endpoint, BoltConfig config) {
    BoltNetworkInternal.DebugDrawer = new BoltInternal.UnityDebugDrawer();

#if UNITY_PRO_LICENSE
    BoltNetworkInternal.UsingUnityPro = true;
#else
    BoltNetworkInternal.UsingUnityPro = false;
#endif

#if BOLT_UPNP_SUPPORT
    BoltNetworkInternal.NatCommunicator = new BoltInternal.StandaloneNatCommunicator();
#endif

    BoltNetworkInternal.GetSceneName = GetSceneName;
    BoltNetworkInternal.GetSceneIndex = GetSceneIndex;
    BoltNetworkInternal.GetGlobalBehaviourTypes = GetGlobalBehaviourTypes;
    BoltNetworkInternal.EnvironmentSetup = BoltInternal.BoltNetworkInternal_User.EnvironmentSetup;
    BoltNetworkInternal.EnvironmentReset = BoltInternal.BoltNetworkInternal_User.EnvironmentReset;
    BoltNetworkInternal.__Initialize(modes, endpoint, config, CreateUdpPlatform());
  }

  static int GetSceneIndex(string name) {
    return BoltInternal.BoltScenes_Internal.GetSceneIndex(name);
  }

  static string GetSceneName(int index) {
    return BoltInternal.BoltScenes_Internal.GetSceneName(index);
  }

  static public List<STuple<BoltGlobalBehaviourAttribute, Type>> GetGlobalBehaviourTypes() {
    Assembly asm = Assembly.GetExecutingAssembly();
    List<STuple<BoltGlobalBehaviourAttribute, Type>> result = new List<STuple<BoltGlobalBehaviourAttribute, Type>>();

    try {
      foreach (Type type in asm.GetTypes()) {
        if (typeof(MonoBehaviour).IsAssignableFrom(type)) {
          var attrs = (BoltGlobalBehaviourAttribute[])type.GetCustomAttributes(typeof(BoltGlobalBehaviourAttribute), false);
          if (attrs.Length == 1) {
            result.Add(new STuple<BoltGlobalBehaviourAttribute, Type>(attrs[0], type));
          }
        }
      }
    }
    catch {
      // just eat this, a bit dangerous but meh.
    }

    return result;
  }

  public static void SetUdpPlatform(UdpPlatform platform) {
    UserAssignedPlatform = platform;
  }

  public static UdpPlatform CreateUdpPlatform() {
    if (UserAssignedPlatform != null) {
      return UserAssignedPlatform;
    }

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
    return new NativePlatform();
#elif (UNITY_PS4 || UNITY_PSM) && !UNITY_EDITOR
    return new DotNetPlatform();
#elif (UNITY_WP8) && !UNITY_EDITOR
    return new Wp8Platform();
#else
    return new DotNetPlatform();
#endif
  }
}
