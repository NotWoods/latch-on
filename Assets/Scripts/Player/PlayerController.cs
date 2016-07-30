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

		protected virtual void Awake() {
			rigidbody = GetComponent<Rigidbody2D>();
			collider = GetComponent<Collider2D>();

			moveForce = rigidbody.mass * moveAcceleration;
			inAirMoveForce = rigidbody.mass * inAirMoveAcceleration;
			jumpForce = rigidbody.mass * jumpAcceleration;
		}

		public virtual void Respawn() {
			transform.position = Vector2.zero;
			rigidbody.velocity = Vector2.zero;
		}

		public float maxSpeed = 5;
		public float moveAcceleration = 365;
		public float inAirMoveAcceleration = 0;
		public float jumpAcceleration = 1000;
		public float groundCheckLength = 0.15f;

		float moveForce, inAirMoveForce, jumpForce;

		public bool isGrounded = false;
		bool shouldJump = false;
		
		protected bool playerCanMove = true;

		protected virtual void Update() {
			if (Input.GetButtonDown("Jump") && isGrounded) shouldJump = true;
		}

		protected virtual void FixedUpdate() {
			isGrounded = CheckGrounded();
			float currentSpeed = Mathf.Sign(rigidbody.velocity.x);
			float currentDirection = Mathf.Sign(rigidbody.velocity.x);

			moveForce = rigidbody.mass * moveAcceleration;
			inAirMoveForce = rigidbody.mass * inAirMoveAcceleration;
			jumpForce = rigidbody.mass * jumpAcceleration;

			if (playerCanMove) {
				float input = Input.GetAxis("Horizontal");
				float inputDirection = Mathf.Sign(input);
				float inputSpeed = Mathf.Abs(input);

				bool changingDirection = currentDirection != inputDirection;

				if (currentSpeed < maxSpeed || changingDirection) 
					rigidbody.AddForce(Vector2.right * input * moveForce);
				
				if (currentSpeed > maxSpeed) {
					rigidbody.velocity = new Vector2(
						maxSpeed * currentDirection,
						rigidbody.velocity.y
					);
				}

				if (shouldJump) {
					rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
					shouldJump = false;
				}
			}
		}

		bool CheckGrounded() {
			Vector2 startPos = new Vector2(
				transform.position.x, collider.bounds.min.y);
			RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.up * -1, 
				groundCheckLength, platformMask);
			return hit? true : false;
		}
	}
}