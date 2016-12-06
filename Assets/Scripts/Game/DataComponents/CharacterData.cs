using UnityEngine;

[DisallowMultipleComponent]
public class CharacterData : MonoBehaviour, IDataComponent {
	public float Gravity = -25f;
	public float RunSpeed = 8f;
	public float GroundDamping = 20f;
	public float InAirDamping = 5f;
	public float JumpHeight = 3f;
	public Vector2 Velocity = new Vector2();
}
