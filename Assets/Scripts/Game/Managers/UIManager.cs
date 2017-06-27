using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : SingletonMonoBehaviour<UIManager> {
	public GameObject CursorPrefab;

	[SerializeField]
	RectTransform _worldSpaceCanvas;
	[SerializeField]
	RectTransform _overlayCanvas;

	[SerializeField]
	TextDisplay textDisplay;

	public HUDWrapper HUD;

	public EgoComponent WorldSpaceCanvas {
		get { return _worldSpaceCanvas.GetComponent<EgoComponent>(); }
	}
	public EgoComponent OverlayCanvas {
		get { return _overlayCanvas.GetComponent<EgoComponent>(); }
	}

	private List<Image> cursors = new List<Image>();
	public static Transform Canvas;

	void Start() {
		cursors.Clear();
	}

	public void Log(string text) {
		textDisplay.Log(text + "\n<size=10>Press R to respawn</size>");
	}
}
