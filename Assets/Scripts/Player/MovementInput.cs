using UnityEngine;

namespace Player {
	public class MovementInput : MonoBehaviour {
		IMoveable player;

		public float metersPerSecond = 1f;

		void Start() {
			player = GetComponent<IMoveable>();
		}

		void Update() {
			float horizontal = Input.GetAxis("Horizontal");
			if (horizontal != 0) {
				player.Drive(horizontal * metersPerSecond * Time.deltaTime);
			}

			bool jumpButton = Input.GetButtonDown("Jump");
			if (jumpButton) player.Jump();
		}
	}
}