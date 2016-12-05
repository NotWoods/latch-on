/// Generic Singleton class
public class Singleton<T> where T : new() {
	protected static T instance;

	/// Returns the instance of this singleton.
	public static T Instance {
		get {
			if (instance == null) instance = new T();
			return instance;
		}
	}
}