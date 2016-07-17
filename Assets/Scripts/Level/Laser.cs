using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	protected virtual bool IsValidTrigger(Collider2D player) {
		return player.tag == "Player";
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!IsValidTrigger(other)) return;

		//other.gameObject.GetComponent<GrapplingHook>().Respawn();
	}
}
