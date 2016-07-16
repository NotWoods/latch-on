using UnityEngine;

namespace Player {
	[RequireComponent (typeof(CharacterController2D))]
	public class MovementInput : MonoBehaviour {
		CharacterController2D player;

		public float accelerationTimeGrounded = 0;
		public float accelerationTimeAirborne = 0.5f;

		public float speed = 2f;
		public float gravity;
		public float jumpVelocity;

		float velocityXSmoothing;

		void Awake() {
			player = GetComponent<CharacterController2D>();
		}

		void Update() {
			Vector2 velocity = player.velocity;
			velocity.y -= gravity * Time.deltaTime;

			float targetVelocityX = Input.GetAxis("Horizontal") * speed;
			float accelerationTime;
			if ((player.collisionFlags & CollisionFlags.Below) != 0) 
				accelerationTime = accelerationTimeGrounded;
			else accelerationTime = accelerationTimeAirborne;
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX,
				ref velocityXSmoothing, accelerationTime);

			if (
				Input.GetKeyDown(KeyCode.Space) &&
				(player.collisionFlags & CollisionFlags.Below) != 0
			) {
				velocity.y += jumpVelocity;
			}


			player.Move(velocity * Time.deltaTime);
		}
	}
}