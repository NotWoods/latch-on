using UnityEngine;
using System.Collections.Generic;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Rope;

class MissingComponentException : System.Exception {}

namespace LatchOn.ECS.Systems {
	/// Manages rope attachment and wrapping
	public class HookSystem : EgoSystem<WorldPosition, VJoystick, LineData, MoveState,  CanGrapple> {
		public float MinFlingSpeed = 0.1f;
		public Vector3 StorageLocation = Vector3.back * 20;

		public override void FixedUpdate() {
			ForEachGameObject((ego, pos, input, line, state, grappler) => {
				Hook hook = GetHook(grappler);
				Vector2 position = pos.Value;

				bool isSwinging = line.Anchored();
				bool buttonHeld = input.HookDown;
				bool didThrow = grappler.DidThrow;

				if (isSwinging) {
					if (buttonHeld) KeepSwinging(line, position);
					else {
						StopSwinging(line, grappler, state, ego);
						RetractHook(hook);
					}
				} else if (didThrow) {
					if (TargetReached(hook)) StartSwinging(line, state, grappler, position);
					else if (PathInterupted(hook, position, line, grappler)) {
						CancelThrow(grappler);
						RetractHook(hook);
					}
					else KeepThrowing(hook);
				} else if (buttonHeld) {
					Vector2 newTarget;
					if (PathExists(position, input, grappler, out newTarget)) {
						StartThrow(grappler, newTarget, position);
					}
				}
			});
		}

		private Dictionary<CanGrapple, Hook> hookCache = new Dictionary<CanGrapple, Hook>();
		private Hook GetHook(CanGrapple holder) {
			if (hookCache.ContainsKey(holder)) return hookCache[holder];

			EgoComponent needleObject;
			if (holder.Hook == null) {
				var gm = GameManager.Instance;
				needleObject = gm.NewEntity(gm.HookPrefab);
				holder.Hook = needleObject;
			} else {
				needleObject = holder.Hook.GetComponent<EgoComponent>();
			}

			Hook hook;
			if (needleObject.TryGetComponents<Hook>(out hook)) {
				needleObject.transform.position = StorageLocation;
				hookCache[holder] = hook;
				return hook;
			} else {
				throw new MissingComponentException();
			}
		}

		/// Step the swinging loop
		private void KeepSwinging(LineData line, Vector2 position) {
			float newLength = Vector2.Distance(position, line.AnchorPoint);
			newLength -= line.RetractSpeed * Time.deltaTime;

			if (newLength < 0.5f) newLength = 0.5f;
			line.CurrentLength = newLength;
		}

		/// Cancel the swing
		private void StopSwinging(LineData line, CanGrapple grappler, MoveState state, EgoComponent egoComponent) {
			line.Clear();
			line.MarkedSides.Clear();
			line.CurrentLength = grappler.StartingLength;

			bool smallXSpeed = false;
			Velocity velocity;
			if (egoComponent.TryGetComponents<Velocity>(out velocity)) {
				smallXSpeed = Mathf.Abs(velocity.x) <= MinFlingSpeed;
			}

			state.Value = smallXSpeed ? MoveType.Fall : MoveType.Flung;
		}

		/// Check if the target has been reached by the hook
		private bool TargetReached(Hook hook) {
			WorldPosition needlePos = hook.GetComponent<WorldPosition>();

			return hook.Target == needlePos.Value;
		}

		private void StartSwinging(LineData line, MoveState state, CanGrapple grappler, Vector2 playerPosition) {
			Hook hook = GetHook(grappler);

			state.Value = MoveType.Swing;
			line.AnchorPoint = hook.CalculatePinHead();
			grappler.DidThrow = false;
			line.CurrentLength = Vector2.Distance(playerPosition, line.AnchorPoint);
		}

		private bool PathInterupted(Hook hook, Vector2 position, LineData line, CanGrapple grappler) {
			Vector2 loopPoint = hook.CalculatePinHead();

			return Physics2D.Linecast(loopPoint, position, grappler.Solids);
			// || Vector2.Distance(loopPoint, position) > line.StartingLength;
		}

		private void CancelThrow(CanGrapple grappler) {
			grappler.DidThrow = false;
		}

		private void RetractHook(Hook hook) {
			hook.transform.position = StorageLocation;
			hook.Deployed = false;
		}

		private void KeepThrowing(Hook hook) {
			Transform hookTransform = hook.transform;
			float hookSpeed = hook.GetComponent<Speed>().Value;

			hookTransform.position = Vector2.MoveTowards(
				hookTransform.position,
				hook.Target,
				hookSpeed * Time.deltaTime
			);
		}

		private bool PathExists(Vector2 position, VJoystick input, CanGrapple grappler, out Vector2 newTarget) {
			RaycastHit2D hit = Physics2D.Raycast(
				position, input.AimAxis,
				grappler.StartingLength, grappler.ShouldGrapple
			);

			newTarget = hit ? hit.point : Vector2.zero;
			return hit;
		}

		private void StartThrow(CanGrapple grappler, Vector2 target, Vector2 start) {
			Hook hook = GetHook(grappler);
			Transform transform = hook.transform;

			Vector2 direction = target - start;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			Vector2 shift = (direction.normalized * hook.Length * -1);
			transform.position = start - shift;

			grappler.DidThrow = true;
			hook.Target = target;
			hook.Deployed = true;
		}
	}
}
