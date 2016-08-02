using UnityEngine;

namespace Player {
	[RequireComponent(typeof(Rigidbody2D))]
	public abstract class BaseController : MonoBehaviour {
		[HideInInspector] public new Rigidbody2D rigidbody;
		[HideInInspector] public new Collider2D collider;
		[HideInInspector] public Collider2D sensorCollider;

		public LayerMask platformMask;

		///True if player is touching the ground. 
		///Remains true for a 10th of a second after leaving the ground
		[HideInInspector] public bool isGrounded;
		float lastGroundTime = 0;
		protected virtual void FixedUpdate() {
			// Stay 'grounded' for an extra 0.1 seconds
			isGrounded = sensorCollider.IsTouchingLayers(platformMask);
			if (isGrounded) lastGroundTime = Time.fixedTime;
			else if (Time.fixedTime - lastGroundTime < 0.1) isGrounded = true;
		}

		public Bounds bounds {
			get {
				Bounds b = collider.bounds;
				Bounds bottom = sensorCollider.bounds;
				b.Encapsulate(bottom.min); b.Encapsulate(bottom.max);
				return b;
			}
		}

		protected virtual void Awake() {
			rigidbody = GetComponent<Rigidbody2D>();
			Collider2D[] colliders = GetComponents<Collider2D>();
			collider = colliders[0]; sensorCollider = colliders[1];
		}

		public virtual void Respawn() {
			transform.position = Vector2.zero;
			rigidbody.velocity = Vector2.zero;
		}
	}
}