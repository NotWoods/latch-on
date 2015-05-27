using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Edit_Handles : MonoBehaviour, IDragHandler, IBeginDragHandler {
	Edit_Camera camScript;
	RectTransform handleCanvas;
	Vector2 handleStartPos;
	Vector2 startScale;
	Vector2 startPos;
	float rightMultiplier;
	float upMultiplier;
	
	public void Awake() {
		camScript = Camera.main.GetComponent<Edit_Camera>();
		handleCanvas = (RectTransform) GameObject.Find("HandleCanvas").transform;

		if (transform.name == "Handle TopLeft") {
			upMultiplier = 1;
			rightMultiplier = -1;
		} else if (transform.name == "Handle TopRight") {
			upMultiplier = 1;
			rightMultiplier = 1;
		} else if (transform.name == "Handle BottomLeft") {
			upMultiplier = -1;
			rightMultiplier = -1;
		} else if (transform.name == "Handle BottomRight") {
			upMultiplier = -1;
			rightMultiplier = 1;
		}
	}

	public void OnBeginDrag(PointerEventData data) {
		startScale = camScript.activeObject.localScale;
		startPos = camScript.activeObject.position;
		handleStartPos = (Vector2) transform.position;
	}

	public void OnDrag(PointerEventData data) {
		Vector2 mousePos = (Vector2) Camera.main.ScreenToWorldPoint(data.position);
		Vector2 newPos = new Vector2(Mathf.Round(mousePos.x*2)/2, Mathf.Round(mousePos.y*2)/2);
		Vector2 newHandlePos = (newPos - handleStartPos);
		Vector2 newHandleMag = new Vector2(newHandlePos.x*rightMultiplier, newHandlePos.y*upMultiplier);

		camScript.activeObject.localScale = (Vector3) newHandleMag + (Vector3) startScale + Vector3.forward;
		camScript.activeObject.position = (Vector3) (newHandlePos/2) + (Vector3) startPos;
		handleCanvas.sizeDelta = (Vector2) camScript.activeObject.localScale;
		handleCanvas.position = camScript.activeObject.position + (Vector3.forward*-0.75f);
	}
}
