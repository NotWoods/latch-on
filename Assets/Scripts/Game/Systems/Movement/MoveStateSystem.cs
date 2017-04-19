using Prime31;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems {
	/// Updates MoveState and WallJumper
	public class MoveStateSystem : EgoSystem<MoveState, CharacterController2D> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, state, controller) => {
				WallJumper wallJumper;
				bool canWallJump = ego.TryGetComponents<WallJumper>(out wallJumper);

				switch (state.Value) {
					case MoveType.Flung:
						if (canWallJump && wallJumper.AgainstSide != Side.None)
							state.Value = MoveType.Fall;
						goto case MoveType.Fall;

					case MoveType.Fall:
						if (controller.isGrounded) state.Value = MoveType.Walk; break;

					case MoveType.Walk:
						if (!controller.isGrounded) state.Value = MoveType.Fall; break;
				}

				if (canWallJump) {
					var collided = controller.collisionState;
					if (!collided.below && (collided.left || collided.right)) {
						if (collided.left) wallJumper.AgainstSide = Side.Left;
						else wallJumper.AgainstSide = Side.Right;
					} else {
						wallJumper.AgainstSide = Side.None;
					}
				}
			});
		}
	}
}
