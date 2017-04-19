using UnityEngine;

namespace LatchOn.ECS.Components.Input {
	/// Virtual joystick data, set by the player or some AI
	[DisallowMultipleComponent]
	public class VJoystick : MonoBehaviour {
		[Range(-1, 1)]
		public float XMoveAxis = 0;
		[Range(-1, 1)]
		public float XMoveAxisRaw = 0;

		[SerializeField, Range(-1, 1)]
		float xAimAxis = 0;
		[SerializeField, Range(-1, 1)]
		float yAimAxis = 0;

		/// Normalized vector representing the aiming direction
		public Vector2 AimAxis {
			get { return new Vector2(xAimAxis, yAimAxis); }
			set { xAimAxis = value.x; yAimAxis = value.y; }
		}

		/// True if jump button was pressed, should be unset once handled
		public bool JumpPressed = false;
		/// True if sink button was pressed, should be unset once handled
		public bool SinkPressed = false;
		/// True if hook button is held down
		public bool HookDown = false;
		/// True if respawn button was pressed
		public bool ShouldRespawn = false;
		/// Sets all `_____Pressed` properties to false
		public void ClearPressed() {
			JumpPressed = SinkPressed = false;
		}
	}
}
