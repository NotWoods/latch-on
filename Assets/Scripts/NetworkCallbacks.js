#pragma strict

@BoltGlobalBehaviour
public class NetworkCallbacks extends Bolt.GlobalEventListener {
	public override function SceneLoadLocalDone(map : String) {
		//BoltNetwork.Instantiate(BoltPrefabs.Player, Vector3.zero, Quaternion.identity);
	}
}