using UnityEngine;

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<Transform, VJoystick, LineData, PlayerState, Velocity, LinkedProps> {
	public float MinFlingSpeed = 0.1f;

	private void DisconnectLine(LineData line,
		PlayerState state, Velocity velocity
	) {
		line.ClearPoints();
		line.MarkedSides.Clear();
		line.FreeLength = line.StartingLength;
		if (velocity.x < -MinFlingSpeed || velocity.x > MinFlingSpeed)
			state.Set(PlayerState.Flung);
		else
			state.Set(PlayerState.Fall);
	}

	private void TryWrap(LineData line,
		Transform transform, Velocity velocity
	) {
		RaycastHit2D shouldWrap = Physics2D.Linecast(
			transform.position,
			line.GetLast(),
			line.NoHookGround
		);

		if (shouldWrap && line.GetLast() != shouldWrap.point) {
			line.WrapPoint(shouldWrap.point + velocity.Value.normalized * -0.1f);
			line.MarkedSides.Push(line.Side(transform.position));
		}
	}

	private void TryUnwrap(LineData line, Transform transform) {
		if (line.Count >= 2) {
			if (line.MarkedSides.Peek() != line.Side(transform.position)) {
				line.UnwrapLast();
				line.MarkedSides.Pop();
			}
		}
	}

	public override void FixedUpdate() {
		ForEachGameObject((ego, transform, input, line, state, velocity, links) => {
			if (!input.HookDown) {
				if (line.IsAnchored()) {
					DisconnectLine(line, state, velocity);
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

			TryWrap(line, transform, velocity);
			TryUnwrap(line, transform);
		});
	}
}
