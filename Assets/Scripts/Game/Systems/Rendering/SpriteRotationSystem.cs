using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class SpriteRotationSystem : EgoSystem<PlayerSprites, Velocity, LineData, WorldPosition> {
		public override void Update() {
			ForEachGameObject((ego, sprites, velocity, line, position) => {
				bool anchored = line.IsAnchored;

				if (!anchored || Mathf.Abs(velocity.x) > 0.1f) {
					sprites.Body.localScale = new Vector3(Mathf.Sign(velocity.x), 1, 1);
				}

				Quaternion rotation = Quaternion.identity;
				if (anchored) {
					Vector2 direction = line.AnchorPoint - position.Value;
					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
					rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
				}

				sprites.Body.rotation = Quaternion.Lerp(
					sprites.Body.rotation,
					rotation,
					Time.deltaTime * 4
				);
			});
		}
	}
}
