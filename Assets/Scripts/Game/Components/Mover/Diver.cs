using UnityEngine;

[DisallowMultipleComponent]
public class Diver : MonoBehaviour {
	public float MinYVelocity = -5;
	public float MaxYVelocity = 5;

	public Vector2 DivingVelocity = new Vector2(11, -11);
}
