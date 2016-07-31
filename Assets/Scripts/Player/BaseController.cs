using UnityEngine;

namespace Player {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BaseControllerFields))]
	public abstract class BaseController : MonoBehaviour {
		[HideInInspector] public new Rigidbody2D rigidbody;
		[HideInInspector] public new Collider2D collider;
		[HideInInspector] public Collider2D sensorCollider;

		[HideInInspector] public LayerMask platformMask;
		[HideInInspector] public Vector2 centerOfMass = Vector2.zero;

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
			BaseControllerFields fields = GetComponent<BaseControllerFields>();
			platformMask = fields.platformMask;
			centerOfMass = fields.centerOfMass;

			rigidbody = GetComponent<Rigidbody2D>();
			Collider2D[] colliders = GetComponents<Collider2D>();
			collider = colliders[0]; sensorCollider = colliders[1];
		}

		protected virtual void OnValidate() {
			if (centerOfMass != Vector2.zero) rigidbody.centerOfMass = centerOfMass;
		}

		public virtual void Respawn() {
			transform.position = Vector2.zero;
			rigidbody.velocity = Vector2.zero;
		}
	}
}