using UnityEngine;

/// Virtual joystick data
[DisallowMultipleComponent]
public class VJoystick : MonoBehaviour {
	[Range(-1, 1)]
	public float XMoveAxis = 0;
	[Range(-1, 1)]
	public float XMoveAxisRaw = 0;

	[Range(-1, 1)]
	public float XAimAxis = 0;
	[Range(-1, 1)]
	public float YAimAxis = 0;
	public Vector2 AimAxis {
		get { return new Vector2(XAimAxis, YAimAxis); }
		set { XAimAxis = value.x; YAimAxis = value.y; }
	}

	public bool JumpPressed = false;
	public bool SinkPressed = false;

	public bool HookDown = false;

	public bool ShouldRespawn = false;

	public enum PointerMode { Mouse, Touch, Controller }
	public PointerMode Mode = PointerMode.Mouse;

	public void ClearPressed() {
		JumpPressed = SinkPressed = false;
	}
}
