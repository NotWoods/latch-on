using UnityEngine;

/// Marks a GameObject as a local player.
[DisallowMultipleComponent]
public class LocalPlayer : MonoBehaviour {
	public ControlType Controller = ControlType.Keyboard;

	public bool IsJoystick() {
		int joystickNum = (int) Controller;
		return joystickNum > 0 && joystickNum <= 8;
	}
}

/// Marks which of the 9 controllers this player is registered to.
public enum ControlType {
	Keyboard=0,
	Touch=9,
	Joystick1=1,
	Joystick2=2,
	Joystick3=3,
	Joystick4=4,
	Joystick5=5,
	Joystick6=6,
	Joystick7=7,
	Joystick8=8
}
