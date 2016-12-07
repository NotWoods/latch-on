using UnityEngine;
using Prime31;

public class MoveSystem : EgoSystem<Transform, CharacterData, InputData, LineData, CharacterController2D> {
	public override void Update() {
		ForEachGameObject((ego, transform, stats, input, line, controller) => {
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

			velocity.y += stats.Gravity * Time.deltaTime;

			if (controller.isGrounded && input.SinkPressed) {
				velocity.y *= 3f;
				controller.ignoreOneWayPlatformsThisFrame = true;
			}

			controller.move(velocity * Time.deltaTime);
			stats.Velocity = controller.velocity;
		});
	}
}
