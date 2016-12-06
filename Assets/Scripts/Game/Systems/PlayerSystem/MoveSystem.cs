using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class MoveSystem : Singleton<MoveSystem>, IPlayerSystem {
		public void Update(
			PlayerStateData state,
			Transform transform,
			CharacterData stats,
			InputData input,
			LineData line,
			CharacterController2D controller
		) {
			if (input.HookDown && HookedSystem.Instance.OnEntry(transform, input, line)) {
				state.SetTo(PlayerState.HookedMovement);
				return;
			}

			Vector2 velocity = stats.Velocity;
			if (controller.isGrounded) velocity.y = 0;

			if (controller.isGrounded && input.JumpPressed) {
				velocity.y = Mathf.Sqrt(2f * stats.JumpHeight * -stats.Gravity);
			}

			float damping = controller.isGrounded ? stats.GroundDamping : stats.InAirDamping;
			// TODO: Change to use SmoothDamp instead later
			velocity.x = Mathf.Lerp(
				velocity.x,
				input.HorizontalInput * stats.RunSpeed,
				Time.deltaTime * damping
			);

			velocity.y = stats.Gravity * Time.deltaTime;

			if (controller.isGrounded && input.SinkPressed) {
				velocity.y *= 3f;
				controller.ignoreOneWayPlatformsThisFrame = true;
			}

			controller.move(velocity * Time.deltaTime);
			velocity = controller.velocity;
		}
	}
}