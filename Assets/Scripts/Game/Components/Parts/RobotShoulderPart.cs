using UnityEngine;
using System.Collections.Generic;

namespace LatchOn.ECS.Components.Parts {
	[DisallowMultipleComponent]
	public class RobotShoulderPart : MonoBehaviour {
		public Transform LeftArm;
		public Transform RightArm;

		public GameObject ArmPrefab;
		public List<Stretchy> ArmExtenders = new List<Stretchy>();
	}
}
