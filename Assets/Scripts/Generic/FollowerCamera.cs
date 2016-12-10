using UnityEngine;

public class FollowerCamera : MonoBehaviour {
	public Vector2 FocusZone = new Vector2(3, 6);

	// [HideInInspector]
	public Collider2D Target;
	private Bounds focusBox;
	private Vector3 cameraOffset;
	private Color gizmoColor = new Color(1, 0, 0, 0.1f);

	void Start() {
		focusBox = new Bounds(Vector3.zero, FocusZone);
		cameraOffset = Vector3.forward * transform.position.z;
	}

	void LateUpdate() {
		focusBox.size = FocusZone;

		if (Target == null) return;
		Bounds target = Target.bounds;
		Vector2 shift = Vector2.zero;

		if (target.min.x < focusBox.min.x)
			shift.x = target.min.x - focusBox.min.x;
		else if (target.max.x > focusBox.max.x)
			shift.x = target.max.x - focusBox.max.x;
		if (target.min.y < focusBox.min.y)
			shift.y = target.min.y - focusBox.min.y;
		else if (target.max.y > focusBox.max.y)
			shift.y = target.max.y - focusBox.max.y;
		focusBox.center += (Vector3) shift;

		transform.position = Vector3.MoveTowards(
			transform.position,
			focusBox.center + cameraOffset,
			10
		);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = gizmoColor;
		Gizmos.DrawCube(focusBox.center, focusBox.size);
	}
}