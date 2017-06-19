using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Components.Parts;

namespace LatchOn.ECS.Systems.Rendering {
	public class ArmExtenderSystem : EgoSystem<WorldPosition, LineData, RobotExtenderPart> {
		public override void Update() {
			ForEachGameObject((ego, position, line, arm) => {
				if (!line.IsAnchored) {
					arm.Part.localScale = new Vector3(1, 1, arm.DefaultScale);
					arm.Part.localRotation = Quaternion.Euler(0, 90, 0);
					return;
				}

				Vector3 between = line.AnchorPoint - (Vector2) arm.Part.position;
				float distance = between.magnitude;

				arm.Part.localScale = new Vector3(1, 1, distance);
				arm.Part.LookAt(line.AnchorPoint);
			});
		}
	}
}
