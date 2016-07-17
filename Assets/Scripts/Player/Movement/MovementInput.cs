using UnityEngine;
using Rope;

namespace Player {
	[RequireComponent (typeof(RopeController))]
	public class MovementInput : MonoBehaviour {
		RopeController player;

		public float accelerationTimeGrounded = 0;
		public float accelerationTimeAirborne = 0.5f;

		public float speed = 2f;
		public float gravity;
		public float jumpVelocity;

		float velocityXSmoothing;

		void Awake() {
			player = GetComponent<RopeController>();
		}

		void Update() {
			Vector2 velocity = player.velocity;
			if (player.iAmTethered) velocity.y = 0;
			velocity.y -= gravity * Time.deltaTime;

			if (!player.iAmTethered) {
				float targetVelocityX = Input.GetAxis("Horizontal") * speed;
				float accelerationTime;
				if ((player.collisionFlags & CollisionFlags.Below) != 0) 
					accelerationTime = accelerationTimeGrounded;
				else accelerationTime = accelerationTimeAirborne;
				velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX,
					ref velocityXSmoothing, accelerationTime);
			}

			if (
				Input.GetKeyDown(KeyCode.Space) &&
				(player.collisionFlags & CollisionFlags.Below) != 0
			) {
				velocity.y += jumpVelocity;
			}

			if (Input.GetMouseButtonDown(0)) {
				Vector2 clickPoint = 
					Camera.main.ScreenToWorldPoint(Input.mousePosition);
				player.GrappleToward(clickPoint);
			} else if (!Input.GetMouseButton(0)) {
				player.iAmTethered = false;
			}

			player.Move(velocity * Time.deltaTime);
		}
	}
}