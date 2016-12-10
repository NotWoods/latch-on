using UnityEngine;

[DisallowMultipleComponent]
public class InputData : MonoBehaviour, IDataComponent {
	[Range(-1, 1)]
	public float HorizontalInput = 0;
	[Range(-1, 1)]
	public float HorizontalInputRaw = 0;

	public bool JumpPressed = false;
	public bool SinkPressed = false;

	public bool HookDown = false;
	public Vector2 PointerDir = new Vector2();

	public bool ShouldRespawn = false;

	public enum PointerMode { Mouse, Touch, Controller }
	public PointerMode Mode = PointerMode.Mouse;
}
