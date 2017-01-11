using UnityEngine;

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<Transform, VJoystick, InspectableLineData, PlayerState, CharacterData, LinkedProps> {
	public float MinFlingSpeed = 0.1f;

	private void DisconnectLine(InspectableLineData line,
		PlayerState state, CharacterData stats
	) {
		line.ClearPoints();
		line.MarkedSides.Clear();
		line.FreeLength = line.StartingLength;
		if (stats.Velocity.x < -MinFlingSpeed || stats.Velocity.x > MinFlingSpeed)
			state.Set(PlayerState.Flung);
		else
			state.Set(PlayerState.Fall);
	}

	private void TryWrap(InspectableLineData line,
		Transform transform, CharacterData stats
	) {
		RaycastHit2D shouldWrap = Physics2D.Linecast(
			transform.position,
			line.GetLast(),
			line.NoHookGround
		);

		if (shouldWrap && line.GetLast() != shouldWrap.point) {
			line.WrapPoint(shouldWrap.point + stats.Velocity.normalized * -0.1f);
			line.MarkedSides.Push(line.Side(transform.position));
		}
	}

	private void TryUnwrap(InspectableLineData line, Transform transform) {
		if (line.Count >= 2) {
			if (line.MarkedSides.Peek() != line.Side(transform.position)) {
				line.UnwrapLast();
				line.MarkedSides.Pop();
			}
		}
	}

	public override void FixedUpdate() {
		ForEachGameObject((ego, transform, input, line, state, stats, links) => {
			if (!input.HookDown) {
				if (line.IsAnchored()) {
					DisconnectLine(line, state, stats);
					// links.Needle.GiveTo(transform);
				}
				return;
			}

			if (!line.IsAnchored()) {
				RaycastHit2D hit = Physics2D.Raycast(
					transform.position, input.AimAxis,
					line.StartingLength, line.NormalGround
				);

				if (!hit) return;

				Vector2 needleLoop = links.Needle.ThrowTo(hit.point, input.AimAxis);
				line.SetAnchor(needleLoop);
				state.Set(PlayerState.Swing);
			}

			float newLength = Vector2.Distance(transform.position, line.GetLast());
			newLength -= line.RetractSpeed * Time.deltaTime;
			line.FreeLength = Mathf.Clamp(newLength, 0.5f, line.StartingLength);

			TryWrap(line, transform, stats);
			TryUnwrap(line, transform);
		});
	}
}
