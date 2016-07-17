using UnityEngine;

namespace UI {
	[RequireComponent(typeof(Canvas))]
	public class Crosshair : MonoBehaviour {
		protected Canvas canvas;

		public Transform trackedObject;

		void Start() {
			canvas = GetComponent<Canvas>();
		}

		void OnDisable() {
			canvas.enabled = false;
			Cursor.visible = true;
		}

		void OnEnable() {
			canvas.enabled = true;
			Cursor.visible = false;
		}

		///Draw a cursor indicating where the raycast hits
		public void FromInputPosition(Vector2 towardsPoint) {
			
		}
	}
}