using UnityEngine;

[DisallowMultipleComponent]
public class CharacterData : MonoBehaviour, IDataComponent {
	public float RunSpeed = 8f;
	public float JumpHeight = 3f;

	public float GroundDamping = 20f;
	public float InAirDamping = 5f;
	public float SwingDamping = 0.5f;

	public float GravityBase = -25f;
	[Range(0, 5)]
	public float GravityScale = 1;

	[HideInInspector]
	public Vector2 Velocity = new Vector2();
}
