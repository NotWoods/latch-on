using UnityEngine;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class RotatorSystem : EgoSystem<Transform, LineData> {
		public override void Update() {
			ForEachGameObject((ego, transform, line) => {
				Quaternion rotation;
				if (line.Anchored()) {
					Vector2 direction = line.AnchorPoint - (Vector2) transform.position;
					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
					rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
				} else {
					rotation = Quaternion.identity;
				}

				transform.rotation = rotation;
			});
		}
	}
}
