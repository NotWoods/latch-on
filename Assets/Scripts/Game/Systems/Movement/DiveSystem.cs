using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	/// Rudimentary diving system.
	public class DiveSystem : EgoSystem<Diver, Velocity, MoveState, VJoystick, CharacterController2D> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, diver, velocity, state, input, controller) => {
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

		bool CanDive(Diver diver, Vector2 velocity, MoveState state, EgoComponent ego) {
			if (!state.IsType(MoveType.Fall | MoveType.Flung)) return false;

			if (ExtraMath.InRange(velocity.y, diver.MinYVelocity, diver.MaxYVelocity)) {
				WallJumper wallJumper;
				if (ego.TryGetComponents<WallJumper>(out wallJumper)) {
					return wallJumper.AgainstSide == Side.None;
				} else {
					return true;
				}
			} else {
				return false;
			}
		}
	}
}
