using UnityEngine;

namespace Player {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlatformerCharacter))]
	public class PlayerController : MonoBehaviour {
		[HideInInspector]
		public new Rigidbody2D rigidbody;
		protected PlatformerCharacter controller;

		public LayerMask platformMask = 0;

		void Start() {
			rigidbody = GetComponent<Rigidbody2D>();
			controller = GetComponent<PlatformerCharacter>();
		}

		public virtual void Respawn() {
			transform.position = Vector2.zero;
			rigidbody.velocity = Vector2.zero;
		}

		protected virtual void Update() {
			float h = Input.GetAxis("Horizontal");
    	// Pass all parameters to the character control script.
      //controller.Move(h, false, Input.GetButtonDown("Jump"));
		}
	}
}