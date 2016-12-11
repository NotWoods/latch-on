using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CursorRendererSystem : EgoSystem<LocalPlayer, Transform, InspectableLineData, InputData> {
	public float PreviewDistance = 2f;
	public float HighlightScale = 1;
	public float DarkScale = 0.5f;
	public Color HighlightColor = Color.white;
	public Color DarkColor = Color.gray;

	private List<Image> cursors = new List<Image>();

	public override void Update() {
		int i = 0;
		ForEachGameObject((o, p, transform, line, input) => {
			bool shouldHighlight = Physics2D.Raycast(transform.position,
				input.PointerDir, line.StartingLength, line.NormalGround);

			RaycastHit2D cursorCheck = Physics2D.Raycast(transform.position,
				input.PointerDir, PreviewDistance, line.NormalGround);
			Vector2 position = cursorCheck
				? cursorCheck.point
				: (Vector2) transform.position + (input.PointerDir * PreviewDistance);

			Image cursor = cursors[i];
			cursor.color = Color.Lerp(
				cursor.color,
				shouldHighlight ? HighlightColor : DarkColor,
				Time.deltaTime * 10
			);

			i++;
		});
	}
}
