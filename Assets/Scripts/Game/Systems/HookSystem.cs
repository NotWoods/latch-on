using UnityEngine;

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<Transform, InputData, InspectableLineData, PlayerState> {
	public override void Update() {
		ForEachGameObject((ego, transform, input, line, state) => {
			if (!input.HookDown) {
				if (state.CurrentMode == PlayerState.Swing) {
					line.ClearPoints();
					line.MarkedSides.Clear();
					line.FreeLength = line.StartingLength;
					state.Set(PlayerState.Flung); // TODO: maybe only if velocity.x != 0?
				}
				return;
			}

			if (!line.IsAnchored()) {
				RaycastHit2D hit = Physics2D.Raycast(
					transform.position,
					input.PointerDir,
					line.StartingLength,
					line.NormalGround
				);

				if (!hit) return;
				Vector2 hitPoint = hit.point;
				line.SetAnchor(hitPoint);
				state.Set(PlayerState.Swing);
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
				line.MarkedSides.Push(line.Side(transform.position));
			}

			if (line.Count >= 2) {
				if (line.MarkedSides.Peek() != line.Side(transform.position)) {
					line.UnwrapLast();
					line.MarkedSides.Pop();
				}
			}
		});
	}
}
