using UnityEngine;

public class InputSystem : EgoSystem<PlayerMarker, InputData, Transform> {
	private Vector2 getPointerDir(InputData input, Vector2 playerPosition) {
		Vector2 result = Vector2.zero;

		switch (input.Mode) {
			case InputData.PointerMode.Touch:
			case InputData.PointerMode.Mouse:
				Vector2 cursorPoint = Camera.main.ScreenToWorldPoint(
					input.Mode == InputData.PointerMode.Touch
						? Input.GetTouch(0).position
						: (Vector2) Input.mousePosition
				);
				result = cursorPoint - playerPosition;
				break;

			case InputData.PointerMode.Controller:
				result.Set(Input.GetAxis("Pointer X"), Input.GetAxis("Pointer Y"));
				break;
		}

		return result.normalized;
	}

	public override void Update() {
		if (GameManager.IsPaused()) return;

		ForEachGameObject((ego, p, input, transform) => {
			input.HorizontalInput = Input.GetAxis("Horizontal");
			input.HorizontalInputRaw = Input.GetAxisRaw("Horizontal");

			if (Input.GetButtonDown("Jump")) input.JumpPressed = true;
			if (Input.GetKeyDown(KeyCode.S)) input.SinkPressed = true;
			if (Input.GetKeyDown(KeyCode.R)) input.ShouldRespawn = true;


			bool touchDown = Input.touchCount > 0;
			bool mouseDown = Input.GetButton("Grapple To Point");
			bool controllerDown = Input.GetButton("Grapple Using Pointer");

			input.HookDown = touchDown || mouseDown || controllerDown;
			if (touchDown) input.Mode = InputData.PointerMode.Touch;
			else if (mouseDown) input.Mode = InputData.PointerMode.Mouse;
			else if (controllerDown) input.Mode = InputData.PointerMode.Controller;
			input.PointerDir = getPointerDir(input, transform.position);
		});
	}
}
