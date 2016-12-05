using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class MoveSystem : SystemBase<MoveSystem>, IPlayerSystem {
		public void Update(int id, float deltaTime) {
			PlayerStateComponent state = Manager.GetComponent<PlayerStateComponent>(id);
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			if (input.HookDown) {
				state.SetTo(PlayerState.HookedMovement);
				return;
			}

			CharacterStatsComponent stats = Manager.GetComponent<CharacterStatsComponent>(id);
			CharacterController2D controller = Manager.GetUnityComponent<CharacterController2D>(id);

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
				deltaTime * damping
			);

			velocity.y = stats.Gravity * deltaTime;

			if (controller.isGrounded && input.SinkPressed) {
				velocity.y *= 3f;
				controller.ignoreOneWayPlatformsThisFrame = true;
			}

			controller.move(velocity * Time.deltaTime);
			velocity = controller.velocity;
		}
	}
}