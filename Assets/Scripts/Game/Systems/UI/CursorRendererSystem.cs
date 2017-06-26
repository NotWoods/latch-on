using UnityEngine;
using UnityEngine.UI;
using LatchOn.ECS.Components;

namespace LatchOn.ECS.Systems.UI {
	public class CursorRendererSystem : EgoSystem<
		EgoConstraint<CursorData, Image, RectTransform>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, data, image, transform) => {
				float t = Time.deltaTime * 10;
				bool highlighted = data.Highlighted;

				image.enabled = !data.Hidden;

				image.color = Color.Lerp(image.color,
					highlighted ? data.HighlightColor : data.DarkColor, t);

				transform.localScale = Vector2.Lerp(transform.localScale,
					Vector2.one * (highlighted ? data.HighlightScale : data.DarkScale), t);
			});
		}
	}
}
