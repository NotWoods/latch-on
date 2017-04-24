using UnityEngine;
using System.Collections.Generic;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Events;

namespace LatchOn.ECS.Systems {
	/// Manages rope attachment and wrapping
	public class HookSystem : EgoSystem<
		WorldPosition, VJoystick, LineData, MoveState, CanGrapple
	> {
		public float MinFlingSpeed = 0.1f;
		public Vector2 StorageLocation = Vector3.back * 40;

		class HookBundle: EgoBundle<Hook, Transform, Speed> {
			public HookBundle(EgoComponent ego, Hook h, Transform t, Speed s):
				base(ego, h, t, s) {}

			public Hook hook { get { return component1; } }
			public Transform transform { get { return component2; } }
			public Speed speed { get { return component3; } }
		}

		public override void FixedUpdate() {
			ForEachGameObject((ego, pos, input, line, state, hookRef) => {
				HookBundle bundle = GetHook(hookRef);
				Vector2 position = pos.Value;

				bool isSwinging = line.IsAnchored;
				bool buttonHeld = input.HookDown;
				bool didThrow = hookRef.DidThrow;

				if (isSwinging) {
					if (buttonHeld) KeepSwinging(line, position);
					else {
						StopSwinging(line, state, ego);
						RetractHook(bundle);
					}
				} else if (didThrow) {
					if (TargetReached(bundle)) {
						StartSwinging(line, state, hookRef, position, bundle.hook, ego);
					} else if (PathInterupted(bundle.hook, position, line, hookRef)) {
						CancelThrow(hookRef);
						RetractHook(bundle);
					}
					else KeepThrowing(bundle);
				} else if (buttonHeld) {
					Vector2 newTarget;
					if (PathExists(position, input, line, hookRef, out newTarget)) {
						StartThrow(hookRef, newTarget, position);
					}
				}
			});
		}

		Dictionary<CanGrapple, HookBundle> cache = new Dictionary<CanGrapple, HookBundle>();
		HookBundle GetHook(CanGrapple hookRef) {
			if (cache.ContainsKey(hookRef)) return cache[hookRef];

			EgoComponent hookObject = hookRef.Hook;
			if (hookObject == null) {
				hookObject = GameManager.Instance.NewEntity(GameManager.Instance.prefabs.Hook);
				hookRef.Hook = hookObject;
			}

			Hook hook = hookObject.GetComponent<Hook>();
			Transform transform = hookObject.GetComponent<Transform>();
			Speed speed = hookObject.GetComponent<Speed>();

			if (!hook || !transform || !speed) {
				throw new System.Exception("Hook missing component");
			}

			var bundle = new HookBundle(hookObject, hook, transform, speed);

			RetractHook(bundle);
			cache[hookRef] = bundle;
			return bundle;
		}

		/// Step the swinging loop
		void KeepSwinging(LineData line, Vector2 position) {
			float newLength = Vector2.Distance(position, line.AnchorPoint);
			newLength -= line.RetractSpeed * Time.deltaTime;

			if (newLength < 0.5f) newLength = 0.5f;
			line.CurrentLength = newLength;
		}

		/// Cancel the swing
		void StopSwinging(LineData line, MoveState state, EgoComponent egoComponent) {
			line.IsAnchored = false;
			EgoEvents<LineDisconnected>.AddEvent(new LineDisconnected(egoComponent));

			Velocity velocity;
			state.Value = MoveType.Flung;
			if (egoComponent.TryGetComponents<Velocity>(out velocity)) {
				bool smallXSpeed = Mathf.Abs(velocity.x) <= MinFlingSpeed;
				if (smallXSpeed) state.Value = MoveType.Fall;
			}
		}

		/// Check if the target has been reached by the hook
		bool TargetReached(HookBundle hookBundle) {
			Hook hook = hookBundle.component1;
			Transform hookTransform = hookBundle.component2;

			return hook.Target == (Vector2) hookTransform.position;
		}

		void StartSwinging(
			LineData line, MoveState state, CanGrapple hookRef,
			Vector2 playerPosition, Hook hook, EgoComponent ego
		) {
			Vector2 anchor = hook.CalculatePinHead();

			state.Value = MoveType.Swing;
			hookRef.DidThrow = false;
			line.AnchorPoint = anchor;
			line.IsAnchored = true;
			line.CurrentLength = Vector2.Distance(playerPosition, anchor);

			EgoEvents<LineConnected>.AddEvent(new LineConnected(ego, line.AnchorPoint));
		}

		bool PathInterupted(Hook hook, Vector2 position, LineData line, CanGrapple grappler) {
			Vector2 loopPoint = hook.CalculatePinHead();

			return Physics2D.Linecast(loopPoint, position, grappler.Solids);
			// || Vector2.Distance(loopPoint, position) > line.StartingLength;
		}

		void CancelThrow(CanGrapple hookRef) {
			hookRef.DidThrow = false;
		}

		void RetractHook(HookBundle hookBundle) {
			hookBundle.transform.position = StorageLocation;
			hookBundle.hook.Deployed = false;
		}

		void KeepThrowing(HookBundle bundle) {
			Transform hookTransform = bundle.transform;

			hookTransform.position = Vector2.MoveTowards(
				hookTransform.position,
				bundle.hook.Target,
				bundle.speed.Value * Time.deltaTime
			);
		}

		bool PathExists(
			Vector2 position, VJoystick input, LineData line, CanGrapple grappler,
			out Vector2 newTarget
		) {
			RaycastHit2D hit = Physics2D.Raycast(
				position, input.AimAxis,
				grappler.StartingLength, grappler.ShouldGrapple
			);

			newTarget = hit ? hit.point : Vector2.zero;
			return hit;
		}

		private void StartThrow(CanGrapple hookRef, Vector2 target, Vector2 start) {
			HookBundle bundle = GetHook(hookRef);
			Hook hook = bundle.hook;
			Transform transform = bundle.transform;

			Vector2 direction = target - start;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			Vector2 shift = (direction.normalized * -hook.Length);
			transform.position = start - shift;

			hookRef.DidThrow = true;
			hook.Target = target;
			hook.Deployed = true;
		}
	}
}
