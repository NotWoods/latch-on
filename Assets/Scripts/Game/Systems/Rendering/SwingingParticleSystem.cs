using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Mover;

namespace LatchOn.ECS.Systems {
	/// Controls when particles are emitted
	public class SwingingParticleSystem : EgoSystem<ParticleSystem, MoveState, Velocity> {
		MoveType SwingOrDive = MoveType.Swing | MoveType.Flung | MoveType.Dive;

		public float MinSpeed = 10;

		public override void Update() {
			float minSpeedSqr = Mathf.Pow(MinSpeed, 2);

			ForEachGameObject((ego, particleSys, state, vel) => {
				bool validState = (state.Value & SwingOrDive) > 0;
				Vector2 velocity = vel.Value;
				if (validState && velocity.sqrMagnitude > minSpeedSqr) {
					if (!particleSys.isPlaying) particleSys.Play();
				} else if (particleSys.isPlaying) {
					particleSys.Stop();
				}
			});
		}
	}
}
