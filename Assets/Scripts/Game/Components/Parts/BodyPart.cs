using UnityEngine;

namespace LatchOn.ECS.Components.Parts {
	[DisallowMultipleComponent]
	public class BodyPart : MonoBehaviour {
		public Transform Part;

		public bool ShouldFlip = false;
	}
}
