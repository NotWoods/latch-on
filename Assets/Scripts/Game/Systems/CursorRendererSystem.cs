using UnityEngine;
using UnityEngine.UI;

public class CursorRendererSystem : EgoSystem<LocalPlayer, Transform, LineData, VJoystick> {
	public float PreviewDistance = 2f;
	public float HighlightScale = 1;
	public float DarkScale = 0.5f;
	public Color HighlightColor = Color.white;
	public Color DarkColor = Color.gray;

	public override void LateUpdate() {
		int i = 0;
		ForEachGameObject((o, p, transform, line, input) => {
			bool shouldHighlight = Physics2D.Raycast(transform.position,
				input.AimAxis, line.StartingLength, line.NormalGround);

			RaycastHit2D cursorCheck = Physics2D.Raycast(transform.position,
				input.AimAxis, PreviewDistance, line.NormalGround);
			Vector2 position = cursorCheck
				? cursorCheck.point
				: (Vector2) transform.position + (input.AimAxis * PreviewDistance);

			Image cursor = UIManager.Instance.GetCursor(i);
			cursor.color = Color.Lerp(
				cursor.color,
				shouldHighlight ? HighlightColor : DarkColor,
				Time.deltaTime * 10
			);

			RectTransform cursorTransform = cursor.rectTransform;
			cursorTransform.position = position;
			cursorTransform.localScale = Vector2.Lerp(
				cursorTransform.localScale,
				Vector2.one * (shouldHighlight ? HighlightScale : DarkScale),
				Time.deltaTime * 10
			);

			i++;
		});
	}
}
