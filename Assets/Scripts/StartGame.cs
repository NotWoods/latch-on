using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	public string defaultPort = "4722";
	
	UnityEngine.UI.InputField hostIp;
	UnityEngine.UI.InputField hostPort;
	UnityEngine.UI.InputField joinIp;
	UnityEngine.UI.InputField joinPort;

	void Start() {
		hostIp = GameObject.Find("Canvas/Host Panel/Host IP").GetComponent<UnityEngine.UI.InputField>();
		hostPort = GameObject.Find("Canvas/Host Panel/Host Port").GetComponent<UnityEngine.UI.InputField>();
		joinIp = GameObject.Find("Canvas/Host Panel/Join IP").GetComponent<UnityEngine.UI.InputField>();
		joinPort = GameObject.Find("Canvas/Host Panel/Join Port").GetComponent<UnityEngine.UI.InputField>();
	}

	void Singleplayer() {
		Application.LoadLevel("Tutorial");
	}
	
	void Host() {
		if (hostIp.text != "") {
			if (hostPort.text == "") {hostPort.text = defaultPort;}
			BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse(hostIp.text + ":" + hostPort.text));
			BoltNetwork.LoadScene("Tutorial");
		}
	}
	
	void Join() {
		if (joinIp.text != "") {
			if (joinPort.text == "") {joinPort.text = defaultPort;}
			BoltLauncher.StartClient();
			BoltNetwork.Connect(UdpKit.UdpEndPoint.Parse(joinIp.text + ":" + joinPort.text));
		}
	}
}
