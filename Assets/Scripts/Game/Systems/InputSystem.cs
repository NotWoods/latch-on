using UnityEngine;

public class InputSystem : EgoSystem<PlayerMarker, InputData, Transform> {
	public override void Update() {
		bool isPaused = Time.timeScale == 0;
		if (isPaused) return;

		ForEachGameObject((ego, p, input, transform) => {
			input.HorizontalInput = Input.GetAxis("Horizontal");
			input.JumpPressed = Input.GetButtonDown("Jump");
			input.SinkPressed = Input.GetKeyDown(KeyCode.S);
			input.ShouldRespawn = Input.GetKeyDown(KeyCode.R);

			bool touchDown = Input.touchCount > 0;
			bool mouseDown = Input.GetButton("Grapple To Point");
			bool controllerDown = Input.GetButton("Grapple Using Pointer");

			input.HookDown = touchDown || mouseDown || controllerDown;
			if (touchDown) input.Mode = InputData.PointerMode.Touch;
			else if (mouseDown) input.Mode = InputData.PointerMode.Mouse;
			else if (controllerDown) input.Mode = InputData.PointerMode.Controller;

			switch (input.Mode) {
				case InputData.PointerMode.Touch:
				case InputData.PointerMode.Mouse:
					Vector2 cursorPoint = Camera.main.ScreenToWorldPoint(
						input.Mode == InputData.PointerMode.Touch
							? Input.GetTouch(0).position
							: (Vector2) Input.mousePosition
					);
					input.PointerDir = cursorPoint - (Vector2) transform.position;
					break;

				case InputData.PointerMode.Controller:
					input.PointerDir.Set(Input.GetAxis("Pointer X"), Input.GetAxis("Pointer Y"));
					break;
			}

			input.PointerDir.Normalize();
		});
	}
}
