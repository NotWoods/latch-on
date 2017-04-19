using UnityEngine;

namespace LatchOn.ECS.Components {
	[DisallowMultipleComponent]
	public class PlayerSprites : MonoBehaviour {
		public Transform Body;
		public Transform Head;
		public Transform LeftHand;
		public Transform RightHand;
	}
}
