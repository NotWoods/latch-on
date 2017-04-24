using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	/// Manages rope attachment and wrapping
	public class WrapSystem : EgoSystem<LineData, WrappingLine, WorldPosition> {
		private void TryWrap(
			LineData line, WrappingLine wrap,
			Vector2 position, Vector2 velocity
		) {
			RaycastHit2D shouldWrap = Physics2D.Linecast(position,
				line.AnchorPoint, wrap.ShouldWrap);

			if (shouldWrap && line.AnchorPoint != shouldWrap.point) {
				Vector2 lastAnchor = line.AnchorPoint;
				Vector2 newAnchor = shouldWrap.point + velocity.normalized * -0.1f;

				Side entityRelativeToLine = (Side) ExtraMath.SideOfLine(position,
					lastAnchor, newAnchor);

				line.AnchorPoint = newAnchor;
				wrap.Push(lastAnchor, entityRelativeToLine);
			}
		}

		private void TryUnwrap(LineData line, WrappingLine wrap, Vector2 position) {
			if (wrap.WrappedItems.Count == 0) return;

			Vector2 lastWrappedPoint;
			Side lastSideOfLine;
			wrap.Peek(out lastWrappedPoint, out lastSideOfLine);

			Side currentSideOfLine = (Side) ExtraMath.SideOfLine(position,
				lastWrappedPoint, line.AnchorPoint);

			if (lastSideOfLine != currentSideOfLine) {
				wrap.Pop();
				line.CurrentLength += Vector2.Distance(line.AnchorPoint, lastWrappedPoint);
				line.AnchorPoint = lastWrappedPoint;
			}
		}

		public override void FixedUpdate() {
			ForEachGameObject((ego, line, wrap, position) => {
				if (line.IsAnchored) {
					Velocity velocity;
					Vector2 velocityValue;
					if (ego.TryGetComponents<Velocity>(out velocity)) {
						velocityValue = velocity.Value;
					} else {
						velocityValue = Vector2.zero;
					}

					TryWrap(line, wrap, position.Value, velocityValue);
					TryUnwrap(line, wrap, position.Value);
				} else {
					wrap.Clear();
				}
			});
		}
	}
}
