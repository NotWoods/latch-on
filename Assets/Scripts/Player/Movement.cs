using UnityEngine;

namespace Player {
	public class Movement : MonoBehaviour {
		IMoveable player;

		void Start() {
			player = GetComponent<IMoveable>();
		}

		void Update() {
			float horizontal = Input.GetAxis("Horizontal");
			if (horizontal != 0) player.Drive(horizontal);

			bool jumpButton = Input.GetButtonDown("Jump");
			if (jumpButton) player.Jump();
		}
	}
}