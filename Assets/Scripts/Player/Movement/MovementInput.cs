using UnityEngine;

namespace Player {
	[RequireComponent (typeof(CharacterController2D))]
	public class MovementInput : MonoBehaviour {
		CharacterController2D player;

		public float speed = 2f;
		public float gravity = -20f;

		void Start() {
			player = GetComponent<CharacterController2D>();
		}

		void FixedUpdate() {
			Vector2 velocity = Vector2.zero;
			velocity.x = Input.GetAxis("Horizontal") * speed;
			velocity.y = gravity * Time.fixedDeltaTime;
			player.Move(velocity * Time.fixedDeltaTime);
		}
	}
}