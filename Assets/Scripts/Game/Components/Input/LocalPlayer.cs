using UnityEngine;

namespace LatchOn.ECS.Components.Input {
	/// Marks a GameObject as a local player.
	[DisallowMultipleComponent]
	public class LocalPlayer : MonoBehaviour {
		public int ControllerNumber = -1;
		public bool UseTouch = false;
		public bool UsePointer = true;
		public bool UseKeyboard = true;
	}
}
