using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	/// Manages movement when swinging
	public class SwingingSystem : EgoSystem<LineData, WorldPosition, Velocity> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, line, position, vel) => {
				if (!line.IsAnchored) return;
				Vector2 velocity = vel.Value;

				float damping;
				Damping damp;
				if (ego.TryGetComponents<Damping>(out damp)) {
					damping = damp.GetValue(MoveType.Swing);
				} else {
					damping = 1;
				}

				velocity.x = Mathf.MoveTowards(velocity.x, 0, Time.deltaTime * damping);

				Vector2 currentPosition = position.Value;
				Vector2 testPosition = currentPosition + (velocity * Time.deltaTime);
				Vector2 tetherPoint = line.AnchorPoint;

				if (Vector2.Distance(testPosition, tetherPoint) > line.CurrentLength) {
					Vector2 direction = testPosition - tetherPoint;
					testPosition = tetherPoint + (direction.normalized * line.CurrentLength);
					velocity = (testPosition - currentPosition) / Time.deltaTime;
				}

				vel.Value = velocity;
			});
		}
	}
}
