using UnityEngine;

public class InputSystem : EgoSystem<InputData, Transform> {
	public override void Update() {
		ForEachGameObject((ego, input, transform) => {
			input.HorizontalInput = Input.GetAxis("Horizontal");
			input.JumpPressed = Input.GetButtonDown("Jump");
			// input.SinkPressed = Input.GetButtonDown("Sink");
			input.ShouldRespawn = Input.GetKeyDown(KeyCode.R);


			bool mouseDown = Input.GetButton("Grapple To Point");
			bool controllerDown = Input.GetButton("Grapple Using Pointer");

			input.HookDown = mouseDown || controllerDown;
			if (mouseDown) input.Mode = InputData.PointerMode.Mouse;
			else if (controllerDown) input.Mode = InputData.PointerMode.Controller;

			switch (input.Mode) {
				case InputData.PointerMode.Controller:
					input.PointerDir.Set(Input.GetAxis("Pointer X"), Input.GetAxis("Pointer Y"));
					break;

				case InputData.PointerMode.Mouse:
					Vector2 cursorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					input.PointerDir = cursorPoint - (Vector2) transform.position;
					break;
			}

			input.PointerDir.Normalize();
		});
	}
}
