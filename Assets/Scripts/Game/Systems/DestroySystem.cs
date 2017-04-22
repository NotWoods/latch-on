using LatchOn.ECS.Components.Health;
using LatchOn.ECS.Events;

namespace LatchOn.ECS.Systems {
	public class DestroySystem : EgoSystem<Destroyable> {
		public override void FixedUpdate() {
			ForEachGameObject((ego, destroyable) => {
				if (destroyable.CurrentHealth < 0) {
					GameManager.Instance.Destory(ego.gameObject);
					EgoEvents<EntityDestroyed>.AddEvent(new EntityDestroyed(ego));
				}
			});
		}
	}
}
