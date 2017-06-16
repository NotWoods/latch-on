using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Parts;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class SpriteRotationSystem : EgoSystem<BodyPart, Velocity, LineData, WorldPosition> {
		public override void Update() {
			ForEachGameObject((ego, body, velocity, line, position) => {
				bool anchored = line.IsAnchored;

				if (!anchored || Mathf.Abs(velocity.x) > 0.1f) {
					float xDirection = Mathf.Sign(velocity.x);
					if (body.ShouldFlip) xDirection *= -1;
					body.Part.localScale = new Vector3(xDirection, 1, 1);
				}

				Quaternion rotation = Quaternion.identity;
				if (anchored) {
					Vector2 direction = line.AnchorPoint - position.Value;
					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
					rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
				}

				body.Part.rotation = Quaternion.Lerp(
					body.Part.rotation,
					rotation,
					Time.deltaTime * 4
				);
			});
		}
	}
}
