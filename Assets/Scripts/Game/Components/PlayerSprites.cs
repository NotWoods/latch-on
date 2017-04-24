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

// TODO
namespace LatchOn.ECS.Components.Input {}
namespace LatchOn.ECS.Components.Mover {}
namespace LatchOn.ECS.Components.Rope {}
