using UnityEngine;

[DisallowMultipleComponent]
public class CharacterData : MonoBehaviour {
	public float RunSpeed = 8f;
	public float JumpHeight = 3f;

	public float GravityBase = -25f;
	[Range(0, 5)]
	public float GravityScale = 1;

	public float MaxFallSpeed = 15;
}
