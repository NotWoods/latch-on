using UnityEngine;

public class InputSystem : EgoSystem<InputData, Transform> {
	public override void Update() {
		ForEachGameObject((ego, input, transform) => {
			input.HorizontalInput = Input.GetAxis("Horizontal");
			input.JumpPressed = Input.GetButtonDown("Jump");
			// input.SinkPressed = Input.GetButtonDown("Sink");

			/*input.HookDown = Input.GetButton("Grapple Using Pointer");
			input.PointerDir.Set(
				Input.GetAxis("Pointer X"),
				Input.GetAxis("Pointer Y")
			);*/

			input.HookDown = Input.GetButton("Grapple To Point");
			if (input.HookDown) {
				Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				input.PointerDir = clickPoint - (Vector2) transform.position;
				input.PointerDir.Normalize();
			}
		});
	}
}
