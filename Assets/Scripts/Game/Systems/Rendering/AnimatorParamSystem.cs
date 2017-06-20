using UnityEngine;
using LatchOn.ECS.Events;
using LatchOn.ECS.Components.Base;
using System;

namespace LatchOn.ECS.Systems.Rendering {
	public class AnimatorParamSystem : EgoSystem<
		EgoParentConstraint<Velocity, EgoConstraint<Animator>>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, velocity, childConstraint) => {
				childConstraint.ForEachGameObject((childEgo, animator) => {
					animator.SetFloat("Speed", Mathf.Abs(velocity.x));
				});
			});
		}
	}
}
