using UnityEngine;
using System.Collections;

public class Edit_Camera : MonoBehaviour {
	public Texture2D cursorTexture;
	RectTransform handleCanvas;

	public float magnitude = 0.05f;
	Vector2 mouseStartPos;
	Vector3 cameraStartPos;
	bool midButtonDown = false;
	
	public Transform activeObject;

	void Start () {
		Cursor.SetCursor(cursorTexture, Vector2.one * 3f, CursorMode.Auto);
		handleCanvas = (RectTransform) GameObject.Find("HandleCanvas").transform;
		activeObject = GameObject.Find("Cube").transform;
	}

	void Update () {
		if ((Input.GetMouseButton(2)) || (Input.touchCount == 2)) {
			if (midButtonDown) {
				Vector2 mousePos = (Vector2) Input.mousePosition;
				Vector2 offset = mouseStartPos - mousePos;
				offset *= magnitude;
				transform.position = new Vector3((offset.x + cameraStartPos.x), (offset.y + cameraStartPos.y), cameraStartPos.z);
			} else {
				mouseStartPos = (Vector2) Input.mousePosition;
				cameraStartPos = transform.position;
				midButtonDown = true;
			}
		} else {
			midButtonDown = false;
		}

		if (Input.GetMouseButtonDown(1)) {
			Vector2 mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.up, 0.1f);
			SetActiveObject(hit.transform);
		}
	}
	
	public void SetActiveObject(Transform obj) {
		activeObject = obj;
		handleCanvas.position = new Vector3(obj.position.x, obj.position.y, -0.75f);
		handleCanvas.sizeDelta = (Vector2) obj.localScale;
	}
}
