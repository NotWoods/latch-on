using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class InputSystem : EgoSystem<
		EgoConstraint<LocalPlayer, VJoystick, WorldPosition>
	> {
		private Vector2 getPointerDir(ControlType controlType, Vector2 playerPosition) {
			Vector2 result = Vector2.zero;
			Vector3? cursorScreenPoint = null;

			switch (controlType) {
				case ControlType.Touch:
					cursorScreenPoint = Input.GetTouch(0).position;
					goto case ControlType.Keyboard;

				case ControlType.Keyboard:
					Vector3 screenPoint = cursorScreenPoint.GetValueOrDefault(Input.mousePosition);
					screenPoint.z = Camera.main.transform.position.z * -1;

					Vector2 cursorPoint = Camera.main.ScreenToWorldPoint(screenPoint);
					result = cursorPoint - playerPosition;
					break;

				case ControlType.Joystick1:
				case ControlType.Joystick2:
				case ControlType.Joystick3:
				case ControlType.Joystick4:
				case ControlType.Joystick5:
				case ControlType.Joystick6:
				case ControlType.Joystick7:
				case ControlType.Joystick8:
					result.Set(Input.GetAxis("Pointer X"), Input.GetAxis("Pointer Y"));
					break;
			}

			if (result.sqrMagnitude > 1) result.Normalize();
			return result;
		}

		public override void Update() {
			if (PauseSystem.Paused) return;

			constraint.ForEachGameObject((ego, player, input, position) => {
				if (Input.GetButtonDown("Respawn")) input.ShouldRespawn = true;

				if (!GameManager.IsActive(ego)) return;

				input.XMoveAxis = Input.GetAxis("Horizontal");
				input.XMoveAxisRaw = Input.GetAxisRaw("Horizontal");

				if (Input.GetButtonDown("Jump")) input.JumpPressed = true;
				if (Input.GetButtonDown("Sink")) input.SinkPressed = true;

				input.LockRopeDown = Input.GetButton("Lock Rope");

				bool touchDown = Input.touchCount > 0;
				bool mouseDown = Input.GetButton("Grapple To Point");
				bool controllerDown = Input.GetButton("Grapple Using Pointer");

				input.HookDown = touchDown || mouseDown || controllerDown;
				if (touchDown) player.Controller = ControlType.Touch;
				else if (mouseDown) player.Controller = ControlType.Keyboard;
				else if (controllerDown) player.Controller = ControlType.Joystick1;
				input.AimAxis = getPointerDir(player.Controller, position.Value);
			});
		}
	}
}
