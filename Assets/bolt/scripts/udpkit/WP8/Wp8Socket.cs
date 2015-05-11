#if UNITY_WP8 && !UNITY_EDITOR
using System.Net;

public class Wp8Socket : UdpKit.UdpPlatformSocket {
  Wp8Platform platform;

  UdpKit.Wp8Socket socket;
  UdpKit.UdpEndPoint endpoint;

  public Wp8Socket(Wp8Platform p) {
    socket = new UdpKit.Wp8Socket(BoltLog.Warn, 8192);
    platform = p;
  }

  public override void Bind(UdpKit.UdpEndPoint ep) {
    socket.Bind(ep.Port.ToString());
  }

  public override bool Broadcast {
    get { return false; }
    set { }
  }

  public override void Close() {
    socket.Close();
  }

  public override UdpKit.UdpEndPoint EndPoint {
    get { return UdpKit.UdpEndPoint.Parse("0.0.0.0:" + socket.LocalPort); }
  }

  public override string Error {
    get { return ""; }
  }

  public override bool IsBound {
    get { return socket.IsBound; }
  }

  public override UdpKit.UdpPlatform Platform {
    get { return platform; }
  }

  public override int RecvFrom(byte[] buffer, int bufferSize, ref UdpKit.UdpEndPoint remoteEndpoint) {
    var host = "";
    var port = "";
    var bytes = socket.RecvFrom(buffer, bufferSize, ref host, ref port);

    if (bytes > 0 && host != null && port != null) {
      remoteEndpoint = UdpKit.UdpEndPoint.Parse(host + ":" + port);
      return bytes;
    }

    return 0;
  }

  public override bool RecvPoll(int timeout) {
    return socket.RecvPoll(timeout);
  }

  public override bool RecvPoll() {
    return RecvPoll(0);
  }

  public override int SendTo(byte[] buffer, int bytesToSend, UdpKit.UdpEndPoint endpoint) {
    return socket.SendTo(buffer, bytesToSend, endpoint.Address.ToString(), endpoint.Port.ToString());
  }

#pragma warning disable 618
  static UdpKit.UdpEndPoint ConvertEndPoint(EndPoint endpoint) {
    return ConvertEndPoint((IPEndPoint)endpoint);
  }

  static UdpKit.UdpEndPoint ConvertEndPoint(IPEndPoint endpoint) {
    return new UdpKit.UdpEndPoint(new UdpKit.UdpIPv4Address(endpoint.Address.Address), (ushort)endpoint.Port);
  }

  static UdpKit.UdpIPv4Address ConvertAddress(IPAddress address) {
    return new UdpKit.UdpIPv4Address(address.Address);
  }

  static IPEndPoint ConvertEndPoint(UdpKit.UdpEndPoint endpoint) {
    return new IPEndPoint(new IPAddress(new byte[] { endpoint.Address.Byte3, endpoint.Address.Byte2, endpoint.Address.Byte1, endpoint.Address.Byte0 }), endpoint.Port);
  }
#pragma warning restore 618
}
#endif