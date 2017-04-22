using System.Collections.Generic;
using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Rope;
using Entity = EgoComponent;

namespace LatchOn.ECS.Systems {
	public class CursorUpdateSystem : EgoSystem<
		LocalPlayer, WorldPosition, LineData, CanGrapple, VJoystick
	> {
		CursorRendererSystem renderSystem;

		public float previewDistance = 2f;
		public GameObject cursorPrefab;

		public CursorUpdateSystem(CursorRendererSystem renderSystem) {
			this.renderSystem = renderSystem;
		}

		Dictionary<Entity, Entity> cursors = new Dictionary<Entity, Entity>();
		Entity GetCursor(Entity trackedEntity) {
			if (cursors.ContainsKey(trackedEntity)) return cursors[trackedEntity];

			Entity cursor = GameManager.Instance.NewEntity(cursorPrefab);
			cursors[trackedEntity] = cursor;

			var transform = cursor.GetComponent<RectTransform>();
			transform.SetParent(UIManager.Canvas);

			return cursor;
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

				Entity cursor = GetCursor(ego);
				renderSystem.UpdateCursor(cursor, shouldHighlight, cursorPosition);
			});
		}
	}
}
