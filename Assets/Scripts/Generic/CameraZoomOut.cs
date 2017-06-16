using UnityEngine;

public class CameraZoomOut : MonoBehaviour {
	public float StandardZoom = -10;
	public float ExtenededZoom = -20;
	public float MaxDelta = 30;

	[SerializeField]
	bool zoomedOut = false;

	void Update() {
		zoomedOut = Input.GetButton("Zoom Out");
	}

	void LateUpdate() {
		float newZoom;
		if (zoomedOut) {
			newZoom = ExtenededZoom;
		} else {
			newZoom = StandardZoom;
		}

		transform.position = new Vector3(
			transform.position.x,
			transform.position.y,
			Mathf.MoveTowards(transform.position.z, newZoom, MaxDelta * Time.deltaTime)
		);
	}
}
