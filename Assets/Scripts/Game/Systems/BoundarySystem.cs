using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Health;

namespace LatchOn.ECS.Systems {
	public class BoundarySystem : EgoSystem<
		EgoConstraint<WorldPosition, Destroyable>
	> {
		Bounds levelBounds;

		public override void Start() {
			levelBounds = GameManager.Instance.GetComponent<LevelBounds>().Bounds;
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, position, destroyable) => {
				if (!levelBounds.Contains(position.Value)) {
					destroyable.CurrentHealth = -1;
					destroyable.DamageMessage = "Fell out of bounds";
				}
			});
		}
	}
}
