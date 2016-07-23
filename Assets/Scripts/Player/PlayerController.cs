using UnityEngine;

namespace Player {
	[RequireComponent(typeof(Rigidbody2D))]
	//[RequireComponent(typeof(PlatformerCharacter))]
	public class PlayerController : MonoBehaviour {
		[HideInInspector]
		public new Rigidbody2D rigidbody;
		[HideInInspector]
		public new Collider2D collider;
		//protected PlatformerCharacter controller;

		public LayerMask platformMask = 0;

		void Awake() {
			rigidbody = GetComponent<Rigidbody2D>();
			collider = GetComponent<Collider2D>();
			//controller = GetComponent<PlatformerCharacter>();
		}

		public virtual void Respawn() {
			transform.position = Vector2.zero;
			rigidbody.velocity = Vector2.zero;
		}

		protected virtual void Update() {
			
		}
	}
}