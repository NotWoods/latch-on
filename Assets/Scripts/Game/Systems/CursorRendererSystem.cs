using UnityEngine;
using UnityEngine.UI;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Rope;

namespace LatchOn.ECS.Systems {
	public class CursorRendererSystem : EgoSystem<
		LocalPlayer, WorldPosition, LineData, CanGrapple, VJoystick
	> {
		public float PreviewDistance = 2f;
		public float HighlightScale = 1;
		public float DarkScale = 0.5f;
		public Color HighlightColor = Color.white;
		public Color DarkColor = Color.gray;

		public override void LateUpdate() {
			int i = 0;
			ForEachGameObject((o, p, position, line, grapple, input) => {
				bool shouldHighlight = Physics2D.Raycast(position.Value,
					input.AimAxis, grapple.StartingLength, grapple.ShouldGrapple);

				RaycastHit2D cursorCheck = Physics2D.Raycast(position.Value,
					input.AimAxis, PreviewDistance, grapple.Solids);
				Vector2 cursorPosition = cursorCheck
					? cursorCheck.point
					: position.Value + (input.AimAxis * PreviewDistance);

				Image cursor = UIManager.Instance.GetCursor(i);
				cursor.color = Color.Lerp(
					cursor.color,
					shouldHighlight ? HighlightColor : DarkColor,
					Time.deltaTime * 10
				);

				RectTransform cursorTransform = cursor.rectTransform;
				cursorTransform.position = cursorPosition;
				cursorTransform.localScale = Vector2.Lerp(
					cursorTransform.localScale,
					Vector2.one * (shouldHighlight ? HighlightScale : DarkScale),
					Time.deltaTime * 10
				);

				i++;
			});
		}
	}
}
