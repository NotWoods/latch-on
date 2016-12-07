using UnityEngine;
using Prime31;

public class HookSystem : EgoSystem<Transform, CharacterData, InputData, LineData, CharacterController2D> {
	public override void Update() {
		ForEachGameObject((ego, transform, stats, input, line, controller) => {
			if (!input.HookDown) {
				line.ClearPoints();
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


		});
	}
}
