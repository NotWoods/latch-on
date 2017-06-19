using UnityEngine;

namespace LatchOn.ECS.Components.Parts {
	[DisallowMultipleComponent]
	public class RobotExtenderPart : MonoBehaviour {
		public Transform Part;

		public float DefaultScale = 0.18f;
	}
}
