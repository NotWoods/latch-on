using UnityEngine;

public class RotatorSystem : EgoSystem<Transform, InspectableLineData> {
	public override void Update() {
		ForEachGameObject((ego, transform, line) => {
			Quaternion rotation;
			if (line.IsAnchored()) {
				Vector2 direction = line.GetLast() - (Vector2) transform.position;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
			} else {
				rotation = Quaternion.identity;
			}

			transform.rotation = rotation;
		});
	}
}
