using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class LinkedProps : MonoBehaviour {
	private Dictionary<GameObject, GameObject> instances = new Dictionary<GameObject, GameObject>();
	private delegate void NewInstanceDelegate(GameObject newInstance);
	private GameObject localInstance(GameObject prefab, NewInstanceDelegate callback) {
		if (instances.ContainsKey(prefab)) return instances[prefab];
		GameObject result = Instantiate(prefab);
		callback(result);
		instances.Add(prefab, result);
		return result;
	}
	private GameObject localInstance(GameObject prefab) {
		return localInstance(prefab, (p) => {});
	}



	public GameObject CursorPrefab;
	public GameObject Cursor {
		get {
			return localInstance(CursorPrefab, (cursor) => {
				cursor.transform.SetParent(UIManager.Canvas);
			});
		}
	}

	public GameObject NeedlePrefab;
	public Needle Needle {
		get {
			return localInstance(NeedlePrefab, (needle) => {
				needle.transform.SetParent(GameManager.Instance.PropsContainer);
			}).GetComponent<Needle>();
		}
	}
}
