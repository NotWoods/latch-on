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
			Debug.Log(levelBounds);
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, position, destroyable) => {
				if (!levelBounds.Contains(position.Value)) {
					destroyable.CurrentHealth = -1;
					UIManager.Instance.Log("Fell out of bounds");
				}
			});
		}
	}
}
