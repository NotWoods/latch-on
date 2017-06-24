using UnityEngine;
using LatchOn.ECS.Components.Camera;

namespace LatchOn.ECS.Systems {
	public class CameraTargetingSystem : EgoSystem<
		EgoConstraint<CameraTarget, CameraFollow>
	> {
		public EgoConstraint<CameraTarget, CameraFollow> Constraint {
			get { return constraint; }
		}

		public override void Start() {
			constraint.ForEachGameObject((ego, targetData, follower) => {
				targetData.FocusBox = new Bounds(Vector3.up * 1.5f, targetData.FocusZone);
			});
		}

		public override void Update() {
			constraint.ForEachGameObject((ego, targetData, follower) => {
				targetData.FocusBox.size = targetData.FocusZone;

				if (targetData.TargetedEntity == null) {
					follower.Target = Vector2.zero;
					return;
				}

				Bounds target = targetData.TargetedEntity.bounds;
				Vector2 shift = Vector2.zero;

				if (target.min.x < targetData.FocusBox.min.x)
					shift.x = target.min.x - targetData.FocusBox.min.x;
				else if (target.max.x > targetData.FocusBox.max.x)
					shift.x = target.max.x - targetData.FocusBox.max.x;
				if (target.min.y < targetData.FocusBox.min.y)
					shift.y = target.min.y - targetData.FocusBox.min.y;
				else if (target.max.y > targetData.FocusBox.max.y)
					shift.y = target.max.y - targetData.FocusBox.max.y;
				targetData.FocusBox.center += (Vector3) shift;

				follower.Target = targetData.FocusBox.center;
			});
		}
	}
}
