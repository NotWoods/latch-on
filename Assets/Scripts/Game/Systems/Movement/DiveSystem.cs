using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems.Movement {
	/// Rudimentary diving system.
	public class DiveSystem : EgoSystem<Diver, Velocity, MoveState, VJoystick, CharacterController2D> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, diver, velocity, state, input, controller) => {
				if (state.Value == MoveState.Dive) {
					if (controller.collisionState.hasCollision()) {
						state.Value = MoveState.Fall;
						return;
					}

					velocity.Value = new Vector2(
						Mathf.Sign(velocity.x) * diver.DivingVelocity.x,
						diver.DivingVelocity.y
					);
				} else if (input.SinkPressed && CanDive(diver, velocity.Value, state, ego)) {
					state.Value = MoveState.Dive;
				}
			});
		}

		private bool CanDive(Diver diver, Vector2 velocity, MoveState state, EgoComponent ego) {
			if (!state.Any(MoveState.Fall, MoveState.Flung)) return false;

			if (velocity.y >= diver.MinYVelocity && velocity.y <= diver.MaxYVelocity) {
				WallJumper wallJumper;
				if (ego.TryGetComponents<WallJumper>(out wallJumper)) {
					return !wallJumper.IsSliding;
				} else {
					return true;
				}
			} else {
				return false;
			}
		}
	}
}
