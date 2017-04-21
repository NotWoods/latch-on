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

			Vector3[] points = new Vector3[wrapper.Count + 1];
			int i = 0;
			foreach (var entry in wrapper) {
				points[i] = entry.point;
				i++;
			}

			renderer.positionCount = points.Length;
			renderer.SetPositions(points);
		});
	}
}
