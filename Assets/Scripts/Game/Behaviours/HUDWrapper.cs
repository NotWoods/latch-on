using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class HUDWrapper : MonoBehaviour {
	Animator animator;

	public bool Visible = false;
	public bool CollectableObtained = false;
	public int Health = 2;

	void Awake() {
		animator = GetComponent<Animator>();
	}

	void Update() {
		animator.SetBool("Showing", Visible);
		animator.SetBool("Collectable obtained", CollectableObtained);
		animator.SetInteger("Health", Health);
	}
}
