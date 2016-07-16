using UnityEngine;

namespace Player {
	public class CameraFollow : MonoBehaviour {
		///Camera offset compared to the focus area
		float zOffset = 30;
		///Area for focus area tracking size
		public Vector2 focusAreaSize = new Vector2(3, 6);
		
		///Bounds representing focus area
		Bounds focusArea;

		///Reference to game object to track
		public GameObject cameraTarget;
		///The collider of the target game object
		Collider2D targetCollider;

		void Start() {
			targetCollider = cameraTarget.GetComponent<Collider2D>();

			focusArea = new Bounds(targetCollider.bounds.center, focusAreaSize);
			float difference = focusArea.min.y - targetCollider.bounds.min.y;
			focusArea.center -= difference * Vector3.up;
		}

		void LateUpdate() {
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

			transform.position = focusArea.center + (zOffset * Vector3.back);
		}

		void OnDrawGizmos() {
			Gizmos.color = new Color(1, 0, 0, 0.1f);
			Gizmos.DrawCube(focusArea.center, focusArea.size);
		}
	}
}