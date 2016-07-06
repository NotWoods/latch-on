using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T sInstance;
	public static T Instance {
		get {
			if (sInstance == null) {
				sInstance = (T) FindObjectOfType( typeof(T) );

				if (sInstance == null) {
					Debug.LogError("An instance of " + typeof(T) + 
					" is needed in the scene, but there is none.");
				}
			}
			return sInstance;
		}
	}
}