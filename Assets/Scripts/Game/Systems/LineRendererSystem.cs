using UnityEngine;

public class LineRendererSystem : EgoSystem<WorldPosition, LineData, LineRenderer> {
	public override void Update() {
		ForEachGameObject((ego, position, line, renderer) => {
			if (!line.Anchored()) {
				renderer.numPositions = 0;
				return;
			}

			Vector3[] points = new Vector3[line.Count + 1];
			int i = 0;
			foreach (Vector2 p in line.Points()) { points[i] = p; i++; }
			points[i] = position.Value;

			renderer.numPositions = points.Length;
			renderer.SetPositions(points);
		});
	}
}
