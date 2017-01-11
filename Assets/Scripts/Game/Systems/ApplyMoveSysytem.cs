using UnityEngine;
using Prime31;

public class ApplyMoveSystem : EgoSystem<Velocity, CharacterController2D> {
	public override void FixedUpdate() {
		ForEachGameObject((ego, velocity, controller) => {
			controller.Move(velocity.Value * Time.deltaTime);
			velocity.Value = controller.velocity;

			VJoystick input;
			if (ego.TryGetComponents<VJoystick>(out input)) {
				input.JumpPressed = false;
				input.SinkPressed = false;
			}
		});
	}
}
