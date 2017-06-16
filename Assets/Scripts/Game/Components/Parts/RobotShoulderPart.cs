using UnityEngine;

namespace LatchOn.ECS.Components.Parts {
	[DisallowMultipleComponent]
	public class RobotShoulderPart : MonoBehaviour {
		public Transform Part;

		public Quaternion NormalRotation;
		public Vector3 ReferenceAngle = Vector3.left;
		public Vector3 Addition = new Vector3(90, 90, 0);
	}
}
