using UnityEngine;
using Prime31;

public class PlayerWalkSystem {
	void Update(PlayerEntity player, float deltaTime) {
		CharacterStatsComponent stats = player.GetComponentOfType<CharacterStatsComponent>();
		InputComponent input = player.GetComponentOfType<InputComponent>();
		CharacterController2D controller = player.GetComponentOfType<CharacterController2D>();

		Vector2 velocity = stats.Velocity;
		velocity.y = 0;

		if (input.HorizontalInput == 0) {
			player.CurrentState = PlayerEntity.State.Idle;
			return;
		}
		if (input.WantToJump) {
			velocity.y = Mathf.Sqrt(2f * stats.JumpHeight * -stats.Gravity);
			player.CurrentState = PlayerEntity.State.Jump;
		}

		// TODO: Change to use SmoothDamp instead
		velocity.x = Mathf.Lerp(
			velocity.x,
			input.HorizontalInput * stats.RunSpeed,
			deltaTime * stats.GroundDamping
		);
		velocity.y = stats.Gravity * deltaTime;

		if (input.WantToSink) {
			velocity.y *= 3f;
			controller.ignoreOneWayPlatformsThisFrame = true;
			player.CurrentState = PlayerEntity.State.Fall;
		}

		controller.move(velocity * deltaTime);
		stats.Velocity = controller.velocity;
	}
}
