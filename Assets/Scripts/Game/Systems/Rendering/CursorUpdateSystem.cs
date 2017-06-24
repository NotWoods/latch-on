using System.Collections.Generic;
using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Input;
using LatchOn.ECS.Components.Rope;
using Entity = EgoComponent;

namespace LatchOn.ECS.Systems.Rendering {
	public class CursorUpdateSystem : EgoSystem<
		EgoConstraint<LocalPlayer, WorldPosition, LineData, CanGrapple, VJoystick>
	> {
		public float previewDistance = 2f;

		Dictionary<Entity, CursorData> cursors = new Dictionary<Entity, CursorData>();
		CursorData GetCursor(Entity trackedEntity) {
			if (cursors.ContainsKey(trackedEntity)) return cursors[trackedEntity];

			Entity cursorEntity = GameManager.Instance
				.NewEntity(UIManager.Instance.CursorPrefab);
			CursorData cursor = cursorEntity.GetComponent<CursorData>();
			cursors[trackedEntity] = cursor;

			Ego.SetParent(UIManager.Canvas.GetComponent<EgoComponent>(), cursorEntity);

			return cursor;
		}

		public override void Start() {
			cursors.Clear();
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, p, position, line, grapple, input) => {
				bool shouldHighlight = false;
				RaycastHit2D hit = Physics2D.Raycast(
					position.Value, input.AimAxis,
					grapple.StartingLength, grapple.Solids
				);

				if (hit) {
					int layer = hit.transform.gameObject.layer;
					if (ExtraMath.InLayerMask(layer, grapple.ShouldGrapple)) {
						shouldHighlight = true;
					} else {
						shouldHighlight = false;
					}
				} else {
					shouldHighlight = false;
				}

				RaycastHit2D cursorCheck = Physics2D.Raycast(position.Value,
					input.AimAxis, previewDistance, grapple.Solids);
				Vector2 cursorPosition = cursorCheck
					? cursorCheck.point
					: position.Value + (input.AimAxis * previewDistance);

				var cursor = GetCursor(ego);
				var cursorTransform = cursor.GetComponent<RectTransform>();
				cursor.Highlighted = shouldHighlight;
				cursor.Hidden = !GameManager.IsActive(ego);
				cursorTransform.position = cursorPosition;
			});
		}
	}
}
