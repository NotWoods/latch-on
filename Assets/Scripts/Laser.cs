using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.gameObject.GetComponent<GrapplingHook>().Respawn();
		}
	}
}
