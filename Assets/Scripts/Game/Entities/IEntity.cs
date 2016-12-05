using System.Collections.Generic;

/// Interface to mark a class as an entity.
public interface IEntity {
	int ID { get; }

	C CreateComponent<C>() where C : IComponent, new();
	bool DestroyComponent<C>() where C : IComponent;
	C GetComponentOfType<C>() where C : IComponent;
	IEnumerable<IComponent> GetComponents();
}
