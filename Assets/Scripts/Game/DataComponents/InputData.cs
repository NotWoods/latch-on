using UnityEngine;

[DisallowMultipleComponent]
public class InputData : MonoBehaviour, IDataComponent {
	[Range(-1, 1)]
	public float HorizontalInput = 0;

	public bool JumpPressed = false;
	public bool SinkPressed = false;

	public bool HookDown = false;
	public Vector2 PointerDir = new Vector2();
}