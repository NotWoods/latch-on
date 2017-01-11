using UnityEngine;

/// Alias for the position in the world. More specific than Transform.
[DisallowMultipleComponent]
public class WorldPosition : MonoBehaviour {
	public Vector2 Value {
		get { return transform.position; }
		// set { transform.position = value; }
	}

	public float x { get { return Value.x; } }
	public float y { get { return Value.y; } }
}
