using UnityEngine;
using LatchOn.ECS.Components.Camera;

namespace LatchOn.ECS.Systems.Cameras {
	public class CameraInputSystem : EgoSystem<
		EgoConstraint<ZoomMode>
	> {
		public override void Update() {
			constraint.ForEachGameObject((ego, zoom) => {
				if (Input.GetButton("Zoom Out")) zoom.Value = Zoom.ZoomedOut;
				else if (Input.GetKey(KeyCode.LeftControl)) zoom.Value = Zoom.ZoomedIn;
				else zoom.Value = Zoom.Standard;
			});
		}
	}
}
