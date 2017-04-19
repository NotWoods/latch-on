using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems {
	public class ApplyMoveSystem : EgoSystem<
		Velocity, CharacterController2D, VJoystick, HasGravity
	> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, vel, controller, input, hasGravity) => {
				Vector2 velocity = vel.Value;

				if (velocity.y < -hasGravity.MaxFallSpeed)
					velocity.y = -hasGravity.MaxFallSpeed;

				controller.Move(velocity * Time.deltaTime);
				vel.Value = controller.velocity;

				input.JumpPressed = false;
				input.SinkPressed = false;
			});
		}
	}
}
