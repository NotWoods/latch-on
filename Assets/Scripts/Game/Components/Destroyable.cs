using UnityEngine;

[DisallowMultipleComponent]
public class Destroyable : MonoBehaviour {
	public int MaxHealth = 3;
	public bool ShouldRespawn = true;

	internal int Health = 3;
}
