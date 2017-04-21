using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class MoveSystem : EgoSystem<
		Velocity, CharacterController2D, VJoystick, MoveState,
		HasGravity, Jumper, Speed
	> {
		float GetJumpVelocity(Jumper jumpStats, HasGravity fallStats) {
			return Mathf.Sqrt(2f * jumpStats.JumpHeight * -fallStats.Gravity);
		}

		void CalculateWalkingVelocity(ref Vector2 velocity,
			Speed speed, VJoystick input, MoveState state,
			EgoComponent ego
		) {
			float damp = 1;
			Damping dampDictionary;
			if (ego.TryGetComponents<Damping>(out dampDictionary)) {
				damp = dampDictionary.GetDamping(state.Value);
			}

			// TODO: Change to use SmoothDamp instead later
			velocity.x = Mathf.Lerp(
				velocity.x,
				input.XMoveAxis * speed.Value,
				Time.deltaTime * damp
			);
		}

		MoveType Controllable = MoveType.Walk | MoveType.Fall;

		public override void FixedUpdate() {
			ForEachGameObject((ego, vel, controller, input, state, fallStats, jumpStats, speed) => {
				Vector2 velocity = vel.Value;
				if (controller.isGrounded) {
					velocity.y = input.JumpPressed ? GetJumpVelocity(jumpStats, fallStats) : 0;

					if (input.SinkPressed) {
						velocity.y *= 3f;
						controller.ignoreOneWayPlatformsThisFrame = true;
					}
				}

				velocity.y += fallStats.Gravity * Time.deltaTime;

				if (state.IsType(Controllable)) {
					CalculateWalkingVelocity(ref velocity, speed, input, state, ego);
				}

				vel.Value = velocity;
			});
		}
	}
}
