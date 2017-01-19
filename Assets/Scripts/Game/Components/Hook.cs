using UnityEngine;

/// Component for needle prop
[DisallowMultipleComponent]
public class Hook : MonoBehaviour {
	public float Speed = 50;
	public float HookLength = 1;
	public Vector3 StorageLocation = Vector3.back * 20;

	public Vector2 Target = new Vector2();
	public bool Deployed = false;
}
