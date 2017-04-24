using UnityEngine;
using Prime31;
using LatchOn.ECS.Components.Mover;

/// Updates MoveState and WallJumper
public class MoveStateSystem : EgoSystem<MoveState, CharacterController2D> {
	public override void FixedUpdate() {
		ForEachGameObject((ego, state, controller) => {
			WallJumper wallJumper;
			bool canWallJump = ego.TryGetComponents<WallJumper>(out wallJumper);

			switch (state.Value) {
				case MoveState.Mode.Flung:
					if (canWallJump && wallJumper.IsSliding) state.Value = MoveState.Fall;
					goto case MoveState.Mode.Fall;

				case MoveState.Mode.Fall:
					if (controller.isGrounded) state.Value = MoveState.Walk; break;

				case MoveState.Mode.Walk:
					if (!controller.isGrounded) state.Value = MoveState.Fall; break;
			}

			if (canWallJump) {
				var collided = controller.collisionState;
				if (!collided.below && (collided.left || collided.right)) {
					if (collided.left) wallJumper.AgaisntSide = -1;
					else wallJumper.AgaisntSide = 1;
				} else {
					wallJumper.AgaisntSide = 0;
				}
			}
		});
	}
}
