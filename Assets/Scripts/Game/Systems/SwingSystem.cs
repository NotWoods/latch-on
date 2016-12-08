using UnityEngine;
using Prime31;

public class SwingSystem : EgoSystem<Transform, CharacterData, InspectableLineData, CharacterController2D> {
	public override void Update() {
		ForEachGameObject((ego, transform, stats, line, controller) => {
			if (!line.IsAnchored()) return;

			Vector2 velocity = stats.Velocity;
			Vector2 testPosition = (Vector2) transform.position + (velocity * Time.deltaTime);
			Vector2 tetherPoint = line.GetLast();

			if (Vector2.Distance(testPosition, tetherPoint) > line.FreeLength) {
				testPosition = line.GetLast() +
					((testPosition - line.GetLast()).normalized * line.FreeLength);
				velocity = (testPosition - (Vector2) transform.position) / Time.deltaTime;
			}

			velocity.y += stats.Gravity * Time.deltaTime;

			controller.move(velocity * Time.deltaTime);
			stats.Velocity = controller.velocity;
		});
	}
}
