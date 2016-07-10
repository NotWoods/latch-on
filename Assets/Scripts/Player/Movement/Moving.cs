using UnityEngine;
using System;

namespace Player {
	public class Moving : MonoBehaviour, IMoveable {
		public GameObject gameObject {get {return gameObject;}}
		Rigidbody2D rigidbody2d;

		void Awake() {
			rigidbody2d = GetComponent<Rigidbody2D>();
		}

		public void Drive(float velocity) {
			transform.Translate(Vector2.right * velocity, Space.World);
		}

		public float jumpForce = 1;

		public float circleCastRadius = 1;
		public Vector2 circleCastOffset;
		Vector2 circleCastOffsetWorld {
			get { return transform.TransformPoint(circleCastOffset); }
		}

		public int groundLayers;

		public bool Jump() {
			Collider2D hit = Physics2D.OverlapCircle(
				circleCastOffsetWorld, circleCastRadius,
				groundLayers
			);
			if (hit != null) {
				rigidbody2d.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

				return true;
			} else return false;
		}
	}
}