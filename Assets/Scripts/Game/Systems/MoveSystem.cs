using UnityEngine;
using Prime31;

public class MoveSystem : EgoSystem<Transform, CharacterData, WallSlideData, InputData, InspectableLineData, CharacterController2D, PlayerState> {
	private float GetJumpVelocity(CharacterData stats) {
		return Mathf.Sqrt(2f * stats.JumpHeight * -stats.GravityBase);
	}

	private void SetState(PlayerState state, WallSlideData wallData,
		CharacterController2D controller) {
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

		var collided = controller.collisionState;
		if ((collided.left || collided.right) && !collided.below) {
			if (collided.left) wallData.Side = -1;
			else if (collided.right) wallData.Side = 1;
		} else  {
			wallData.Side = 0;
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
		CharacterData stats, WallSlideData wall,
		InputData input, CharacterController2D controller
	) {
		if (velocity.y < -wall.MaxSlideSpeed) velocity.y = -wall.MaxSlideSpeed;
		int inputXSign = ExtraMath.Sign(input.HorizontalInputRaw);

		if (wall.TimeToUnstick > 0) {
			velocity.x = 0;
			if (inputXSign != wall.Side && inputXSign != 0) {
				wall.TimeToUnstick -= Time.deltaTime;
			} else {
				wall.ResetTime();
			}
		} else {
			wall.ResetTime();
		}

		if (input.JumpPressed) {
			Vector2 modifier;
			if (inputXSign == wall.Side) modifier = wall.WallJumpClimb;
			else if (inputXSign == 0) modifier = wall.WallJumpOff;
			else { modifier = wall.WallLeap; }

			velocity.x = -wall.Side * modifier.x;
			velocity.y = modifier.y;
		}
	}

	public override void FixedUpdate() {
		ForEachGameObject((o, transform, stats, wallData, input, line, controller, state) => {
			SetState(state, wallData, controller);

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
			if (wallData.IsSliding) {
				CalculateWallJumpVelocity(ref velocity, stats, wallData, input, controller);
			}

			if (velocity.y < -stats.MaxFallSpeed) velocity.y = -stats.MaxFallSpeed;

			controller.Move(velocity * Time.deltaTime);
			stats.Velocity = controller.velocity;
			input.JumpPressed = input.SinkPressed = false;
		});
	}
}
