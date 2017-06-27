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
		/// True if lock rope button is held down
		public bool LockRopeDown = false;
		// TODO: move property to another component
		public bool ShouldRespawn = false;

		/// Seconds spent standing still
		public float IdleTime = 0;

		/// Sets all `_____Pressed` properties to false
		[ContextMenu("Clear Pressed")]
		public void ClearPressed() {
			JumpPressed = SinkPressed = false;
		}

		public bool NoInput() {
			return XMoveAxis == 0 && !JumpPressed	&& !SinkPressed && !HookDown;
		}
	}
}
