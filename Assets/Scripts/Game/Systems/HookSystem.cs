using UnityEngine;

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<Transform, InputData, InspectableLineData> {
	public override void Update() {
		ForEachGameObject((ego, transform, input, line) => {
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

			float newLength = Vector2.Distance(transform.position, line.GetLast());
			newLength -= line.RetractSpeed * Time.deltaTime;
			line.FreeLength = Mathf.Clamp(newLength, 0, line.StartingLength);

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
