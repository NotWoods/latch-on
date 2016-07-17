using UnityEngine;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	public class Crosshair : MonoBehaviour {
		public new RectTransform transform;
		Canvas canvas;

		public float offsetDistance = 2.5f;

		void Start() {
			canvas = GetComponent<Canvas>();
		}

		public void Render(Vector2 origin, Vector2 towardsPoint) {
			Vector2 offset = towardsPoint.normalized * offsetDistance;
			transform.position = origin + offset;
		}

		void OnDisable() {
			canvas.enabled = false;
			Cursor.visible = true;
		}

		void OnEnable() {
			canvas.enabled = true;
			Cursor.visible = false;
		}
	}
}