using UnityEngine;
using System.Collections.Generic;

class MissingComponentException : System.Exception {}

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<WorldPosition, VJoystick, LineData, MoveState> {
	public float MinFlingSpeed = 0.1f;

	private void DisconnectLine(LineData line,
		MoveState state, float velocityX
	) {
		line.Clear();
		line.MarkedSides.Clear();
		line.FreeLength = line.StartingLength;
		if (velocityX < -MinFlingSpeed || velocityX > MinFlingSpeed)
			state.Value = MoveState.Flung;
		else
			state.Value = MoveState.Fall;
	}

	public override void FixedUpdate() {
		ForEachGameObject((ego, position, input, line, state) => {
			if (!input.HookDown) {
				if (line.Anchored()) {
					Velocity velocity;
					if (ego.TryGetComponents<Velocity>(out velocity)) {
						DisconnectLine(line, state, velocity.x);
					} else {
						DisconnectLine(line, state, MinFlingSpeed + 1);
					}

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

				NeedleHolder needleHolder;
				if (ego.TryGetComponents<NeedleHolder>(out needleHolder)) {
					Hook needle = GetHook(needleHolder);
					line.WorldAnchor = ThrowTo(needle.transform, hit.point, input.AimAxis);
				} else {
					line.WorldAnchor = hit.point;
				}

				state.Value = MoveState.Swing;
			}

			float newLength = Vector2.Distance(position.Value, line.WorldAnchor);
			newLength -= line.RetractSpeed * Time.deltaTime;
			line.FreeLength = Mathf.Clamp(newLength, 0.5f, line.StartingLength);
		});
	}

	private Dictionary<NeedleHolder, Hook> hookCache = new Dictionary<NeedleHolder, Hook>();
	private Hook GetHook(NeedleHolder holder) {
		if (hookCache.ContainsKey(holder)) return hookCache[holder];

		EgoComponent needleObject;
		if (holder.Needle == null) {
			needleObject = GameManager.Instance.NewEntity(holder.NeedlePrefab);
		} else {
			needleObject = holder.Needle.GetComponent<EgoComponent>();
		}

		Hook hook;
		if (needleObject.TryGetComponents<Hook>(out hook)) {
			hookCache[holder] = hook;
			return hook;
		} else {
			throw new MissingComponentException();
		}
	}

	private Transform GetLoop(Transform needleTransform) {
		return needleTransform.Find("Loop");
	}

	private Vector2 ThrowTo(Transform needle, Vector2 point, Vector2 direction) {
		needle.position = point;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
		needle.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		return GetLoop(needle).position;
	}
}
