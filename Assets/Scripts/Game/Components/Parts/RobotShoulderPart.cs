using UnityEngine;

namespace LatchOn.ECS.Components.Parts {
	[DisallowMultipleComponent]
	public class RobotShoulderPart : MonoBehaviour {
		public Transform LeftArm;
		public Transform RightArm;
	}
}
