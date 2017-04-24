using UnityEngine;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems.Rendering {
	public class HookTrailSystem : EgoSystem<Hook, TrailRenderer> {
		public override void Update() {
			ForEachGameObject((ego, hook, trailRenderer) => {
				if (hook.Deployed != trailRenderer.enabled) {
					trailRenderer.enabled = hook.Deployed;
					if (hook.Deployed)
						trailRenderer.Clear();
				}
			});
		}
	}
}
