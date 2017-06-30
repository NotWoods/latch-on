using UnityEngine;
using LatchOn.ECS.Events;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;
using System;
using NextState = Tuple<string, float>;

namespace LatchOn.ECS.Systems.Rendering {
	public class AnimatorStateSystem : EgoSystem<
		EgoParentConstraint<MoveState, Velocity, EgoConstraint<Animator>>
	> {
		NextState GetAnimationState(EgoComponent ego, MoveType type, float xVelocity) {
			switch (type) {
				case MoveType.Walk:
					if (Mathf.Abs(xVelocity) < 0.1f) return new NextState("Idle", 0.1f);
					else return new NextState("Moving", 0);
				case MoveType.Fall:
					WallJumper wallJumper;
					if (ego.TryGetComponents(out wallJumper)) {
						if (wallJumper.AgainstSide != Side.None) {
							return new NextState("WallSlide", 0);
						}
					}
					return new NextState("Fall", 0);
				default:
					return new NextState(Enum.GetName(typeof(MoveType), type), 0.25f);
			}
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, state, velocity, childConstraint) => {
				if (!GameManager.IsActive(ego)) return;

				NextState next = GetAnimationState(ego, state.Value, velocity.x);
				string nextName = next.first;
				float fadeTime = next.second;

				childConstraint.ForEachGameObject((childEgo, animator) => {
					AnimatorStateInfo current = animator.IsInTransition(0)
						? animator.GetNextAnimatorStateInfo(0)
						: animator.GetCurrentAnimatorStateInfo(0);

					if (!current.IsName(nextName)) animator.CrossFade(nextName, fadeTime);
				});
			});
		}
	}
}
