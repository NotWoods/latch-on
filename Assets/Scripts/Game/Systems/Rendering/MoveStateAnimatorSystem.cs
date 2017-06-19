using UnityEngine;
using LatchOn.ECS.Events;
using LatchOn.ECS.Components;
using System;

namespace LatchOn.ECS.Systems.Rendering {
	public class MoveStateAnimatorSystem : EgoSystem<ChildAnimator> {
		void Handle(MoveStateChangeEvent e) {
			ChildAnimator childAnimator = _bundles[e.egoComponent].component1;
			Animator animator = childAnimator.Animator;

			string typeName = Enum.GetName(typeof(MoveType), e.newState);
			animator.SetTrigger("MoveType." + typeName);
		}

		public override void Start() {
			EgoEvents<MoveStateChangeEvent>.AddHandler(Handle);
		}
	}
}
