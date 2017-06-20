using UnityEngine;
using UnityEngine.SceneManagement;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Health;

namespace LatchOn.ECS.Systems {
	public class TouchDamageSystem : EgoSystem<
		EgoConstraint<Destroyable>
	> {
		void Handle(TriggerEnter2DEvent e) {
			TouchDamager damager;
			if (e.egoComponent1.TryGetComponents(out damager)) {
				constraint.ForEachGameObject((egoComponent, destroyable) => {
					if (egoComponent != e.egoComponent2) return;

					destroyable.CurrentHealth -= damager.Damage;
					destroyable.DamageMessage = damager.Message;
				});
			}
		}

		public override void Start() {
      EgoEvents<TriggerEnter2DEvent>.AddHandler(Handle);
    }
	}
}
