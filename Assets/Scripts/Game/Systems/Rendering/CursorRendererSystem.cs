using UnityEngine;
using UnityEngine.UI;
using LatchOn.ECS.Components;

namespace LatchOn.ECS.Systems {
	public class CursorRendererSystem : EgoSystem<
		CursorData, Image, RectTransform
	> {
		public void UpdateCursor(EgoComponent cursor, bool highlighted, Vector2 position) {
			var bundle = _bundles[cursor];
			CursorData data = bundle.component1;
			Image image = bundle.component2;
			RectTransform transform = bundle.component3;

			float t = Time.deltaTime * 10;

			image.color = Color.Lerp(image.color,
				highlighted ? data.HighlightColor : data.DarkColor, t);

			transform.position = position;
			transform.localScale = Vector2.Lerp(transform.localScale,
				Vector2.one * (highlighted ? data.HighlightScale : data.DarkScale), t);
		}
	}
}
