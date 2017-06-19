using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems.Movement {
	public class MoveSystem : EgoSystem<
		EgoConstraint<Descends, Velocity, CharacterController2D, VJoystick, MoveState, Speed>
	> {
		private static float GetJumpVelocity(Descends stats) {
			return Mathf.Sqrt(2f * stats.JumpHeight * -stats.Gravity);
		}

		private static void CalculateWalkingVelocity(ref Vector2 velocity,
			float runSpeed, VJoystick input, MoveState state,
			EgoComponent ego
		) {
			float damp = 1;
			Damping dampDictionary;
			if (ego.TryGetComponents(out dampDictionary)) {
				damp = dampDictionary.GetValue(state.Value);
			}

			// TODO: Change to use SmoothDamp instead later
			velocity.x = Mathf.Lerp(
				velocity.x,
				input.XMoveAxis * runSpeed,
				Time.deltaTime * damp
			);
		}

		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, stats, vel, controller, input, state, speed) => {
				Vector2 velocity = vel.Value;
				if (controller.isGrounded) {
					velocity.y = input.JumpPressed ? GetJumpVelocity(stats) : 0;

					if (input.SinkPressed) {
						velocity.y *= 3f;
						controller.ignoreOneWayPlatformsThisFrame = true;
					}
				}

				velocity.y += stats.Gravity * Time.deltaTime;

				if (state.Any(MoveType.Walk, MoveType.Fall)) {
					CalculateWalkingVelocity(ref velocity, speed.Value, input, state, ego);
				}

				vel.Value = velocity;
			});
		}
	}
}
