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
		if (controller.isGrounded) {
			if (state.Any(PlayerState.Flung, PlayerState.Fall, PlayerState.WallSlide)) {
				state.Set(PlayerState.Walk);
			}
		} else {
			if (state.CurrentMode == PlayerState.Swing) return;
			var collisions = controller.collisionState;

			if ((collisions.left || collisions.right) && !collisions.below
			&& controller.velocity.y < 0) {
				state.Set(PlayerState.WallSlide);
			} else if (state.Any(PlayerState.Walk, PlayerState.WallSlide)) {
				state.Set(PlayerState.Fall);
			}
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
		CharacterData stats, InputData input, CharacterController2D controller
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

	private Vector2 CalculateWallJumpVelocity(Vector2 velocity,
		CharacterData stats, InputData input, CharacterController2D controller
	) {
		if (velocity.y < -stats.MaxWallSlideSpeed) {
			velocity.y = -stats.MaxWallSlideSpeed;
		}

		int wallXDir = controller.collisionState.left ? -1 : 1;
		int inputXSign = ExtraMath.Sign(input.HorizontalInputRaw);

		if (stats.TimeToWallUnstick > 0) {
			velocity.x = 0;
			if (inputXSign != wallXDir && inputXSign != 0) {
				stats.TimeToWallUnstick -= Time.deltaTime;
			} else {
				stats.TimeToWallUnstick = stats.WallStickTime;
			}
		} else {
			stats.TimeToWallUnstick = stats.WallStickTime;
		}

		if (wallXDir == inputXSign) {
			velocity.x = -wallXDir * WallJumpClimb.x;
			velocity.y = WallJumpClimb.y;
		} else if (inputXSign == 0) {
			velocity.x = -wallXDir * WallJumpOff.x;
			velocity.y = WallJumpOff.y;
		} else {
			velocity.x = -wallXDir * WallLeap.x;
			velocity.y = WallLeap.y;
		}

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
			} else if (state.CurrentMode != PlayerState.Flung) {
				velocity = CalculateWalkingVelocity(velocity, stats, input, controller);
			}
			if (input.JumpPressed && state.CurrentMode == PlayerState.WallSlide) {
				velocity = CalculateWallJumpVelocity(velocity, stats, input, controller);
			}

			controller.Move(velocity * Time.deltaTime);
			stats.Velocity = controller.velocity;
		});
	}
}
