using UnityEngine;

[DisallowMultipleComponent]
public class CursorAlias : MonoBehaviour, IDataComponent {
	public float TransitionMultiplier = 10;

	public float HighlightScale = 0.2f;
	public float DarkScale = 0.1f;

	public Vector2 Position {
		get { return cursor.transform.position; }
		set { cursor.transform.position = value; }
	}

	public bool Highlighted;

	private SpriteRenderer cursor;
	void Awake() {
		cursor = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	void Update() {
		cursor.transform.localScale = Vector3.Lerp(
			cursor.transform.localScale,
			Vector3.one * (Highlighted ? HighlightScale : DarkScale),
			Time.deltaTime * TransitionMultiplier
		);
	}
}
