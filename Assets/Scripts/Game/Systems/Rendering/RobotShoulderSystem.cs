using UnityEngine;
using LatchOn.ECS.Components.Parts;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class RobotShoulderSystem : EgoSystem<
		EgoConstraint<RobotShoulderPart, BodyPart, LineData>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, shoulder, body, line) => {
				if (!line.IsAnchored) {
					shoulder.LeftArm.rotation = Quaternion.Euler(0, 0, 180);
					shoulder.RightArm.rotation = Quaternion.Euler(0, 0, 180);
					return;
				}

				Vector2 diff = line.AnchorPoint - (Vector2) shoulder.LeftArm.position;
				diff.Normalize();

				float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

				bool facingLeft = Mathf.Sign(body.Part.localScale.x) == -1;
				if (facingLeft) {
					rot_z -= 180;
				}

				shoulder.LeftArm.rotation = Quaternion.Euler(0, 0, rot_z);
				shoulder.RightArm.rotation = Quaternion.Euler(0, 0, rot_z);
			});
		}
	}
}
