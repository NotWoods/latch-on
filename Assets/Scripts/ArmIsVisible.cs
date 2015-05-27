using UnityEngine;
using System.Collections;

public class ArmIsVisible : MonoBehaviour {

	GrapplingHook playerScript; 
	void Start() {
		playerScript = transform.parent.parent.GetComponent<GrapplingHook>();
	}
	void OnBecameInvisible () {
		playerScript.Respawn();
	}
}
