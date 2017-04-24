using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems {
	public class InputSystem : EgoSystem<LocalPlayer, VJoystick, WorldPosition> {
		private Vector2 getPointerDir(ControlType controlType, Vector2 playerPosition) {
			Vector2 result = Vector2.zero;
			Vector2? cursorScreenPoint = null;

			switch (controlType) {
				case ControlType.Touch:
					cursorScreenPoint = Input.GetTouch(0).position;
					goto case ControlType.Keyboard;

				case ControlType.Keyboard:
					cursorScreenPoint = cursorScreenPoint.HasValue
						? cursorScreenPoint
						: (Vector2) Input.mousePosition;
					Vector2 cursorPoint = Camera.main.ScreenToWorldPoint(cursorScreenPoint.Value);
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

			ForEachGameObject((ego, player, input, position) => {
				input.XMoveAxis = Input.GetAxis("Horizontal");
				input.XMoveAxisRaw = Input.GetAxisRaw("Horizontal");

				if (Input.GetButtonDown("Jump")) input.JumpPressed = true;
				if (Input.GetButtonDown("Sink")) input.SinkPressed = true;
				if (Input.GetButtonDown("Respawn")) input.ShouldRespawn = true;


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
