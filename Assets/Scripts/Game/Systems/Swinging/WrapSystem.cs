using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Swinging {
	/// Manages rope attachment and wrapping
	public class WrapSystem : EgoSystem<
		EgoConstraint<LineData, WrappingLine, WorldPosition>
	> {
		private static void TryWrap(
			LineData line, WrappingLine wrap,
			Vector2 position
		) {
			RaycastHit2D shouldWrap = Physics2D.Linecast(position,
				line.AnchorPoint, wrap.ShouldWrap);

			if (shouldWrap && line.AnchorPoint != shouldWrap.point) {
				Bounds hitBounds = shouldWrap.collider.bounds;

				Vector2 lastAnchor = line.AnchorPoint;
				Vector2 newAnchor = shouldWrap.point;

				Vector2 awayFromHit = newAnchor - (Vector2) hitBounds.center;
				awayFromHit.Normalize();
				while (shouldWrap.collider.OverlapPoint(newAnchor)) {
					newAnchor += awayFromHit * 0.1f;
				}

				Side entityRelativeToLine = (Side) ExtraMath.SideOfLine(position,
					lastAnchor, newAnchor);

				line.AnchorPoint = newAnchor;
				wrap.Push(lastAnchor, entityRelativeToLine);
			}
		}

		private static void TryUnwrap(LineData line, WrappingLine wrap, Vector2 position) {
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
			constraint.ForEachGameObject((ego, line, wrap, position) => {
				if (line.IsAnchored) {
					TryWrap(line, wrap, position.Value);
					TryUnwrap(line, wrap, position.Value);
				} else {
					wrap.Clear();
				}
			});
		}
	}
}
