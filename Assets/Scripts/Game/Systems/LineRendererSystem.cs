using UnityEngine;

public class LineRendererSystem : EgoSystem<Transform, InspectableLineData, LineRenderer> {
	public override void Update() {
		ForEachGameObject((ego, transform, line, renderer) => {
			if (!line.IsAnchored()) {
				renderer.enabled = false;
				return;
			}

			renderer.enabled = true;

			Vector3[] points = new Vector3[line.Count + 1];
			int i = 0;
			foreach (Vector2 p in line.Points()) { points[i] = p; i++; }
			points[i] = transform.position;

			renderer.numPositions = points.Length;
			renderer.SetPositions(points);
		});
	}
}
