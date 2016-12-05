using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class WalkSystem : SystemBase<WalkSystem> {
		public void Update(int id, float deltaTime) {
			CharacterStatsComponent stats = Manager.GetComponent<CharacterStatsComponent>(id);
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			CharacterController2D controller = Manager.GetComponent<CharacterController2D>(id);
			PlayerStateComponent state = Manager.GetComponent<PlayerStateComponent>(id);

			Vector2 velocity = stats.Velocity;
			velocity.y = 0;

			if (input.HorizontalInput == 0) {
				state.SetTo(PlayerState.Idle);
			}

			if (input.WantToJump) {
				velocity.y = Mathf.Sqrt(2f * stats.JumpHeight * -stats.Gravity);
				state.SetTo(PlayerState.Jump);
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
				state.SetTo(PlayerState.Fall);
			}

			controller.move(velocity * deltaTime);
			stats.Velocity = controller.velocity;
		}
	}
}