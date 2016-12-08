using UnityEngine;
using Prime31;

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<Transform, InputData, InspectableLineData, CharacterController2D> {
	/// To be seen as a new point, the distance between two vectors must be greater than this number.
	public float VectorEpsilon = 0.01f;

	public override void Update() {
		ForEachGameObject((ego, transform, input, line, controller) => {
			if (!input.HookDown) {
				line.ClearPoints();
				line.FreeLength = line.StartingLength;
				return;
			}

			if (!line.IsAnchored()) {
				RaycastHit2D hit = Physics2D.Raycast(
					transform.position,
					input.PointerDir,
					line.StartingLength,
					line.NormalGround
				);

				if (hit) {
					Vector2 hitPoint = hit.point;
					line.SetAnchor(hitPoint);
				} else {
					return;
				}
			}

			line.FreeLength = Vector2.Distance(transform.position, line.GetLast());

			RaycastHit2D shouldWrap = Physics2D.Linecast(
				transform.position,
				line.GetLast(),
				line.NoHookGround
			);
			if (shouldWrap && line.GetLast() != shouldWrap.point) {
				line.WrapPoint(shouldWrap.point);
			}
		});
	}
}
