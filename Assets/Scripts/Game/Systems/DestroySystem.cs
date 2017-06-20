using UnityEngine;
using LatchOn.ECS.Components.Health;

namespace LatchOn.ECS.Systems {
	public class DestroySystem : EgoSystem<EgoConstraint<Destroyable>> {
		private GameManager gameManager = GameManager.Instance;

		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, destroyable) => {
				if (destroyable.CurrentHealth < 0) {
					gameManager.DestroyedObjects.Enqueue(ego.gameObject);
					Ego.DestroyGameObject(ego);
					UIManager.Instance.Log(destroyable.DamageMessage);
				}
			});
		}
	}
}
