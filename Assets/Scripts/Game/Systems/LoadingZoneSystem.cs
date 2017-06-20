using UnityEngine;
using UnityEngine.SceneManagement;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class LoadingZoneSystem : EgoSystem {
		public static void LoadLevel(int buildIndex) {
			SceneManager.LoadScene(buildIndex);
		}

		void Handle(TriggerEnter2DEvent e) {
			if (e.egoComponent2.HasComponents<LocalPlayer>()) {
				LoadingZone zone;
				if (e.egoComponent1.TryGetComponents(out zone)) {
					UIManager.Instance.Log("Completed level");
					LoadLevel(zone.NextScene.buildIndex);
				}
			}
		}

		public override void Start() {
      EgoEvents<TriggerEnter2DEvent>.AddHandler(Handle);
    }
	}
}
