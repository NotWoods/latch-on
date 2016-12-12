using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : SingletonMonoBehaviour<UIManager> {
	const string CanvasName = "Canvas";

	public GameObject CursorPrefab;

	private List<Image> cursors;
	private Transform canvas;

	void Awake() {
		cursors = new List<Image>();

		if (GetComponent<Canvas>() != null) {
			canvas = transform;
		} else {
			canvas = transform.Find(CanvasName);
		}
	}

	public Image GetCursor(int index) {
		if (cursors.Count > index) return cursors[index];

		Image cursor = Instantiate(CursorPrefab).GetComponent<Image>();
		cursor.rectTransform.SetParent(canvas, false);
		cursors.Add(cursor);
		return cursor;
	}
}
