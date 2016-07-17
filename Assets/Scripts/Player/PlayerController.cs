using UnityEngine;

namespace Player {
	[RequireComponent(typeof(Rigidbody2D))]
	//[RequireComponent(typeof(PlatformerCharacter))]
	public class PlayerController : MonoBehaviour {
		[HideInInspector]
		public new Rigidbody2D rigidbody;
		//protected PlatformerCharacter controller;

		public LayerMask platformMask = 0;

		void Awake() {
			rigidbody = GetComponent<Rigidbody2D>();
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