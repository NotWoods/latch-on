using UnityEngine;
using LatchOn.ECS.Components.Camera;

namespace LatchOn.ECS.Systems {
	public class CameraZoomSystem : EgoSystem<
		EgoConstraint<CameraFollow, ZoomMode>
	> {
		public float Standard = 13;
		public float ZoomedIn = 4;
		public float ZoomedOut = 20;

		public override void Update() {
			constraint.ForEachGameObject((ego, follower, zoom) => {
				float newZoom;
				switch (zoom.Value) {
					case Zoom.ZoomedIn:
						newZoom = ZoomedIn;
						break;
					case Zoom.Standard: default:
						newZoom = Standard;
						break;
					case Zoom.ZoomedOut:
						newZoom = ZoomedOut;
						break;
				}

				Vector3 newOffset = follower.Offset;
				newOffset.z = Mathf.MoveTowards(
					newOffset.z,
					newZoom * -1,
					Time.deltaTime * 30
				);
				follower.Offset = newOffset;
			});
		}
	}
}
