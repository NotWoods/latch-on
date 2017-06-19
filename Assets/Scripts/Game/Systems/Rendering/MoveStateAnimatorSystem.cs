using UnityEngine;
using LatchOn.ECS.Events;
using LatchOn.ECS.Components;
using System;

namespace LatchOn.ECS.Systems.Rendering {
	public class MoveStateAnimatorSystem : EgoSystem<
		EgoConstraint<ChildAnimator>
	> {
		void Handle(MoveStateChangeEvent e) {
			var bundle = constraint.GetLookup(constraint.rootBundles)[e.egoComponent] as EgoBundle<ChildAnimator>;
			ChildAnimator childAnimator = bundle.component1;
			Animator animator = childAnimator.Animator;

			string typeName = Enum.GetName(typeof(MoveType), e.newState);
			animator.SetTrigger("MoveType." + typeName);
		}

		public override void Start() {
			EgoEvents<MoveStateChangeEvent>.AddHandler(Handle);
		}
	}
}
