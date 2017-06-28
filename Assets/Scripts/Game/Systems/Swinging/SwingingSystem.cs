using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Swinging {
	/// Manages movement when swinging
	public class SwingingSystem : EgoSystem<
		EgoConstraint<LineData, WorldPosition, Velocity, Damping>
	> {
		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, line, position, vel, damp) => {
				// Only use swing system when line is active
				if (!line.IsAnchored) return;
				Vector2 velocity = vel.Value;

				float damping = damp.GetValue(MoveType.Swing);
				// Adjust velocity to dampened value
				velocity.x = Mathf.MoveTowards(velocity.x, 0, Time.deltaTime * damping);

				Vector2 currentPosition = position.Value;
				Vector2 tetherPoint = line.AnchorPoint;

				// Represents where the entity will be in the next frame
				Vector2 testPosition = currentPosition + (velocity * Time.deltaTime);

				// If the player will be past the current line length, adjust it so
				// the player is within the swinging radius.
				if (Vector2.Distance(testPosition, tetherPoint) > line.CurrentLength) {
					Vector2 direction = testPosition - tetherPoint;
					testPosition = tetherPoint + (direction.normalized * line.CurrentLength);

					// Alter the velocity to match the new location
					velocity = (testPosition - currentPosition) / Time.deltaTime;
				}

				vel.Value = velocity;
			});
		}
	}
}
