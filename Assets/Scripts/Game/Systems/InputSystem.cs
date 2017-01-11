using UnityEngine;

public class InputSystem : EgoSystem<LocalPlayer, VJoystick, WorldPosition> {
	private Vector2 getPointerDir(VJoystick input, Vector2 playerPosition) {
		Vector2 result = Vector2.zero;

		switch (input.Mode) {
			case VJoystick.PointerMode.Touch:
			case VJoystick.PointerMode.Mouse:
				Vector2 cursorPoint = Camera.main.ScreenToWorldPoint(
					input.Mode == VJoystick.PointerMode.Touch
						? Input.GetTouch(0).position
						: (Vector2) Input.mousePosition
				);
				result = cursorPoint - playerPosition;
				break;

			case VJoystick.PointerMode.Controller:
				result.Set(Input.GetAxis("Pointer X"), Input.GetAxis("Pointer Y"));
				break;
		}

		return result.normalized;
	}

	public override void Update() {
		if (GameManager.IsPaused()) return;

		ForEachGameObject((ego, p, input, position) => {
			input.XMoveAxis = Input.GetAxis("Horizontal");
			input.XMoveAxisRaw = Input.GetAxisRaw("Horizontal");

			if (Input.GetButtonDown("Jump")) input.JumpPressed = true;
			if (Input.GetButtonDown("Sink")) input.SinkPressed = true;
			if (Input.GetKeyDown(KeyCode.R)) input.ShouldRespawn = true;


			bool touchDown = Input.touchCount > 0;
			bool mouseDown = Input.GetButton("Grapple To Point");
			bool controllerDown = Input.GetButton("Grapple Using Pointer");

			input.HookDown = touchDown || mouseDown || controllerDown;
			if (touchDown) input.Mode = VJoystick.PointerMode.Touch;
			else if (mouseDown) input.Mode = VJoystick.PointerMode.Mouse;
			else if (controllerDown) input.Mode = VJoystick.PointerMode.Controller;
			input.AimAxis = getPointerDir(input, position.Value);
		});
	}
}
