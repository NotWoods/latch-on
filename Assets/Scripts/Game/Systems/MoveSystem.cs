using UnityEngine;
using Prime31;

public class MoveSystem : EgoSystem<Transform, CharacterData, InputData, InspectableLineData, CharacterController2D, PlayerState> {
	public float SwingDamping = 0.5f;

	public override void Update() {
		ForEachGameObject((o, transform, stats, input, line, controller, state) => {
			Vector2 velocity = stats.Velocity;
			if (controller.isGrounded) {
				velocity.y = 0;
				if (state.E == PlayerState.Mode.Flung || state.E == PlayerState.Mode.Fall) {
					Debug.Log("hello");
					state.Set(PlayerState.Mode.Walk);
				}
			} else if (state.E == PlayerState.Mode.Walk) {
				state.Set(PlayerState.Mode.Fall);
			}

			if (controller.isGrounded && input.JumpPressed) {
				velocity.y = Mathf.Sqrt(2f * stats.JumpHeight * -stats.Gravity);
			}

			if (!line.IsAnchored()) {
				float damping = controller.isGrounded ? stats.GroundDamping : stats.InAirDamping;
				// TODO: Change to use SmoothDamp instead later
				velocity.x = Mathf.Lerp(
					velocity.x,
					input.HorizontalInput * stats.RunSpeed,
					Time.deltaTime * damping
				);
			} else {
				velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * SwingDamping);
			}

			velocity.y += stats.Gravity * Time.deltaTime;

			if (controller.isGrounded && input.SinkPressed) {
				velocity.y *= 3f;
				controller.ignoreOneWayPlatformsThisFrame = true;
			}

			if (line.IsAnchored()) {
				Vector2 testPosition = (Vector2) transform.position + (velocity * Time.deltaTime);
				Vector2 tetherPoint = line.GetLast();

				if (Vector2.Distance(testPosition, tetherPoint) > line.FreeLength) {
					testPosition = line.GetLast() +
						((testPosition - line.GetLast()).normalized * line.FreeLength);
					velocity = (testPosition - (Vector2) transform.position) / Time.deltaTime;
				}
			}

			controller.Move(velocity * Time.deltaTime);
			stats.Velocity = controller.velocity;
		});
	}
}
