using UnityEngine;
using LatchOn.ECS.Events;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using System;

namespace LatchOn.ECS.Systems.Rendering {
	public class AnimatorStateSystem : EgoSystem<
		EgoParentConstraint<MoveState, Velocity, EgoConstraint<Animator>>
	> {
		string GetAnimationStateName(MoveType type, float xVelocity) {
			switch (type) {
				case MoveType.Walk:
					if (Mathf.Abs(xVelocity) < 1) return "Idle";
					else return "Moving";
				default:
					return Enum.GetName(typeof(MoveType), type);
			}
		}

		public override void Update() {
			constraint.ForEachGameObject((egoComponent, state, velocity, childConstraint) => {
				childConstraint.ForEachGameObject((childEgo, animator) => {
					animator.Play(GetAnimationStateName(state.Value, velocity.x));
				});
			});
		}
	}
}
