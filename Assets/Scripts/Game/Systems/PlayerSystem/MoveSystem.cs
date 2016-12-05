using UnityEngine;
using Prime31;

namespace PlayerSystem {
	public class MoveSystem : SystemBase<MoveSystem>, IPlayerSystem {
		public void Update(int id, float deltaTime) {
			CharacterStatsComponent stats = Manager.GetComponent<CharacterStatsComponent>(id);
			InputComponent input = Manager.GetComponent<InputComponent>(id);
			CharacterController2D controller = Manager.GetComponent<CharacterController2D>(id);
			PlayerStateComponent state = Manager.GetComponent<PlayerStateComponent>(id);

			Vector2 velocity = stats.Velocity;
			if (controller.isGrounded) velocity.y = 0;

			if (controller.isGrounded && input.WantToJump) {
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

			if (controller.isGrounded && input.WantToSink) {
				velocity.y *= 3f;
				controller.ignoreOneWayPlatformsThisFrame = true;
			}

			controller.move(velocity * Time.deltaTime);
			velocity = controller.velocity;
		}
	}
}