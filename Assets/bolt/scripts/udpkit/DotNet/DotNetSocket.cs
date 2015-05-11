﻿#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_PS4 || UNITY_PSM
using System;
using System.Net;
using System.Net.Sockets;
using UdpKit;

class DotNetSocket : UdpPlatformSocket {
  string error;
  Socket socket;
  DotNetPlatform platform;
  EndPoint recvEndPoint;
  UdpEndPoint endpoint;

  public override UdpPlatform Platform {
    get { return platform; }
  }

  public DotNetSocket(DotNetPlatform platform) {
    this.platform = platform;

    try {
      socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      socket.Blocking = false;

      SetConnReset(socket);
    }
    catch (SocketException exn) {
      HandleSocketException(exn);
    }

    recvEndPoint = new IPEndPoint(IPAddress.Any, 0);
  }

  public override string Error {
    get { return error; }
  }

  public override bool IsBound {
    get { return socket != null && socket.IsBound; }
  }

  public override UdpEndPoint EndPoint {
    get {
      VerifyIsBound();
      return endpoint;
    }
  }

  public override bool Broadcast {
    get {
      VerifyIsBound();

      try {
        error = null;
        return socket.EnableBroadcast;
      }
      catch (SocketException exn) {
        HandleSocketException(exn);
        return false;
      }
    }
    set {
      VerifyIsBound();

      try {
        socket.EnableBroadcast = value;
      }
      catch (SocketException exn) {
        error = null;
        HandleSocketException(exn);
      }
    }
  }

  public override void Close() {
    VerifyIsBound();

    try {
      error = null;
      socket.Close();
    }
    catch (SocketException exn) {
      HandleSocketException(exn);
    }
  }

  public override void Bind(UdpEndPoint ep) {
    try {
      error = null;
      socket.Bind(DotNetPlatform.ConvertEndPoint(ep));

      endpoint = DotNetPlatform.ConvertEndPoint(socket.LocalEndPoint);

      UdpLog.Info("Socket bound to {0}", endpoint);
    }
    catch (SocketException exn) {
      HandleSocketException(exn);
    }
  }

  public override bool RecvPoll() {
    return RecvPoll(0);
  }

  public override bool RecvPoll(int timeoutInMs) {
    try {
      return socket.Poll(timeoutInMs * 1000, SelectMode.SelectRead);
    }
    catch (SocketException exn) {
      HandleSocketException(exn);
      return false;
    }
  }

  public override int RecvFrom(byte[] buffer, int bufferSize, ref UdpEndPoint endpoint) {
    try {
      int bytesReceived = socket.ReceiveFrom(buffer, 0, bufferSize, SocketFlags.None, ref recvEndPoint);

      if (bytesReceived > 0) {
        endpoint = DotNetPlatform.ConvertEndPoint(recvEndPoint);
        return bytesReceived;
      }
      else {
        return -1;
      }
    }
    catch (SocketException exn) {
      HandleSocketException(exn);
      return -1;
    }
  }


  public override int SendTo(byte[] buffer, int bytesToSend, UdpEndPoint endpoint) {
    try {
      return socket.SendTo(buffer, 0, bytesToSend, SocketFlags.None, DotNetPlatform.ConvertEndPoint(endpoint));
    }
    catch (SocketException exn) {
      HandleSocketException(exn);
      return -1;
    }
  }

  void HandleSocketException(SocketException exn) {
    error = exn.ErrorCode + ": " + exn.SocketErrorCode.ToString();
  }

  void VerifyIsBound() {
    if (IsBound == false) {
      throw new InvalidOperationException();
    }
  }

  static void SetConnReset(Socket s) {
    try {
      const uint IOC_IN = 0x80000000;
      const uint IOC_VENDOR = 0x18000000;
      uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
      s.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
    }
    catch { }
  }

}
#endif