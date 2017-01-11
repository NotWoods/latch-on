using UnityEngine;

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<WorldPosition, VJoystick, LineData, MoveState, Velocity, LinkedProps> {
	public float MinFlingSpeed = 0.1f;

	private void DisconnectLine(LineData line,
		MoveState state, Velocity velocity
	) {
		line.Clear();
		line.MarkedSides.Clear();
		line.FreeLength = line.StartingLength;
		if (velocity.x < -MinFlingSpeed || velocity.x > MinFlingSpeed)
			state.Value = MoveState.Flung;
		else
			state.Value = MoveState.Fall;
	}

	private void TryWrap(LineData line,
		Vector2 position, Velocity velocity
	) {
		RaycastHit2D shouldWrap = Physics2D.Linecast(
			position,
			line.WorldAnchor,
			line.NoHookGround
		);

		if (shouldWrap && line.WorldAnchor != shouldWrap.point) {
			line.Push(shouldWrap.point + velocity.Value.normalized * -0.1f);
			line.MarkedSides.Push(line.Side(position));
		}
	}

	private void TryUnwrap(LineData line, Vector2 position) {
		if (line.Count >= 2) {
			if (line.MarkedSides.Peek() != line.Side(position)) {
				line.Pop();
				line.MarkedSides.Pop();
			}
		}
	}

	public override void FixedUpdate() {
		ForEachGameObject((ego, position, input, line, state, velocity, links) => {
			if (!input.HookDown) {
				if (line.Anchored()) {
					DisconnectLine(line, state, velocity);
					// links.Needle.GiveTo(transform);
				}
				return;
			}

			if (!line.Anchored()) {
				RaycastHit2D hit = Physics2D.Raycast(
					position.Value, input.AimAxis,
					line.StartingLength, line.NormalGround
				);

				if (!hit) return;

				Vector2 needleLoop = links.Needle.ThrowTo(hit.point, input.AimAxis);
				line.WorldAnchor = needleLoop;
				state.Value = MoveState.Swing;
			}

			float newLength = Vector2.Distance(position.Value, line.WorldAnchor);
			newLength -= line.RetractSpeed * Time.deltaTime;
			line.FreeLength = Mathf.Clamp(newLength, 0.5f, line.StartingLength);

			TryWrap(line, position.Value, velocity);
			TryUnwrap(line, position.Value);
		});
	}
}
