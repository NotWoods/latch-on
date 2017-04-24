using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class MoveSystem : EgoSystem<
		Velocity, CharacterController2D, VJoystick, MoveState,
		HasGravity, Jumper, Speed, Damping
	> {
		float GetJumpVelocity(Jumper jumpStats, HasGravity fallStats) {
			return Mathf.Sqrt(2f * jumpStats.JumpHeight * -fallStats.Gravity);
		}

		void CalculateWalkingVelocity(ref Vector2 velocity,
			Speed speed, VJoystick input, MoveState state, Damping dampDictionary,
			EgoComponent ego
		) {
			float damp = dampDictionary.GetDamping(state.Value);

			// TODO: Change to use SmoothDamp instead later
			velocity.x = Mathf.Lerp(
				velocity.x,
				input.XMoveAxis * speed.Value,
				Time.deltaTime * damp
			);
		}

		MoveType Controllable = MoveType.Walk | MoveType.Fall;

		public override void FixedUpdate() {
			ForEachGameObject(
			(ego, vel, controller, input, state, fallStats, jumpStats, speed, damp) => {
				Vector2 velocity = vel.Value;
				if (controller.isGrounded) {
					velocity.y = 0;
					if (input.JumpPressed) velocity.y = GetJumpVelocity(jumpStats, fallStats);

					if (input.SinkPressed) {
						velocity.y *= 3f;
						controller.ignoreOneWayPlatformsThisFrame = true;
					}
				}

				velocity.y += fallStats.Gravity * Time.deltaTime;

				if (state.IsType(Controllable)) {
					CalculateWalkingVelocity(ref velocity, speed, input, state, damp, ego);
				}

				vel.Value = velocity;
			});
		}
	}
}
