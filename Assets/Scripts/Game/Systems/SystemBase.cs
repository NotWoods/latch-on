public class SystemBase {
	protected static EntityManager manager = EntityManager.Instance;

	protected static SystemBase instance;
	public static SystemBase Instance {
		get {
			if (instance == null) instance = new SystemBase();
			return instance;
		}
	}
}