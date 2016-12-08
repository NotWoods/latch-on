using UnityEngine;

public class CursorRendererSystem : EgoSystem<Transform, InspectableLineData, InputData> {
	public float PreviewDistance = 2f;

	public override void Update() {
		ForEachGameObject((ego, transform, line, input) => {
			RaycastHit2D cursorCheck = Physics2D.Raycast(transform.position, input.PointerDir, PreviewDistance, line.NormalGround);
			RaycastHit2D hitCheck = Physics2D.Raycast(transform.position, input.PointerDir, line.StartingLength, line.NormalGround);

			Vector2 cursorPos = cursorCheck
				? cursorCheck.point
				: (Vector2) transform.position + (input.PointerDir * PreviewDistance);

			// TODO: use non-gizmos
			//if (!hitCheck) Gizmos.color = Color.gray;
			//Gizmos.DrawWireSphere(cursorPos, 0.1f);
		});
	}
}
