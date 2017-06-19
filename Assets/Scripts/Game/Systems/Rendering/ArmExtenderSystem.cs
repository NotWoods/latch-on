using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Components.Parts;

namespace LatchOn.ECS.Systems.Rendering {
	public class ArmExtenderSystem : EgoSystem<WorldPosition, LineData, RobotExtenderPart> {
		public override void Update() {
			ForEachGameObject((ego, position, line, arm) => {
				bool armsIn = !line.IsAnchored;

				if (armsIn) {
					CanGrapple grapple;
					if (ego.TryGetComponents(out grapple)) armsIn = !grapple.DidThrow;
				}

				foreach (var toHide in arm.ToHide) {
					toHide.GetComponent<MeshRenderer>().enabled = armsIn;
					foreach (Transform child in toHide) {
						child.GetComponent<MeshRenderer>().enabled = armsIn;
					}
				}

				if (!line.IsAnchored) {
					arm.Part.localScale = new Vector3(1, 1, arm.DefaultScale);
					arm.Part.localRotation = Quaternion.Euler(0, 90, 0);
					return;
				}

				Vector3 between = line.AnchorPoint - (Vector2) arm.Part.position;
				float distance = between.magnitude;

				arm.Part.localScale = new Vector3(1, 1, distance);
				arm.Part.LookAt(line.AnchorPoint);

				arm.PartMaterial.mainTextureScale = new Vector2(distance / arm.DefaultScale, 1);
			});
		}
	}
}
