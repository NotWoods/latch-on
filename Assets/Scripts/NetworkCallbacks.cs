using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener {
	public GameObject robot;
	public GameObject robotHand;

	public override void SceneLoadLocalDone(string map) {
		Transform player = LoadPlayerOnline();
		Camera.main.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().target = player;
	}

	public Transform LoadPlayerOnline() {
		BoltEntity thisHook = BoltNetwork.Instantiate(robotHand, new Vector3(0.265f, -0.145f, -0.48f), Quaternion.identity);
		BoltEntity player = BoltNetwork.Instantiate(robot, Vector3.zero, Quaternion.identity);
		player.gameObject.GetComponent<GrapplingHook>().hook = thisHook.transform;
		return player.transform;
	}
}
