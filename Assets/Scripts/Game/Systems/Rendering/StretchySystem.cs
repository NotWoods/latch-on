using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Components.Parts;

namespace LatchOn.ECS.Systems.Rendering {
	public class StretchySystem : EgoSystem<
		EgoConstraint<Stretchy, Transform>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, stretchy, transform) => {
				if (!stretchy.IsStretching) {
					ego.gameObject.SetActive(false);
					return;
				}

				Vector3 between = stretchy.StartPoint - stretchy.EndPoint;
				float distance = between.magnitude;

				transform.localScale = new Vector3(1, 1, distance);
				transform.position = stretchy.EndPoint;
				transform.LookAt(stretchy.StartPoint);

				stretchy.TileMaterial.mainTextureScale = new Vector2(
					distance / stretchy.DefaultScale,
					1
				);

				ego.gameObject.SetActive(true);
			});
		}
	}
}
