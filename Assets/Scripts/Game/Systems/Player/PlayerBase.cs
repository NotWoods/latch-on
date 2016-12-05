using UnityEngine;
using Prime31;

public class MoveSystem {
	public float Gravity = -25f;
	public float RunSpeed = 8f;
	public float GroundDamping = 20f;
	public float InAirDamping = 5f;
	public float JumpHeight = 3f;

	public enum PlayerState {
		Idle, Running, Jumping, Hooked
	}

	public float HorizontalInput = 0;
	public bool WantToJump = false;
	public bool WantToSink = false;
	public PlayerState CurrrentState = PlayerState.Idle;
	private CharacterController2D controller;
	private RaycastHit2D lastControllerColliderHit;
	private Vector2 velocity;

	void Update() {
		if (controller.isGrounded) velocity.y = 0;

		if (HorizontalInput != 0) { CurrrentState = PlayerState.Running; }
		else { CurrrentState = PlayerState.Idle; }

		if (controller.isGrounded && WantToJump) {
			velocity.y = Mathf.Sqrt(2f * JumpHeight * -Gravity);
			CurrrentState = PlayerState.Jumping;
		}

		float damping = controller.isGrounded ? GroundDamping : InAirDamping;
		// TODO: Change to use SmoothDamp instead later
		velocity.x = Mathf.Lerp(velocity.x, HorizontalInput * RunSpeed, Time.deltaTime * damping);

		velocity.y = Gravity * Time.deltaTime;

		if (controller.isGrounded && WantToSink) {
			velocity.y *= 3f;
			controller.ignoreOneWayPlatformsThisFrame = true;
		}

		controller.move(velocity * Time.deltaTime);
		velocity = controller.velocity;
	}
}
