using UnityEngine;

/// Singleton MonoBehaviour
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T instance;

	/// Returns the instance of this singleton behaviour.
	public static T Instance {
		get {
			if (instance == null) {
				instance = (T) FindObjectOfType(typeof (T));
        if (instance == null) {
					GameObject blank = new GameObject();
					blank.name = typeof (T).ToString();
					instance = blank.AddComponent<T>();
				}
			}

			return instance;
		}
	}
}
