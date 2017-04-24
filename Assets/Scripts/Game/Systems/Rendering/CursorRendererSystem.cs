using UnityEngine;
using UnityEngine.UI;
using LatchOn.ECS.Components;

namespace LatchOn.ECS.Systems.Rendering {
	public class CursorRendererSystem : EgoSystem<CursorData, Image, RectTransform> {
		public void UpdateCursor(EgoComponent cursor, bool highlighted, Vector2 pos) {
			ExtractComponents(cursor, (ego, data, image, transform) => {
				float t = Time.deltaTime * 10;

				image.color = Color.Lerp(image.color,
					highlighted ? data.HighlightColor : data.DarkColor, t);

				transform.position = pos;
				transform.localScale = Vector2.Lerp(transform.localScale,
					Vector2.one * (highlighted ? data.HighlightScale : data.DarkScale), t);
			});
		}
	}
}
