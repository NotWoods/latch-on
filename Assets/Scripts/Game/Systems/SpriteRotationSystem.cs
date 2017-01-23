using UnityEngine;

public class SpriteRotationSystem : EgoSystem<PlayerSprites, Velocity> {
	public override void Update() {
		ForEachGameObject((ego, sprites, velocity) => {
			LineData line;
			bool hasLine = ego.TryGetComponents<LineData>(out line);
			bool anchored = hasLine && line.Anchored();

			if (!anchored || Mathf.Abs(velocity.x) > 0.1f) {
				sprites.Body.localScale = new Vector3(Mathf.Sign(velocity.x), 1, 1);
			}

			WorldPosition position;
			if (hasLine && ego.TryGetComponents<WorldPosition>(out position)) {
				Quaternion rotation = Quaternion.identity;
				if (anchored) {
					Vector2 direction = line.WorldAnchor - position.Value;
					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
					rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
				}

				sprites.Body.rotation = Quaternion.Lerp(
					sprites.Body.rotation,
					rotation,
					Time.deltaTime * 4
				);
			}
		});
	}
}
