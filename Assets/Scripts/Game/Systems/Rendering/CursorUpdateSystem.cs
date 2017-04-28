using System.Collections.Generic;
using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Rope;
using Entity = EgoComponent;

namespace LatchOn.ECS.Systems.Rendering {
	public class CursorUpdateSystem : EgoSystem<
		LocalPlayer, WorldPosition, LineData, CanGrapple, VJoystick
	> {
		public float previewDistance = 2f;

		Dictionary<Entity, CursorData> cursors = new Dictionary<Entity, CursorData>();
		CursorData GetCursor(Entity trackedEntity) {
			if (cursors.ContainsKey(trackedEntity)) return cursors[trackedEntity];

			Entity cursorEntity = GameManager.Instance
				.NewEntity(UIManager.Instance.CursorPrefab);
			CursorData cursor = cursorEntity.GetComponent<CursorData>();
			cursors[trackedEntity] = cursor;

			var transform = cursor.GetComponent<RectTransform>();
			transform.SetParent(UIManager.Canvas);

			return cursor;
		}

		public override void Update() {
			ForEachGameObject((ego, p, position, line, grapple, input) => {
				bool shouldHighlight = Physics2D.Raycast(position.Value,
					input.AimAxis, grapple.StartingLength, grapple.ShouldGrapple);

				RaycastHit2D cursorCheck = Physics2D.Raycast(position.Value,
					input.AimAxis, previewDistance, grapple.Solids);
				Vector2 cursorPosition = cursorCheck
					? cursorCheck.point
					: position.Value + (input.AimAxis * previewDistance);

				var cursor = GetCursor(ego);
				var cursorTransform = cursor.GetComponent<RectTransform>();
				cursor.Highlighted = shouldHighlight;
				cursorTransform.position = cursorPosition;
			});
		}
	}
}
