using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : SingletonMonoBehaviour<UIManager> {
	const string CanvasName = "Canvas";

	public GameObject CursorPrefab;

	private List<Image> cursors;
	public static Transform Canvas;

	void Awake() {
		cursors = new List<Image>();

		if (GetComponent<Canvas>() != null) {
			Canvas = transform;
		} else {
			Canvas = transform.Find(CanvasName);
		}
	}

	public Image GetCursor(int index) {
		if (cursors.Count > index) return cursors[index];

		Image cursor = Instantiate(CursorPrefab).GetComponent<Image>();
		cursor.rectTransform.SetParent(Canvas, false);
		cursors.Add(cursor);
		return cursor;
	}
}
