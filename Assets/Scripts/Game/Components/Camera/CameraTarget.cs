using UnityEngine;
using UnityEngine.SceneManagement;

namespace LatchOn.ECS.Components.Camera {
	[DisallowMultipleComponent]
	public class CameraTarget : MonoBehaviour {
		// Transform to target
		public Collider2D TargetedEntity;
		// Area around the target that can be travelled before the camera moves
		public Vector2 FocusZone = new Vector2(3, 6);

		// Bounds representation of focus zone
		public Bounds FocusBox;

		// Color of the debug gizmo
		private Color gizmoColor = new Color(1, 0, 0, 0.1f);
		void OnDrawGizmos() {
			Gizmos.color = gizmoColor;
			Gizmos.DrawCube(FocusBox.center, FocusBox.size);
		}
	}
}
