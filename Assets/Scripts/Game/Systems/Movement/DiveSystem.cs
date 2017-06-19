using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems.Movement {
	/// Rudimentary diving system.
	public class DiveSystem : EgoSystem<
		EgoConstraint<Diver, Velocity, MoveState, VJoystick, CharacterController2D>
	> {
		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, diver, velocity, state, input, controller) => {
				if (state.Value == MoveType.Dive) {
					if (controller.collisionState.hasCollision()) {
						state.Value = MoveType.Fall;
						return;
					}

					velocity.Value = new Vector2(
						Mathf.Sign(velocity.x) * diver.DivingVelocity.x,
						diver.DivingVelocity.y
					);
				} else if (input.SinkPressed && CanDive(diver, velocity.Value, state, ego)) {
					state.Value = MoveType.Dive;
				}
			});
		}

		private bool CanDive(Diver diver, Vector2 velocity, MoveState state, EgoComponent ego) {
			if (!state.Any(MoveType.Fall, MoveType.Flung)) return false;

			if (!ExtraMath.InRange(velocity.y, diver.MinYVelocity, diver.MaxYVelocity))
				return false;

			WallJumper wallJumper;
			if (ego.TryGetComponents(out wallJumper))
				return wallJumper.AgainstSide == Side.None;
			else
				return true;
		}
	}
}
