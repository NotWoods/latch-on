using UnityEngine;
using System.Collections.Generic;

/// Interface to mark a class as an entity.
public abstract class Entity : MonoBehaviour, IEntity {
	public int ID { get; protected set; }

	private static int IDIncrementor = 0;
	// Assign a unique ID. Should be called in Awake().
	protected void AssignID() {
		ID = IDIncrementor++;
	}

	protected Dictionary<System.Type, IComponent> Components;

	public virtual C CreateComponent<C>() where C : IComponent, new() {
		C component = new C();
		Components.Add(typeof (C), component);
		return component;
	}

	public virtual bool DestroyComponent<C>() where C : IComponent {
		return Components.Remove(typeof (C));
	}

	public virtual C GetComponentOfType<C>() where C : IComponent {
		return (C) Components[typeof (C)];
	}

	public virtual IEnumerable<IComponent> GetComponents() {
		return Components.Values;
	}
}
