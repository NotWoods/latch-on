using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {
	public Vector3 cameraStart = new Vector3(-500f, -500f, -15f);
	public Vector2 cameraMax = new Vector2(500f, 500f);

	public GameObject sun;
	public GameObject robot;
	public GameObject robotHand;
	public GameObject cam;
	public GameObject crosshair;

	void LoadObjects() {
		Instantiate(sun, Vector3.zero, Quaternion.Euler(30, 30, 0));
		Instantiate(crosshair, Vector3.right*2, Quaternion.identity);
	}
	
	void LoadCamera(Vector3 startPos, Transform followPlayer) {
		GameObject thisCam = (GameObject) Instantiate(cam, startPos, Quaternion.identity);
		thisCam.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().target = followPlayer;
		thisCam.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().minPos = new Vector2(cameraStart.x, cameraStart.y);
		thisCam.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().maxPos = cameraMax;
		followPlayer.GetComponent<GrapplingHook>().cameraScript = thisCam.GetComponent<UnityStandardAssets._2D.Camera2DFollow>();
	}
	
	Transform LoadPlayerOffline() {
		GameObject thisHook = (GameObject) Instantiate(robotHand, new Vector3(0.265f, -0.145f, -0.48f), Quaternion.identity);
		GameObject player = (GameObject) Instantiate(robot, Vector3.zero, Quaternion.identity);
		player.GetComponent<GrapplingHook>().hook = thisHook.transform;
		player.GetComponent<GrapplingHook>().hookBody = thisHook.GetComponent<Rigidbody2D>();
		player.GetComponent<GrapplingHook>().crosshair = (RectTransform) crosshair.transform;
		return player.transform;
	}

	void Start() {
		LoadObjects();
		Transform ply = LoadPlayerOffline();
		LoadCamera(cameraStart, ply);
		ply.GetComponent<GrapplingHook>().StartTwo();
	}
}
