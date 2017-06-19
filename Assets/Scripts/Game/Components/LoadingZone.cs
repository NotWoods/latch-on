using UnityEngine;
using UnityEngine.SceneManagement;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class LoadingZone : MonoBehaviour {
		[SerializeField]
		string NextSceneName;
		[SerializeField]
		int NextSceneBuildIndex;

		public Scene NextScene {
			get {
				if (NextSceneName != "") {
					return SceneManager.GetSceneByName(NextSceneName);
				} else {
					return SceneManager.GetSceneByBuildIndex(NextSceneBuildIndex);
				}
			}
		}

		[ContextMenu("Set Missing Values")]
		void SetMissingValue() {
			var next = NextScene;
			NextSceneName = next.name;
			NextSceneBuildIndex = next.buildIndex;
		}
	}
}
