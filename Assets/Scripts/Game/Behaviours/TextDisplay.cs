using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Animator))]
public class TextDisplay : MonoBehaviour {
	private string enterState = "Enter Reveal";

	private Text text;
	private Animator animator;

	void Awake() {
		text = GetComponent<Text>();
		animator = GetComponent<Animator>();
	}

	public void Log(string message) {
		text.text = message;
		animator.Play(enterState);
	}
}
