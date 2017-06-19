using UnityEngine;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class RotatorSystem : EgoSystem<
		EgoConstraint<Transform, LineData>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, transform, line) => {
				Quaternion rotation = Quaternion.identity;
				if (line.IsAnchored) {
					Vector2 direction = line.AnchorPoint - (Vector2) transform.position;
					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
					rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
				}

				transform.rotation = rotation;
			});
		}
	}
}
