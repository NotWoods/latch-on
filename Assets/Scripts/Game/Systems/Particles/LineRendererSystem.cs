using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class LineRendererSystem : EgoSystem<
		EgoConstraint<WorldPosition, LineData, WrappingLine, LineRenderer>
	> {
		public static Vector3[] BuildPoints(
			Vector2 playerPos,
			LineData line,
			WrappingLine wrap = null
		) {
			int totalPoints = 2;
			if (wrap) totalPoints += wrap.WrappedItems.Count;

			Vector3[] points = new Vector3[totalPoints];

			int i = 0;
			if (wrap) {
				foreach (WrappingLine.Entry item in wrap.WrappedItems) {
					points[i] = item.point;
					i++;
				}
			}

			points[i] = line.AnchorPoint;
			points[i + 1] = playerPos;

			return points;
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, position, line, wrap, renderer) => {
				if (!line.IsAnchored) {
					renderer.positionCount = 0;
					return;
				}

				Vector3[] points = BuildPoints(position.Value, line, wrap);

				renderer.positionCount = points.Length;
				renderer.SetPositions(points);
			});
		}
	}
}
