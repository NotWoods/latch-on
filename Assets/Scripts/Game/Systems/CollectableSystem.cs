using UnityEngine;
using UnityEngine.SceneManagement;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class CollectableSystem : EgoSystem {
		public static void LoadLevel(int buildIndex) {
			SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
		}

		void Handle(TriggerEnter2DEvent e) {
			Collected pocket;
			Collectable collectable;
			if (e.egoComponent2.TryGetComponents(out pocket)
			&& e.egoComponent1.TryGetComponents(out collectable)) {
				if (!collectable.BeenCollected) {
					pocket.CollectedItems.Add(collectable);
					collectable.BeenCollected = true;
					collectable.gameObject.SetActive(false);
				}
			}
		}

		public override void Start() {
      EgoEvents<TriggerEnter2DEvent>.AddHandler(Handle);
    }
	}
}
