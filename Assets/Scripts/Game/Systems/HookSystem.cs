using UnityEngine;
using System.Collections.Generic;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Rope;
using HookBundle = EgoBundle<
	LatchOn.ECS.Components.Rope.Hook,
	UnityEngine.Transform,
	LatchOn.ECS.Components.Base.Speed
>;

class MissingComponentException : System.Exception {}

namespace LatchOn.ECS.Systems {
	/// Manages rope attachment and wrapping
	public class HookSystem : EgoSystem<
		WorldPosition, VJoystick, LineData, MoveState, CanGrapple
	> {
		public float MinFlingSpeed = 0.1f;
		public Vector3 StorageLocation = Vector3.back * 20;

		public override void FixedUpdate() {
			ForEachGameObject((ego, pos, input, line, state, grappler) => {
				HookBundle bundle = GetHook(grappler);
				Vector2 position = pos.Value;

				bool isSwinging = line.IsAnchored;
				bool buttonHeld = input.HookDown;
				bool didThrow = grappler.DidThrow;

				if (isSwinging) {
					if (buttonHeld) KeepSwinging(line, position);
					else {
						StopSwinging(line, grappler, state, ego);
						RetractHook(bundle);
					}
				} else if (didThrow) {
					if (TargetReached(bundle)) StartSwinging(line, state, grappler, position);
					else if (PathInterupted(bundle.component1, position, grappler)) {
						CancelThrow(grappler);
						RetractHook(bundle);
					}
					else KeepThrowing(bundle);
				} else if (buttonHeld) {
					Vector2 newTarget;
					if (PathExists(position, input, grappler, out newTarget)) {
						StartThrow(grappler, newTarget, position);
					}
				}
			});
		}

		private Dictionary<CanGrapple, HookBundle> cache = new Dictionary<CanGrapple, HookBundle>();
		private HookBundle GetHook(CanGrapple holder) {
			if (cache.ContainsKey(holder)) return cache[holder];

			EgoComponent hookObject = holder.Hook;
			if (holder.Hook == null) {
				var gm = GameManager.Instance;
				hookObject = gm.NewEntity(gm.HookPrefab);
				if (gm.PropsContainer) hookObject.transform.parent = gm.PropsContainer;
				holder.Hook = hookObject;
			}

			var hook = hookObject.GetComponent<Hook>();
			var transform = hookObject.GetComponent<Transform>();
			var speed = hookObject.GetComponent<Speed>();

			if (!hook || !transform || !speed) {
				throw new System.Exception("Hook missing component");
			}

			var bundle = new HookBundle(hookObject, hook, transform, speed);

			RetractHook(bundle);
			cache[holder] = bundle;
			return bundle;
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
			line.IsAnchored = false;
			line.CurrentLength = grappler.StartingLength;

			Velocity velocity;
			state.Value = MoveType.Flung;
			if (egoComponent.TryGetComponents<Velocity>(out velocity)) {
				bool smallXSpeed = Mathf.Abs(velocity.x) <= MinFlingSpeed;
				if (smallXSpeed) state.Value = MoveType.Fall;
			}
		}

		/// Check if the target has been reached by the hook
		private bool TargetReached(HookBundle hookBundle) {
			Hook hook = hookBundle.component1;
			Transform hookTransform = hookBundle.component2;

			return hook.Target == (Vector2) hookTransform.position;
		}

		private void StartSwinging(
			LineData line, MoveState state, CanGrapple grappler,
			Vector2 playerPosition
		) {
			Hook hook = GetHook(grappler).component1;

			state.Value = MoveType.Swing;
			line.AnchorPoint = hook.CalculatePinHead();
			line.IsAnchored = true;
			grappler.DidThrow = false;
			line.CurrentLength = Vector2.Distance(playerPosition, line.AnchorPoint);
		}

		private bool PathInterupted(Hook hook, Vector2 position, CanGrapple grappler) {
			Vector2 loopPoint = hook.CalculatePinHead();

			return Physics2D.Linecast(loopPoint, position, grappler.Solids);
			// || Vector2.Distance(loopPoint, position) > grappler.StartingLength;
		}

		private void CancelThrow(CanGrapple grappler) {
			grappler.DidThrow = false;
		}

		private void RetractHook(HookBundle hook) {
			hook.component2.position = StorageLocation;
			hook.component1.Deployed = false;
		}

		private void KeepThrowing(HookBundle hook) {
			Transform hookTransform = hook.component2;
			float hookSpeed = hook.component3.Value;
			Vector2 target = hook.component1.Target;

			hookTransform.position = Vector2.MoveTowards(
				hookTransform.position,
				target,
				hookSpeed * Time.deltaTime
			);
		}

		private bool PathExists(
			Vector2 position, VJoystick input, CanGrapple grappler,
			out Vector2 newTarget
		) {
			RaycastHit2D hit = Physics2D.Raycast(
				position, input.AimAxis,
				grappler.StartingLength, grappler.ShouldGrapple
			);

			newTarget = hit ? hit.point : Vector2.zero;
			return hit;
		}

		private void StartThrow(CanGrapple grappler, Vector2 target, Vector2 start) {
			HookBundle bundle = GetHook(grappler);
			Hook hook = bundle.component1;
			Transform transform = bundle.component2;

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
