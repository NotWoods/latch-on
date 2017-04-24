using UnityEngine;

namespace LatchOn.ECS.Components.Input {
	/// Marks a GameObject as a local player.
	[DisallowMultipleComponent]
	public class LocalPlayer : MonoBehaviour {
		public int ControllerNumber = -1;
		public bool UseTouch = false;
		public bool UsePointer = true;
		public bool UseKeyboard = true;


		public ControlType Controller {
			get {
				if (UseTouch) return ControlType.Touch;
				else if (UseKeyboard) return ControlType.Keyboard;
				else return (ControlType) ControllerNumber;
			}
			set {
				switch (value) {
					case ControlType.Keyboard:
						ControllerNumber = -1;
						UseTouch = false;
						UsePointer = true;
						UseKeyboard = true;
						break;
					case ControlType.Touch:
						ControllerNumber = -1;
						UseTouch = true;
						UsePointer = false;
						UseKeyboard = true;
						break;
					default:
						ControllerNumber = (int) value;
						UseKeyboard = false;
						break;
				}
			}
		}

		public bool IsJoystick() {
			return !UseKeyboard;
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
}
