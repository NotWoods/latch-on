using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	/// Manages rope attachment and wrapping
	public class WrapSystem : EgoSystem<LineData, WorldPosition> {
		private void TryWrap(LineData line, Vector2 position, Vector2 velocity) {
			RaycastHit2D shouldWrap = Physics2D.Linecast(
				position,
				line.WorldAnchor,
				line.NoHookGround
			);

			if (shouldWrap && line.WorldAnchor != shouldWrap.point) {
				line.Push(shouldWrap.point + velocity.normalized * -0.1f);
				line.MarkedSides.Push(line.Side(position));
			}
		}

		private void TryUnwrap(LineData line, Vector2 position) {
			if (line.Count >= 2) {
				if (line.MarkedSides.Peek() != line.Side(position)) {
					line.Pop();
					line.MarkedSides.Pop();
				}
			}
		}

		public override void FixedUpdate() {
			ForEachGameObject((ego, line, position) => {
				if (line.Anchored()) {
					Velocity velocity;
					Vector2 velocityValue;
					if (ego.TryGetComponents<Velocity>(out velocity)) {
						velocityValue = velocity.Value;
					} else {
						velocityValue = Vector2.zero;
					}

					TryWrap(line, position.Value, velocityValue);
					TryUnwrap(line, position.Value);
				}
			});
		}
	}
}
