using System.Collections.Generic;
using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Rope;
using Entity = EgoComponent;

namespace LatchOn.ECS.Systems.Rendering {
	public class CursorUpdateSystem : EgoSystem<
		LocalPlayer, WorldPosition, LineData, CanGrapple, VJoystick
	> {
		public CursorUpdateSystem(CursorRendererSystem renderSystem) {
			this.renderSystem = renderSystem;
		}

		public float previewDistance = 2f;

		CursorRendererSystem renderSystem;
		Dictionary<Entity, Entity> cursors = new Dictionary<Entity, Entity>();
		Tuple<Entity, bool> GetCursor(Entity trackedEntity) {
			if (cursors.ContainsKey(trackedEntity))
				return new Tuple<Entity, bool>(cursors[trackedEntity], true);

			Entity cursor = GameManager.Instance
				.NewEntity(UIManager.Instance.CursorPrefab);
			cursors[trackedEntity] = cursor;

			var transform = cursor.GetComponent<RectTransform>();
			transform.SetParent(UIManager.Canvas);

			return new Tuple<Entity, bool>(cursor, false);
		}

		public override void LateUpdate() {
			ForEachGameObject((ego, p, position, line, grapple, input) => {
				bool shouldHighlight = Physics2D.Raycast(position.Value,
					input.AimAxis, grapple.StartingLength, grapple.ShouldGrapple);

				RaycastHit2D cursorCheck = Physics2D.Raycast(position.Value,
					input.AimAxis, previewDistance, grapple.Solids);
				Vector2 cursorPosition = cursorCheck
					? cursorCheck.point
					: position.Value + (input.AimAxis * previewDistance);

				var cursor = GetCursor(ego);
				if (cursor.second)
					renderSystem.UpdateCursor(cursor.first, shouldHighlight, cursorPosition);
			});
		}
	}
}
