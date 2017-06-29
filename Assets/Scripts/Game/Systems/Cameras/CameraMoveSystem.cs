using UnityEngine;
using LatchOn.ECS.Components.Camera;

namespace LatchOn.ECS.Systems.Cameras {
	/// Move the camera based on the camera follow parameters
	public class CameraMoveSystem : EgoSystem<
		EgoConstraint<CameraFollow, Transform>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, follower, transform) => {
				transform.position = Vector3.MoveTowards(
					transform.position,
					(Vector3) follower.Target + follower.Offset,
					10
				);
			});
		}
	}
}
