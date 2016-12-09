using UnityEngine;

public class CursorRendererSystem : EgoSystem<Transform, InspectableLineData, InputData, CursorAlias> {
	public float PreviewDistance = 2f;

	public override void Update() {
		ForEachGameObject((ego, transform, line, input, cursor) => {
			RaycastHit2D cursorCheck = Physics2D.Raycast(transform.position, input.PointerDir, PreviewDistance, line.NormalGround);
			RaycastHit2D hitCheck = Physics2D.Raycast(transform.position, input.PointerDir, line.StartingLength, line.NormalGround);

			cursor.Highlighted = hitCheck ? true : false;

			cursor.Position = cursorCheck
				? cursorCheck.point
				: (Vector2) transform.position + (input.PointerDir * PreviewDistance);
		});
	}
}