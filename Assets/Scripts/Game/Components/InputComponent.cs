using UnityEngine;

public class InputComponent : IComponent {
	public float HorizontalInput = 0;

	public bool JumpPressed = false;
	public bool SinkPressed = false;

	public bool HookDown = false;
	public Vector2 PointerDir = new Vector2();
}
