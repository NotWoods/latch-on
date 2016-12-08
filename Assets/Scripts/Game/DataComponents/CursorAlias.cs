using UnityEngine;

[DisallowMultipleComponent]
public class CursorAlias : MonoBehaviour, IDataComponent {
	public Color CursorColor = Color.white;
	public float TransitionMultiplier = 10;

	public float HighlightScale = 0.2f;
	public float DarkScale = 0.1f;

	private Transform cursorObject;
	private SpriteRenderer spriteRenderer;
	void Awake() {
		cursorObject = transform.GetChild(0);
		spriteRenderer = cursorObject.GetComponent<SpriteRenderer>();
	}

	void Update() {
		cursorObject.localScale = Vector3.Lerp(
			cursorObject.localScale,
			Vector3.one * (Highlighted ? HighlightScale : DarkScale),
			Time.deltaTime * TransitionMultiplier
		);
	}

	public Vector2 Position {
		get { return cursorObject.position; }
		set { cursorObject.position = value; }
	}

	public bool Highlighted;
}
