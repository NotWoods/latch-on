using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	/// Manages movement when swinging
	public class SwingingSystem : EgoSystem<LineData, WorldPosition, Velocity> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, line, position, vel) => {
				if (!line.Anchored()) return;
				Vector2 velocity = vel.Value;

				float damping;
				Damping damp;
				if (ego.TryGetComponents<Damping>(out damp)) {
					damping = damp.GetValue(MoveState.Swing);
				} else {
					damping = 1;
				}

				velocity.x = Mathf.MoveTowards(velocity.x, 0, Time.deltaTime * damping);

				Vector2 currentPosition = position.Value;
				Vector2 testPosition = currentPosition + (velocity * Time.deltaTime);
				Vector2 tetherPoint = line.WorldAnchor;

				if (Vector2.Distance(testPosition, tetherPoint) > line.FreeLength) {
					Vector2 direction = testPosition - tetherPoint;
					testPosition = tetherPoint + (direction.normalized * line.FreeLength);
					velocity = (testPosition - currentPosition) / Time.deltaTime;
				}

				vel.Value = velocity;
			});
		}
	}
}
