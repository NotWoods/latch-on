using UnityEngine;

public class CameraZoomOut : MonoBehaviour {
	public float StandardZoom = -10;
	public float ReducedZoom = -4;
	public float ExtenededZoom = -20;
	public float MaxDelta = 30;

	[Range(-1, 1)]
	[SerializeField]
	int zoom = 0;

	void Update() {
		if (Input.GetButton("Zoom Out")) zoom = 1;
		else if (Input.GetKey(KeyCode.LeftControl)) zoom = -1;
		else zoom = 0;
	}

	void LateUpdate() {
		float newZoom;
		switch (zoom) {
			case -1: newZoom = ReducedZoom; break;
			case 1: newZoom = ExtenededZoom; break;
			case 0: default:
				newZoom = StandardZoom; break;
		}

		transform.position = new Vector3(
			transform.position.x,
			transform.position.y,
			Mathf.MoveTowards(transform.position.z, newZoom, MaxDelta * Time.deltaTime)
		);
	}
}
