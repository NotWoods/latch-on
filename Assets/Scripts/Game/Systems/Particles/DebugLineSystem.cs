using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class DebugLineSystem : EgoSystem<
		EgoConstraint<WorldPosition, LineData>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, position, line) => {
				if (!line.IsAnchored) { return; }

				WrappingLine wrap = null;
				ego.TryGetComponents(out wrap);

				Vector3[] points = LineRendererSystem.BuildPoints(position.Value, line, wrap);

				for (int i = 1; i < points.Length; i++) {
					Debug.DrawLine(points[i], points[i-1]);
				}
			});
		}
	}
}
