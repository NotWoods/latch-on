using UnityEngine;
using LatchOn.ECS.Events;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Mover;
using System;

namespace LatchOn.ECS.Systems.Rendering {
	public class AnimatorParametersSystem : EgoSystem<
		EgoParentConstraint<MoveState, VJoystick, WallJumper, EgoConstraint<Animator>>
	> {
		void HandleStateChange(MoveStateChangeEvent e) {
			constraint.ForEachGameObject((ego, s, i, w, childConstraint) => {
				if (!GameManager.IsActive(ego) || ego != e.egoComponent) return;
				childConstraint.ForEachGameObject((childEgo, animator) => {
					foreach (MoveType moveType in Enum.GetValues(typeof(MoveType))) {
						string triggerName = "MoveType."
							+ Enum.GetName(typeof(MoveType), moveType);

						if (moveType == e.newState) {
							animator.SetTrigger(triggerName);
						} else {
							animator.ResetTrigger(triggerName);
						}
					}
				});
			});
		}

		public override void Start() {
			EgoEvents<MoveStateChangeEvent>.AddHandler(HandleStateChange);
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, s, input, wallJumper, childConstraint) => {
				if (!GameManager.IsActive(ego)) return;

				float xInput = Mathf.Abs(input.XMoveAxis);
				Side side = wallJumper.AgainstSide;

				childConstraint.ForEachGameObject((childEgo, animator) => {
					animator.SetFloat("Speed", xInput);
					animator.SetInteger("Agaisnt Side", (int) side);
				});
			});
		}
	}
}
