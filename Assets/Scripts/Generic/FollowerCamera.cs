using UnityEngine;

public class FollowerCamera : MonoBehaviour {
	public Color GizmoColor = new Color(1, 0, 0, 0.1f);
	/// Area for focus area tracking size
	public Vector2 FocusAreaSize = new Vector2(3, 6);
	/// Reference to game object to track
	public GameObject CameraTarget;
	/// Camera offset compared to the focus area
	float zOffset = 30;
	/// Bounds representing focus area
	Bounds focusArea;
	/// The collider of the target game object
	BoxCollider2D targetCollider;

	void Start() {
		if (CameraTarget == null) return;

		targetCollider = CameraTarget.GetComponent<BoxCollider2D>();

		focusArea = new Bounds(targetCollider.bounds.center, FocusAreaSize);
		float difference = focusArea.min.y - targetCollider.bounds.min.y;
		focusArea.center -= difference * Vector3.up;
	}

	void LateUpdate() {
		if (CameraTarget == null) return;

		Bounds target = targetCollider.bounds;
		Vector2 shift = Vector2.zero;
		if (target.min.x < focusArea.min.x)
			shift.x = target.min.x - focusArea.min.x;
		else if (target.max.x > focusArea.max.x)
			shift.x = target.max.x - focusArea.max.x;
		if (target.min.y < focusArea.min.y)
			shift.y = target.min.y - focusArea.min.y;
		else if (target.max.y > focusArea.max.y)
			shift.y = target.max.y - focusArea.max.y;
		focusArea.center += (Vector3) shift;

		transform.position = Vector3.MoveTowards(
			transform.position,
			focusArea.center + (zOffset * Vector3.back),
			10
		);
	}

	void OnDrawGizmosSelected() {
		if (CameraTarget == null) return;

		Gizmos.color = GizmoColor;
		Gizmos.DrawCube(focusArea.center, focusArea.size);
	}
}