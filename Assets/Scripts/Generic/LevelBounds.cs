using UnityEngine;

public class LevelBounds : MonoBehaviour {
	[SerializeField]
	private Bounds _bounds;

	public Bounds Bounds {
		get {
			return new Bounds((Vector2) _bounds.center, _bounds.size);
		}
	}

	public Color OutlineColor = new Color(0, 0, 1, 0.1f);

	void OnDrawGizmos() {
		Gizmos.color = OutlineColor;
		Gizmos.DrawWireCube(Bounds.center, Bounds.size);
	}
}
