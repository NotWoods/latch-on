using UnityEngine;
using Prime31;

public class ApplyMoveSystem : EgoSystem<Velocity, CharacterController2D> {
	public override void FixedUpdate() {
		ForEachGameObject((ego, vel, controller) => {
			Vector2 velocity = vel.Value;

			CharacterData stats;
			if (ego.TryGetComponents<CharacterData>(out stats)) {
				if (velocity.y < -stats.MaxFallSpeed) velocity.y = -stats.MaxFallSpeed;
			}

			controller.Move(velocity * Time.deltaTime);
			vel.Value = controller.velocity;

			VJoystick input;
			if (ego.TryGetComponents<VJoystick>(out input)) {
				input.JumpPressed = false;
				input.SinkPressed = false;
			}
		});
	}
}
