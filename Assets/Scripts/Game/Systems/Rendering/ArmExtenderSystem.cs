using UnityEngine;
using LatchOn.ECS.Components;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Components.Parts;

namespace LatchOn.ECS.Systems.Rendering {
	public class ArmExtenderSystem : EgoSystem<Transform, LineData, WrappingLine, RobotShoulderPart> {
		public override void Update() {
			ForEachGameObject((ego, transform, line, wrap, arm) => {
				if (!line.IsAnchored) {
					foreach (Stretchy segment in arm.ArmExtenders) {
						segment.IsStretching = false;
					}
					return;
				}

				var points = LineRendererSystem.BuildPoints(transform.position, line, wrap);
				if (points.Length > arm.ArmExtenders.Count) {
					EgoComponent entity = GameManager.Instance.NewEntity(arm.ArmPrefab);
					arm.ArmExtenders.Add(entity.GetComponent<Stretchy>());
					entity.transform.parent = transform;
					entity.transform.localPosition = new Vector3(0.05f, -0.13f, 0);
				}

				int i;
				for (i = 0; i < points.Length - 1; i++) {
					arm.ArmExtenders[i].IsStretching = true;
					arm.ArmExtenders[i].StartPoint = points[i+1];
					arm.ArmExtenders[i].EndPoint = points[i];
				}
				for (; i < arm.ArmExtenders.Count; i++) {
					arm.ArmExtenders[i].IsStretching = false;
				}
			});
		}
	}
}
