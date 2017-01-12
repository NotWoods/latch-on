using UnityEngine;

[DisallowMultipleComponent]
public class CharacterData : MonoBehaviour {
	public float RunSpeed = 8f;
	public float JumpHeight = 3f;

	public float Gravity = -25f;

	public float MaxFallSpeed = 15;
}
