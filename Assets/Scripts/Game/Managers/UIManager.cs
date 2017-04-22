using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager> {
	const string CanvasName = "Canvas";
	public static Transform Canvas;

	void Awake() {
		if (GetComponent<Canvas>() != null) Canvas = transform;
		else Canvas = transform.Find(CanvasName);
	}
}
