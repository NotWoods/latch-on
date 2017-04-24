using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	/// Manages rope attachment and wrapping
	public class WrapSystem : EgoSystem<LineData, WrappingLine, WorldPosition, Velocity> {
		void TryWrap(LineData line, WrappingLine wrapper, Vector2 position, Vector2 velocity) {
			var anchorPoint = line.AnchorPoint;
			RaycastHit2D shouldWrap = Physics2D.Linecast(position, anchorPoint,
				wrapper.ShouldWrap);

			if (shouldWrap && anchorPoint != shouldWrap.point) {
				Vector2 newAnchor = shouldWrap.point + velocity.normalized * -0.1f;

				line.AnchorPoint = newAnchor;
				wrapper.Push(newAnchor, position);
			}
		}

		void TryUnwrap(LineData line, WrappingLine wrapper, Vector2 position) {
			int count = wrapper.Count;
			bool canUnwrap = count >= 2;
			if (!canUnwrap) return;

			var topEntry = wrapper.Peek();
			bool playerOnOtherSideOfLine = topEntry.side != wrapper.SideOfLine(position);
			if (playerOnOtherSideOfLine) {
				wrapper.Pop();
				line.AnchorPoint = wrapper.Peek().point;
			}
		}

		public override void FixedUpdate() {
			ForEachGameObject((ego, line, wrapper, position, velocity) => {
				if (line.IsAnchored) {
					TryWrap(line, wrapper, position.Value, velocity.Value);
					TryUnwrap(line, wrapper, position.Value);
				}
			});
		}
	}
}
