using UnityEngine;
using LatchOn.ECS.Events;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Mover;
using System;

namespace LatchOn.ECS.Systems.Rendering {
	public class MoveStateAnimatorSystem : EgoSystem<
		EgoParentConstraint<MoveState, EgoConstraint<Animator>>
	> {
		void Handle(MoveStateChangeEvent e) {
			constraint.ForEachGameObject((egoComponent, state, childConstraint) => {
				if (egoComponent != e.egoComponent) return;
				childConstraint.ForEachGameObject((childEgo, animator) => {
					string typeName = Enum.GetName(typeof(MoveType), e.newState);
					animator.SetTrigger("MoveType." + typeName);
				});
			});
		}

		public override void Start() {
			EgoEvents<MoveStateChangeEvent>.AddHandler(Handle);
		}
	}
}
