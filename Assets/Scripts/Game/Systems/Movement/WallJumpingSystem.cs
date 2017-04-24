using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems.Movement {
	/// Manages wall jumping movement
	public class WallJumpingSystem : EgoSystem<WallJumper, Velocity, VJoystick> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, wallJumper, vel, input) => {
				if (!wallJumper.IsSliding) return;
				Vector2 velocity = vel.Value;

				if (velocity.y < -wallJumper.MaxSlideSpeed) {
					velocity.y = -wallJumper.MaxSlideSpeed;
				}
				int inputXSign = ExtraMath.Sign(input.XMoveAxisRaw);

				bool isSwinging = false;
				MoveState state;
				if (ego.TryGetComponents<MoveState>(out state)) {
					isSwinging = state.Value == MoveState.Swing;
				}

				if (wallJumper.TimeToUnstick > 0 && !isSwinging && inputXSign != 0) {
					velocity.x = 0;
					if (inputXSign != wallJumper.AgaisntSide) {
						wallJumper.TimeToUnstick -= Time.deltaTime;
					} else {
						wallJumper.ResetTime();
					}
				} else {
					wallJumper.ResetTime();
				}

				if (input.JumpPressed) {
				Vector2 modifier;
				if (isSwinging)
					modifier = wallJumper.SwiningJump;
				else if (inputXSign == wallJumper.AgaisntSide)
					modifier = wallJumper.ClimbingJump;
				else if (inputXSign == 0)
					modifier = wallJumper.FallOffJump;
				else
					modifier = wallJumper.LeapingJump;

				velocity.x = -wallJumper.AgaisntSide * modifier.x;
				velocity.y = modifier.y;
			}

				vel.Value = velocity;
			});
		}
	}
}
