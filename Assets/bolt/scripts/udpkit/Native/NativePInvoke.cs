﻿#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System;
using UdpKit;

public static class NativePInvoke {
#if UNITY_ANDROID
  public const string DLL_NAME = "udpkit_android";
#elif UNITY_IPHONE
  public const string DLL_NAME = "__Internal";
#else
  public const string DLL_NAME = null;
#endif

  public const int UDPKIT_SOCKET_OK = 0;
  public const int UDPKIT_SOCKET_ERROR = -1;
  public const int UDPKIT_SOCKET_NOTVALID = -2;
  public const int UDPKIT_SOCKET_NODATA = -3;

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern IntPtr CreateSocket();

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 Bind(IntPtr socket, UdpEndPoint.Native addr);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 BroadcastEnable(IntPtr socket);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 BroadcastDisable(IntPtr socket);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 SendTo(IntPtr socket, [Out] byte[] buffer, int size, UdpEndPoint.Native addr);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 RecvFrom(IntPtr socket, [Out] byte[] buffer, int size, [Out] out UdpEndPoint.Native addr);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 RecvPoll(IntPtr socket, int timeout);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 GetEndPoint(IntPtr socket, [Out] out UdpEndPoint.Native addr);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern Int32 Close(IntPtr socket);

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  static extern IntPtr PlatformName();
  public static string PlatformName_Wrapper() { return Marshal.PtrToStringAnsi(PlatformName()); }

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  static extern IntPtr Error();
  public static string Error_Wrapper() { return Marshal.PtrToStringAnsi(Error()); }

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern UInt32 GetPrecisionTime();

  [DllImport(DLL_NAME)]
  [SuppressUnmanagedCodeSecurity]
  public static extern UInt32 GetBroadcastAddress();
}
#endif