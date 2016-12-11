using UnityEngine;
using Prime31;

public class MoveSystem : EgoSystem<Transform, CharacterData, InputData, InspectableLineData, CharacterController2D, PlayerState> {
	Vector2 WallJumpClimb = new Vector2(7.5f, 16);
	Vector2 WallJumpOff = new Vector2(8.5f, 7);
	Vector2 WallLeap = new Vector2(18, 17);

	private float GetJumpVelocity(CharacterData stats) {
		return Mathf.Sqrt(2f * stats.JumpHeight * -stats.GravityBase);
	}

	private void SetState(PlayerState state, CharacterController2D controller) {
		switch (state.CurrentMode) {
			case PlayerState.Mode.Flung:
			case PlayerState.Mode.Fall:
				if (controller.isGrounded) state.Set(PlayerState.Walk); break;

			case PlayerState.Mode.Walk:
				if (!controller.isGrounded) state.Set(PlayerState.Fall); break;

			case PlayerState.Mode.Swing:
			default:
				break;
		}
	}

	private void CalculateSwingingVelocity(ref Vector2 velocity,
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
	}

	private void CalculateWalkingVelocity(ref Vector2 velocity,
		CharacterData stats, InputData input, CharacterController2D controller
	) {
		float damping = controller.isGrounded ? stats.GroundDamping : stats.InAirDamping;
		// TODO: Change to use SmoothDamp instead later
		velocity.x = Mathf.Lerp(
			velocity.x,
			input.HorizontalInput * stats.RunSpeed,
			Time.deltaTime * damping
		);
	}

	private void CalculateWallJumpVelocity(ref Vector2 velocity,
		CharacterData stats, InputData input, CharacterController2D controller
	) {
		if (velocity.y < -stats.MaxWallSlideSpeed) {
			velocity.y = -stats.MaxWallSlideSpeed;
		}

		int wallXDir = controller.collisionState.left ? -1 : 1;
		int inputXSign = ExtraMath.Sign(input.HorizontalInputRaw);

		if (stats.TimeToWallUnstick > 0) {
			velocity.x = 0;
			// Debug.Log(inputXSign + " " + wallXDir); // TODO Debug sticking
			if (inputXSign != wallXDir && inputXSign != 0) {
				stats.TimeToWallUnstick -= Time.deltaTime;
			} else {
				stats.TimeToWallUnstick = stats.WallStickTime;
			}
		} else {
			stats.TimeToWallUnstick = stats.WallStickTime;
		}

		if (input.JumpPressed) {
			Vector2 modifier;
			if (wallXDir == inputXSign) modifier = WallJumpClimb;
			else if (inputXSign == 0) modifier = WallJumpOff;
			else modifier = WallLeap;

			velocity.x = -wallXDir * modifier.x;
			velocity.y = modifier.y;
		}
	}

	public override void FixedUpdate() {
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

			if (state.CurrentMode == PlayerState.Swing) {
				CalculateSwingingVelocity(ref velocity, transform, stats, line);
			} else if (state.CurrentMode != PlayerState.Flung) {
				CalculateWalkingVelocity(ref velocity, stats, input, controller);
			}
			if (false) {
				CalculateWallJumpVelocity(ref velocity, stats, input, controller);
			}

			if (velocity.y < -stats.MaxFallSpeed) velocity.y = -stats.MaxFallSpeed;

			controller.Move(velocity * Time.deltaTime);
			stats.Velocity = controller.velocity;
			input.JumpPressed = input.SinkPressed = false;
		});
	}
}
