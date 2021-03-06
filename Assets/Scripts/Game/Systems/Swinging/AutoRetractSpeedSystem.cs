using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems.Swinging {
	/// Enable to automatically retract the rope
	public class AutoRetractSpeedSystem : EgoSystem<
		EgoConstraint<LineData, Velocity, VJoystick>
	> {
		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, line, velocity, input) => {
				if (input.LockRopeDown) {
					line.RetractSpeed = 0;
				} else if (ExtraMath.InRange(velocity.x, -1, 1)
				&& velocity.y > (line.RetractSpeed - 0.5)) {
					line.RetractSpeed = line.BaseRetractSpeed + 2;
				} else {
					line.RetractSpeed = line.BaseRetractSpeed;
				}
			});
		}
	}
}
