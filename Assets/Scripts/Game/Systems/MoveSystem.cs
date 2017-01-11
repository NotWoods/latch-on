using UnityEngine;
using Prime31;

public class MoveSystem : EgoSystem<WorldPosition, CharacterData, WallJumper, VJoystick, LineData, CharacterController2D, MoveState, Velocity, Damping> {
	private float GetJumpVelocity(CharacterData stats) {
		return Mathf.Sqrt(2f * stats.JumpHeight * -stats.GravityBase);
	}

	private void SetState(MoveState state, WallJumper wallData,
		CharacterController2D controller, VJoystick input) {
		switch (state.Value) {
			case MoveState.Mode.Flung:
				if (wallData.IsSliding) state.Value = MoveState.Fall;
				goto case MoveState.Mode.Fall;
			case MoveState.Mode.Fall:
				if (controller.isGrounded) state.Value = MoveState.Walk; break;

			case MoveState.Mode.Walk:
				if (!controller.isGrounded) state.Value = MoveState.Fall; break;

			case MoveState.Mode.Swing:
			default:
				break;
		}

		var collided = controller.collisionState;
		if ((collided.left || collided.right) && !collided.below) {
			if (collided.left) wallData.AgaisntSide = -1;
			else if (collided.right) wallData.AgaisntSide = 1;
		} else  {
			wallData.AgaisntSide = 0;
		}
	}

	private void CalculateSwingingVelocity(ref Vector2 velocity,
		Vector2 position, Damping damping, LineData line
	) {
		velocity.x = Mathf.MoveTowards(
			velocity.x, 0, Time.deltaTime * damping.GetValue(MoveState.Swing)
		);

		Vector2 currentPosition = position;
		Vector2 testPosition = currentPosition + (velocity * Time.deltaTime);
		Vector2 tetherPoint = line.WorldAnchor;

		if (Vector2.Distance(testPosition, tetherPoint) > line.FreeLength) {
			Vector2 direction = testPosition - tetherPoint;
			testPosition = tetherPoint + (direction.normalized * line.FreeLength);
			velocity = (testPosition - currentPosition) / Time.deltaTime;
		}
	}

	private void CalculateWalkingVelocity(ref Vector2 velocity,
		CharacterData stats, VJoystick input, CharacterController2D controller,
		Damping damping
	) {
		float damp = damping.GetValue(controller.isGrounded ? MoveState.Walk : MoveState.Fall);
		// TODO: Change to use SmoothDamp instead later
		velocity.x = Mathf.Lerp(
			velocity.x,
			input.XMoveAxis * stats.RunSpeed,
			Time.deltaTime * damp
		);
	}

	private void CalculateWallJumpVelocity(ref Vector2 velocity,
		WallJumper wall, VJoystick input, MoveState state
	) {
		if (velocity.y < -wall.MaxSlideSpeed) velocity.y = -wall.MaxSlideSpeed;
		int inputXSign = ExtraMath.Sign(input.XMoveAxisRaw);

		if (wall.TimeToUnstick > 0 && state.Value != MoveState.Swing && inputXSign != 0) {
			velocity.x = 0;
			if (inputXSign != wall.AgaisntSide) {
				wall.TimeToUnstick -= Time.deltaTime;
			} else {
				wall.ResetTime();
			}
		} else {
			wall.ResetTime();
		}

		if (input.JumpPressed) {
			Vector2 modifier;
			if (state.Value == MoveState.Swing) modifier = wall.SwiningJump;
			else if (inputXSign == wall.AgaisntSide) modifier = wall.ClimbingJump;
			else if (inputXSign == 0) modifier = wall.FallOffJump;
			else { modifier = wall.LeapingJump; }

			velocity.x = -wall.AgaisntSide * modifier.x;
			velocity.y = modifier.y;
		}
	}

	public override void FixedUpdate() {
		ForEachGameObject((o, position, stats, wallData, input, line, controller, state, vel, damping) => {
			SetState(state, wallData, controller, input);

			Vector2 velocity = vel.Value;
			if (controller.isGrounded) {
				velocity.y = input.JumpPressed ? GetJumpVelocity(stats) : 0;

				if (input.SinkPressed) {
					velocity.y *= 3f;
					controller.ignoreOneWayPlatformsThisFrame = true;
				}
			}

			velocity.y += stats.GravityBase * stats.GravityScale * Time.deltaTime;

			if (state.Value == MoveState.Swing) {
				CalculateSwingingVelocity(ref velocity, position.Value, damping, line);
			} else if (state.Value != MoveState.Flung) {
				CalculateWalkingVelocity(ref velocity, stats, input, controller, damping);
			}
			if (wallData.IsSliding) {
				CalculateWallJumpVelocity(ref velocity, wallData, input, state);
			}

			if (velocity.y < -stats.MaxFallSpeed) velocity.y = -stats.MaxFallSpeed;

			controller.Move(velocity * Time.deltaTime);
			vel.Value = controller.velocity;
			input.JumpPressed = input.SinkPressed = false;
		});
	}
}
