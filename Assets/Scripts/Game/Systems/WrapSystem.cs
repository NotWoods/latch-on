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
				wrap.WrappedPoints.Add(line.AnchorPoint);
				line.AnchorPoint = shouldWrap.point + velocity.normalized * -0.1f;

				Side entityRelativeToLine = (Side) ExtraMath.SideOfLine(position,
					wrap.WrappedPoints[wrap.WrappedPoints.Count - 1], line.AnchorPoint);
				wrap.MarkedSides.Add(entityRelativeToLine);
			}
		}

		private void TryUnwrap(LineData line, WrappingLine wrap, Vector2 position) {
			if (wrap.WrappedPoints.Count == 0) return;

			int topPointIndex = wrap.WrappedPoints.Count - 1;
			Vector2 lastWrappedPoint = wrap.WrappedPoints[topPointIndex];

			Side lastSideOfLine = wrap.MarkedSides[wrap.MarkedSides.Count - 1];
			Side currentSideOfLine = (Side) ExtraMath.SideOfLine(position,
				lastWrappedPoint, line.AnchorPoint);

			if (lastSideOfLine != currentSideOfLine) {
				wrap.MarkedSides.RemoveAt(wrap.MarkedSides.Count - 1);
				line.CurrentLength += Vector2.Distance(line.AnchorPoint, lastWrappedPoint);
				wrap.WrappedPoints.RemoveAt(topPointIndex);
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
					wrap.WrappedPoints.Clear();
					wrap.MarkedSides.Clear();
				}
			});
		}
	}
}
