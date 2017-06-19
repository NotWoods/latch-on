using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Components.Parts;

namespace LatchOn.ECS.Systems.Rendering {
	public class HideWhenGrapplingSystem : EgoSystem<
		EgoConstraint<HideWhenGrappling, LineData>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, hide, line) => {
				bool armsIn = !line.IsAnchored;
				if (armsIn) {
					CanGrapple grapple;
					if (ego.TryGetComponents(out grapple)) armsIn = !grapple.DidThrow;
				}

				foreach (var toHide in hide.ToHide) {
					toHide.enabled = armsIn;
					foreach (Transform child in toHide.transform) {
						child.GetComponent<MeshRenderer>().enabled = armsIn;
					}
				}
			});
		}
	}
}
