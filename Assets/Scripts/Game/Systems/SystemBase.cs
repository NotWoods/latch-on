/// Generic system class
public class SystemBase<T> : Singleton<T> where T : new() {
	protected static EntityManager Manager = EntityManager.Instance;
}