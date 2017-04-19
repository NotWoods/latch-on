using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	/// Manages wall jumping movement
	public class WallJumpingSystem : EgoSystem<WallJumper, Velocity, VJoystick> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, wallJumper, vel, input) => {
				if (wallJumper.AgainstSide == Side.None) return;
				Vector2 velocity = vel.Value;

				if (velocity.y < -wallJumper.MaxSlideSpeed) {
					velocity.y = -wallJumper.MaxSlideSpeed;
				}

				Side inputXSide = (Side) ExtraMath.Sign(input.XMoveAxisRaw);

				bool isSwinging = false;
				MoveState state;
				if (ego.TryGetComponents<MoveState>(out state)) {
					isSwinging = state.Value == MoveType.Swing;
				}

				if (wallJumper.TimeToUnstick > 0 && !isSwinging && inputXSide != Side.None) {
					velocity.x = 0;
					if (inputXSide != wallJumper.AgainstSide) {
						wallJumper.TimeToUnstick -= Time.deltaTime;
					} else {
						wallJumper.ResetUnstickTime();
					}
				} else {
					wallJumper.ResetUnstickTime();
				}

				if (input.JumpPressed) {
				Vector2 modifier;
				if (isSwinging)
					modifier = wallJumper.SwingingJump;
				else if (inputXSide == wallJumper.AgainstSide)
					modifier = wallJumper.ClimbingJump;
				else if (inputXSide == Side.None)
					modifier = wallJumper.FallOffJump;
				else
					modifier = wallJumper.LeapingJump;

				velocity.x = (int) wallJumper.AgainstSide * -modifier.x;
				velocity.y = modifier.y;
			}

				vel.Value = velocity;
			});
		}
	}
}
