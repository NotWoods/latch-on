using UnityEngine;
using UnityEngine.SceneManagement;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class LoadingZone : MonoBehaviour {
		[SerializeField]
		string NextSceneName = "";
		[SerializeField]
		int NextSceneBuildIndex = -1;

		public Scene NextScene {
			get {
				if (NextSceneName != "") {
					Debug.Log("Return via string");
					return SceneManager.GetSceneByName(NextSceneName);
				} else if (NextSceneBuildIndex >= 0) {
					Debug.Log("Return via index");
					return SceneManager.GetSceneByBuildIndex(NextSceneBuildIndex);
				} else {
					Debug.Log("Return via auto");
					Scene current = SceneManager.GetActiveScene();
					int next = current.buildIndex + 1;
					if (next > SceneManager.sceneCountInBuildSettings) next = 0;
					return SceneManager.GetSceneByBuildIndex(next);
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
