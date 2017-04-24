using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class LineRendererSystem : EgoSystem<WorldPosition, LineData, WrappingLine, LineRenderer> {
		public override void Update() {
			ForEachGameObject((ego, position, line, wrap, renderer) => {
				if (!line.IsAnchored) {
					renderer.positionCount = 0;
					return;
				}

				int totalPoints = 2 + wrap.WrappedItems.Count;
				Vector3[] points = new Vector3[totalPoints];

				int i = 0;
				foreach (WrappingLine.Entry item in wrap.WrappedItems) {
					points[i] = item.point;
					i++;
				}

				points[i] = line.AnchorPoint;
				points[i + 1] = position.Value;

				renderer.positionCount = points.Length;
				renderer.SetPositions(points);
			});
		}
	}
}
