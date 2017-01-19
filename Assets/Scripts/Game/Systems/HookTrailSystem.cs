using UnityEngine;

public class HookTrailSystem : EgoSystem<Hook, TrailRenderer> {
	public override void Update() {
		ForEachGameObject((ego, hook, trailRenderer) => {
			if (hook.Deployed != trailRenderer.enabled) {
				trailRenderer.enabled = hook.Deployed;
				if (hook.Deployed) {
					Debug.Log("Started!");
					trailRenderer.Clear();
				}
			}
		});
	}
}
