using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems.Movement {
	/// Finalized move step
	public class ApplyMoveSystem : EgoSystem<
		EgoConstraint<Velocity, CharacterController2D>
	> {
		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, vel, controller) => {
				Vector2 velocity = vel.Value;

				/// Limit max falling speed if needed
				Descends stats;
				if (ego.TryGetComponents(out stats)) {
					if (velocity.y < -stats.MaxFallSpeed) velocity.y = -stats.MaxFallSpeed;
				}

				controller.Move(velocity * Time.deltaTime);
				vel.Value = controller.velocity;

				VJoystick input;
				if (ego.TryGetComponents(out input)) {
					input.JumpPressed = false;
					input.SinkPressed = false;
				}
			});
		}
	}
}
