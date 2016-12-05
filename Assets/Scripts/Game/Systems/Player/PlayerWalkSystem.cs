using UnityEngine;
using Prime31;

public class PlayerWalkSystem {
	void Update(PlayerEntity player, float deltaTime) {
		velocity.y = 0;

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
