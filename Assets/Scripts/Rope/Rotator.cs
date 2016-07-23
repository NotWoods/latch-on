using UnityEngine;

namespace Rope {
	public class Rotator : MonoBehaviour {
		GrappleController controller;
		new Rigidbody2D rigidbody;

		bool faceRight = false;

		void Start() {
			GameObject parent = transform.parent.gameObject;
			controller = parent.GetComponent<GrappleController>();
			rigidbody = parent.GetComponent<Rigidbody2D>();
		}

		void Update() {
			float velX = rigidbody.velocity.x;
			if (velX > 0) faceRight = true;
			else if (velX < 0) faceRight = false;
		}

		void LateUpdate() {
			Quaternion rotation;
			if (controller.isTethered) {
				Vector2 direction = 
					controller.tetherPoint - (Vector2) transform.position;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
			}	else {
				rotation = Quaternion.identity;
			}
				
			
			if (faceRight) {
				rotation *= Quaternion.Euler(0, 180, 0);
			} 
			transform.rotation = rotation;
		}
	}
}