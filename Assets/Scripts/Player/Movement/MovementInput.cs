using UnityEngine;

namespace Player {
	[RequireComponent (typeof(CharacterController2D))]
	public class MovementInput : MonoBehaviour {
		CharacterController2D player;

		public float speed = 2f;
		public float gravity = -20f;
		public float jumpVelocity = 8;

		void Start() {
			player = GetComponent<CharacterController2D>();
		}

		void Update() {
			Vector2 velocity = player.velocity;
			velocity.x = Input.GetAxis("Horizontal") * speed;
			velocity.y += gravity * Time.deltaTime;

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