using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

public class LineRendererSystem : EgoSystem<WorldPosition, LineData, WrappingLine, LineRenderer> {
	public override void Update() {
		ForEachGameObject((ego, position, line, wrapper, renderer) => {
			if (!line.IsAnchored) {
				renderer.positionCount = 0;
				return;
			}

			Vector3[] points = new Vector3[wrapper.Count + 2];
			int i = 0;
			foreach (var entry in wrapper) {
				points[i] = entry.point;
				i++;
			}

			points[i] = line.AnchorPoint;
			points[i + 1] = position.Value;

			renderer.positionCount = points.Length;
			renderer.SetPositions(points);
		});
	}
}
