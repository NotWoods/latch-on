using UnityEngine;
using System.Collections.Generic;

class MissingComponentException : System.Exception {}

/// Manages rope attachment and wrapping
public class HookSystem : EgoSystem<WorldPosition, VJoystick, LineData, MoveState, NeedleHolder> {
	public float MinFlingSpeed = 0.1f;

	public override void FixedUpdate() {
		ForEachGameObject((ego, pos, input, line, state, needleHolder) => {
			Hook hook = GetHook(needleHolder);
			Vector2 position = pos.Value;

			bool isSwinging = line.Anchored();
			bool buttonHeld = input.HookDown;
			bool didThrow = needleHolder.DidThrow;

			if (isSwinging) {
				if (buttonHeld) KeepSwinging(line, position);
				else {
					StopSwinging(line, state, ego);
					RetractHook(hook);
				}
			} else if (didThrow) {
				if (TargetReached(hook)) StartSwinging(line, state, needleHolder, position);
				else if (PathInterupted(hook, position, line)) {
					CancelThrow(needleHolder);
					RetractHook(hook);
				}
				else KeepThrowing(hook);
			} else if (buttonHeld) {
				Vector2 newTarget;
				if (PathExists(position, input, line, out newTarget)) {
					StartThrow(needleHolder, newTarget, position);
				}
			}
		});
	}

	private Dictionary<NeedleHolder, Hook> hookCache = new Dictionary<NeedleHolder, Hook>();
	private Hook GetHook(NeedleHolder holder) {
		if (hookCache.ContainsKey(holder)) return hookCache[holder];

		EgoComponent needleObject;
		if (holder.Needle == null) {
			var gm = GameManager.Instance;
			needleObject = gm.NewEntity(gm.HookPrefab);
			holder.Needle = needleObject.gameObject;
		} else {
			needleObject = holder.Needle.GetComponent<EgoComponent>();
		}

		Hook hook;
		if (needleObject.TryGetComponents<Hook>(out hook)) {
			needleObject.transform.position = hook.StorageLocation;
			hookCache[holder] = hook;
			return hook;
		} else {
			throw new MissingComponentException();
		}
	}

	/// Step the swinging loop
	private void KeepSwinging(LineData line, Vector2 position) {
		float newLength = Vector2.Distance(position, line.WorldAnchor);
		newLength -= line.RetractSpeed * Time.deltaTime;

		if (newLength < 0.5f) newLength = 0.5f;
		line.FreeLength = newLength;
	}

	/// Cancel the swing
	private void StopSwinging(LineData line, MoveState state, EgoComponent egoComponent) {
		line.Clear();
		line.MarkedSides.Clear();
		line.FreeLength = line.StartingLength;

		bool smallXSpeed = false;
		Velocity velocity;
		if (egoComponent.TryGetComponents<Velocity>(out velocity)) {
			smallXSpeed = Mathf.Abs(velocity.x) <= MinFlingSpeed;
		}

		state.Value = smallXSpeed ? MoveState.Fall : MoveState.Flung;
	}

	/// Check if the target has been reached by the hook
	private bool TargetReached(Hook hook) {
		WorldPosition needlePos = hook.GetComponent<WorldPosition>();

		return hook.Target == needlePos.Value;
	}

	private void StartSwinging(LineData line, MoveState state, NeedleHolder needleHolder, Vector2 playerPosition) {
		Hook hook = GetHook(needleHolder);

		state.Value = MoveState.Swing;
		line.WorldAnchor = hook.CalculatePinHead();
		needleHolder.DidThrow = false;
		line.FreeLength = Vector2.Distance(playerPosition, line.WorldAnchor);
	}

	private bool PathInterupted(Hook hook, Vector2 position, LineData line) {
		Vector2 loopPoint = hook.CalculatePinHead();

		return Physics2D.Linecast(loopPoint, position, line.NoHookGround);
		// || Vector2.Distance(loopPoint, position) > line.StartingLength;
	}

	private void CancelThrow(NeedleHolder needleHolder) {
		needleHolder.DidThrow = false;
	}

	private void RetractHook(Hook hook) {
		hook.transform.position = hook.StorageLocation;
		hook.Deployed = false;
	}

	private void KeepThrowing(Hook hook) {
		Transform hookTransform = hook.transform;

		hookTransform.position = Vector2.MoveTowards(
			hookTransform.position,
			hook.Target,
			hook.Speed * Time.deltaTime
		);
	}

	private bool PathExists(Vector2 position, VJoystick input, LineData line, out Vector2 newTarget) {
		RaycastHit2D hit = Physics2D.Raycast(
			position, input.AimAxis,
			line.StartingLength, line.NormalGround
		);

		newTarget = hit ? hit.point : Vector2.zero;
		return hit;
	}

	private void StartThrow(NeedleHolder needleHolder, Vector2 target, Vector2 start) {
		Hook hook = GetHook(needleHolder);
		Transform transform = hook.transform;

		Vector2 direction = target - start;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		Vector2 shift = (direction.normalized * hook.HookLength * -1);
		transform.position = start - shift;

		needleHolder.DidThrow = true;
		hook.Target = target;
		hook.Deployed = true;
	}
}
