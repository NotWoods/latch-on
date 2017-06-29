using UnityEngine;
using LatchOn.ECS.Components.Base;
using LatchOn.ECS.Components.Rope;
using LatchOn.ECS.Components.Input;

namespace LatchOn.ECS.Systems.Swinging {
	/// Enable to manually adjust the rope length with an input axis
	public class AdjustRetractSystem : EgoSystem<
		EgoConstraint<LineData, VJoystick>
	> {
		public override void FixedUpdate() {
			constraint.ForEachGameObject((ego, line, input) => {
				line.RetractSpeed = line.BaseRetractSpeed * input.RopeAdjustAxis;
			});
		}
	}
}
