using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Edit_MoveHandle : MonoBehaviour, IDragHandler, IBeginDragHandler {
	Edit_Camera camScript;
	RectTransform handleCanvas;
	Vector2 handleStartPos;
	Vector2 mouseOffset;

	public void Awake() {
		camScript = Camera.main.GetComponent<Edit_Camera>();
		handleCanvas = (RectTransform) GameObject.Find("HandleCanvas").transform;
	}

	public void OnBeginDrag(PointerEventData data) {
		handleStartPos = (Vector2) transform.position;
	}

	public void OnDrag(PointerEventData data) {
		Vector2 mousePos = (Vector2) Camera.main.ScreenToWorldPoint(data.position);
		Vector3 newPos = new Vector3(Mathf.Round(mousePos.x*2)/2, Mathf.Round(mousePos.y*2)/2, 0);
		camScript.activeObject.position = newPos;
		handleCanvas.position = newPos + (Vector3.back*0.75f);
	}
}
