using UnityEngine;
using UnityEngine.SceneManagement;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class LoadingZone : MonoBehaviour {
		[SerializeField]
		int _nextSceneBuildIndex = -1;

		public int NextSceneBuildIndex {
			get {
				if (_nextSceneBuildIndex >= 0) {
					return _nextSceneBuildIndex;
				} else {
					Scene current = SceneManager.GetActiveScene();
					int next = current.buildIndex + 1;
					if (next >= SceneManager.sceneCountInBuildSettings) next = 0;
					return next;
				}
			}
		}
	}
}
