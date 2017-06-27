using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// Provides access to HUD animator paramaters through getters and setters
[RequireComponent(typeof(Animator))]
public class HUDWrapper : MonoBehaviour {
	Animator animator;

	public bool Visible {
		get { return animator.GetBool("Showing"); }
		set { animator.SetBool("Showing", value); }
	}
	public bool CollectableObtained {
		get { return animator.GetBool("Collectable obtained"); }
		set { animator.SetBool("Collectable obtained", value); }
	}
	public int Health {
		get { return animator.GetInteger("Health"); }
		set { animator.SetInteger("Health", value); }
	}

	void Awake() {
		animator = GetComponent<Animator>();
	}
}
