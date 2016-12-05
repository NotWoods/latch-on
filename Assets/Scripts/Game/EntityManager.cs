using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager> {
  private Dictionary<int, IEntity> Entities = new Dictionary<int, IEntity>();
  private Dictionary<Type, Dictionary<int, IComponent>> Components = new Dictionary<Type, Dictionary<int, IComponent>>();

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
		} catch (KeyNotFoundException) {
			return false;
		}
	}

  public C GetComponent<C>(int entityId) where C : IComponent {
		Dictionary<int, IComponent> entityContainer = Components[typeof (C)];
		return (C) entityContainer[entityId];
	}

	public T GetUnityComponent<T>(int entityId) where T : Component {
		IEntity entity = Entities[entityId];
		if (entity is MonoBehaviour) {
			MonoBehaviour entityBehaviour = (MonoBehaviour) entity;
			return entityBehaviour.GetComponent<T>();
		} else {
			throw new InvalidOperationException("Entity is not a GameObject");
		}
	}

  public IEnumerable<IComponent> GetComponents(int entityId) {
		foreach (Dictionary<int, IComponent> container in Components.Values) {
			if (container.ContainsKey(entityId)) yield return container[entityId];
		}
	}
}
