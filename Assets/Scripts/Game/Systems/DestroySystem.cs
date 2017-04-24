using UnityEngine;
using LatchOn.ECS.Components.Health;

namespace LatchOn.ECS.Systems {
	public class DestroySystem : EgoSystem<Destroyable> {
		private GameManager gameManager = GameManager.Instance;

		public override void FixedUpdate() {
			ForEachGameObject((ego, destroyable) => {
				if (destroyable.Health < 0) {
					GameObject go = ego.gameObject;
					gameManager.Destory(go);
					if (destroyable.ShouldRespawn) gameManager.DestroyedObjects.Enqueue(go);
				}
			});
		}
	}
}
