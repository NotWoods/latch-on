using UnityEngine;
using Prime31;

public class MoveSystem : EgoSystem<CharacterData, Velocity, CharacterController2D, VJoystick, MoveState> {
	private float GetJumpVelocity(CharacterData stats) {
		return Mathf.Sqrt(2f * stats.JumpHeight * -stats.GravityBase);
	}

	private void CalculateWalkingVelocity(ref Vector2 velocity,
		CharacterData stats, VJoystick input, MoveState state,
		EgoComponent ego
	) {
		float damp = 1;
		Damping dampDictionary;
		if (ego.TryGetComponents<Damping>(out dampDictionary)) {
			damp = dampDictionary.GetValue(state.Value);
		}

		// TODO: Change to use SmoothDamp instead later
		velocity.x = Mathf.Lerp(
			velocity.x,
			input.XMoveAxis * stats.RunSpeed,
			Time.deltaTime * damp
		);
	}

	public override void FixedUpdate() {
		ForEachGameObject((ego, stats, vel, controller, input, state) => {
			Vector2 velocity = vel.Value;
			if (controller.isGrounded) {
				velocity.y = input.JumpPressed ? GetJumpVelocity(stats) : 0;

				if (input.SinkPressed) {
					velocity.y *= 3f;
					controller.ignoreOneWayPlatformsThisFrame = true;
				}
			}

			velocity.y += stats.GravityBase * stats.GravityScale * Time.deltaTime;

			if (!state.Any(MoveState.Swing, MoveState.Flung)) {
				CalculateWalkingVelocity(ref velocity, stats, input, state, ego);
			}

			vel.Value = velocity;
		});
	}
}
