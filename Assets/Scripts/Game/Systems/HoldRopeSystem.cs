using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems {
	public class HoldRopeSystem : EgoSystem<PlayerSprites, MoveState> {
		Vector2 normalLeftHand = new Vector2(0.43f, -0.4f);
		Vector2 normalRightHand = new Vector2(0.65f, -0.25f);

		Vector2 raisedLeftHand = new Vector2(0, 0.9f);
		Vector2 raisedRightHand = new Vector2(0.06f, 1.1f);

		public override void Update() {
			ForEachGameObject((ego, sprites, state) => {
				Transform leftHand = sprites.LeftHand;
				Transform rightHand = sprites.RightHand;

				if (state.Value == MoveState.Swing) {
					SetPos(leftHand, raisedLeftHand);
					SetPos(rightHand, raisedRightHand);
				} else {
					SetPos(leftHand, normalLeftHand);
					SetPos(rightHand, normalRightHand);
				}
			});
		}

		private void SetPos(Transform transform, Vector2 position) {
			transform.localPosition = (Vector3) position + (Vector3.forward * -1);
		}
	}
}
