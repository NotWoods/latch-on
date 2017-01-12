using UnityEngine;

/// Controls when particles are emitted
public class SwingingParticleSystem : EgoSystem<ParticleSystem, MoveState, Velocity> {
	public float MinSpeed = 10;

	public override void Update() {
		ForEachGameObject((ego, particleSys, state, velocity) => {
			if (state.Any(MoveState.Swing, MoveState.Flung)
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
