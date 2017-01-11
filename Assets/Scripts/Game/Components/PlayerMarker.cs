using UnityEngine;

/// Marks which of the 9 controllers this player is registered to.
public enum ControlType {
	Keyboard=-1,
	Joystick1=0,
	Joystick2=1,
	Joystick3=2,
	Joystick4=3,
	Joystick5=4,
	Joystick6=5,
	Joystick7=6,
	Joystick8=7
}

/// Marks a GameObject as a local player.
[DisallowMultipleComponent]
public class LocalPlayer : MonoBehaviour, IDataComponent {
	public ControlType Controller = ControlType.Keyboard;
}
