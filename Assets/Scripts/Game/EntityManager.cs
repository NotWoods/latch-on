using System;
using System.Collections.Generic;

public class EntityManager {
	IEntity this[int index] => Entities[index];
  private Dictionary<int, IEntity> Entities;
  private Dictionary<Type, Dictionary<int, IComponent>> Components;

	private EntityManager() {
		Entities = new Dictionary<int, IEntity>();
		Components = new Dictionary<Type, Dictionary<int, IComponent>>()
	}

	protected static EntityManager instance = new EntityManager();
	public static EntityManager Instance { get { return instance; } }

	private static int IDIncrementor = 0;
  public IEntity CreateEntity() {
		int id = IDIncrementor++;
		IEntity entity = null; // TODO

		Entities.Add(id, entity);
		return entity;
	}

  public bool DestroyEntity(int entityId) {
		// TODO cleanup
		return Entities.Remove(entityId);
	}


  public C CreateComponent<C>(int entityId) where C : IComponent, new() {
		C component = new C();
		Type type = typeof (C);
		bool containerExists = Components.ContainsKey(type);

		Dictionary<int, IComponent> entityContainer;
		if (containerExists) { entityContainer = Components[type]; }
		else { entityContainer = new Dictionary<int, IComponent>(); }

		entityContainer.Add(entityId, component);
		if (!containerExists) Components.Add(type, entityContainer);

		return component;
	}

  public bool RemoveComponent<C>(int entityId) where C : IComponent {
		try {
			Dictionary<int, IComponent> entityContainer = Components[typeof (C)];
			return entityContainer.Remove(entityId);
		} catch (KeyNotFoundException e) {
			return false;
		}
	}

  public C GetComponent<C>(int entityId) where C : IComponent {
		Dictionary<int, IComponent> entityContainer = Components[typeof (C)];
		return (C) entityContainer[entityId];
	}

  public IEnumerable<IComponent> GetComponents(int entityId) {
		foreach (Dictionary<int, IComponent> container in Components.Values) {
			if (container.ContainsKey(entityId)) yield return container[entityId];
		}
	}
}
