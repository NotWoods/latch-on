using UnityEngine;
using Prime31;

public class MoveSystem : EgoSystem<Transform, CharacterData, InputData, InspectableLineData, CharacterController2D, PlayerState> {
	private float GetJumpVelocity(CharacterData stats) {
		return Mathf.Sqrt(2f * stats.JumpHeight * -stats.GravityBase);
	}

	private void SetState(PlayerState state, CharacterController2D controller) {
		if (controller.isGrounded) {
			if (state.E == PlayerState.Flung || state.E == PlayerState.Fall) {
				state.Set(PlayerState.Walk);
			}
		} else {
			if (state.E == PlayerState.Walk) state.Set(PlayerState.Fall);
		}
	}

	private Vector2 CalculateSwingingVelocity(Vector2 velocity,
		Transform transform, CharacterData stats, InspectableLineData line
	) {
		velocity.x = Mathf.MoveTowards(velocity.x, 0, Time.deltaTime * stats.SwingDamping);

		Vector2 currentPosition = transform.position;
		Vector2 testPosition = currentPosition + (velocity * Time.deltaTime);
		Vector2 tetherPoint = line.GetLast();

		if (Vector2.Distance(testPosition, tetherPoint) > line.FreeLength) {
			Vector2 direction = testPosition - tetherPoint;
			testPosition = tetherPoint + (direction.normalized * line.FreeLength);
			velocity = (testPosition - currentPosition) / Time.deltaTime;
		}

		return velocity;
	}

	private Vector2 CalculateWalkingVelocity(Vector2 velocity,
		InputData input, CharacterData stats, CharacterController2D controller
	) {
		float damping = controller.isGrounded ? stats.GroundDamping : stats.InAirDamping;
		// TODO: Change to use SmoothDamp instead later
		velocity.x = Mathf.Lerp(
			velocity.x,
			input.HorizontalInput * stats.RunSpeed,
			Time.deltaTime * damping
		);

		return velocity;
	}

	public override void Update() {
		ForEachGameObject((o, transform, stats, input, line, controller, state) => {
			SetState(state, controller);

			Vector2 velocity = stats.Velocity;
			if (controller.isGrounded) {
				velocity.y = input.JumpPressed ? GetJumpVelocity(stats) : 0;

				if (input.SinkPressed) {
					velocity.y *= 3f;
					controller.ignoreOneWayPlatformsThisFrame = true;
				}
			}

			velocity.y += stats.GravityBase * stats.GravityScale * Time.deltaTime;

			if (line.IsAnchored()) {
				velocity = CalculateSwingingVelocity(velocity, transform, stats, line);
			} else if (state.E != PlayerState.Flung) {
				velocity = CalculateWalkingVelocity(velocity, input, stats, controller);
			}

			controller.Move(velocity * Time.deltaTime);
			stats.Velocity = controller.velocity;
		});
	}
}
