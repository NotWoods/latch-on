#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PrepLevelTools : ScriptableObject {
	public static GameObject AddGameManager() {
		GameObject gm = GameObject.Find("/GameManager");
		if (!gm) {
			gm = (GameObject) PrefabUtility.InstantiatePrefab(null);
			gm.name = "GameManager";
		}
		return gm;
	}

	public static GameObject AddUIManager() {
		GameObject ui = GameObject.Find("/UIManager");
		if (!ui) {
			Object prefab = null; // TODO
			ui = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
			ui.name = "UIManager";
		}
		return ui;
	}

	public static void AddContainers() {
		if (!GameObject.Find("/World")) {
			GameObject blank = new GameObject();
			blank.name = "World";
		}

		GameObject actors = GameObject.Find("/Actors");
		if (!actors) {
			actors = new GameObject();
			actors.name = "Actors";
		}
		GameManager.Instance.ActorContainer = actors.transform;

		GameObject props = GameObject.Find("/Props");
		if (!props) {
			props = new GameObject();
			props.name = "Props";
		}
		GameManager.Instance.PropsContainer = props.transform;
	}

	[MenuItem("Tools/Prep Level")]
	static void PrepLevel() {
		AddGameManager();
		AddUIManager();
		AddContainers();
	}
}
#endif
