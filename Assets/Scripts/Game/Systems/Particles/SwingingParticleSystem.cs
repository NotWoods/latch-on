using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems.Rendering {
	/// Controls when particles are emitted
	public class SwingingParticleSystem : EgoSystem<
		EgoConstraint<ParticleSystem, MoveState, Velocity>
	> {
		public float MinSpeed = 10;

		public override void Update() {
			constraint.ForEachGameObject((ego, particleSys, state, velocity) => {
				if (state.IsType(MoveType.Swing | MoveType.Flung | MoveType.Dive)
				&& velocity.Value.sqrMagnitude > Mathf.Pow(MinSpeed, 2)) {
					if (!particleSys.isPlaying) {
						particleSys.Play();
					}
				} else if (particleSys.isPlaying) {
					particleSys.Stop();
				}
			});
		}
	}
}
