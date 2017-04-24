using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	/// Stores configuration values for an entity
	[DisallowMultipleComponent]
	public class MoveConfig : MonoBehaviour {
		public float JumpHeight = 3f;

		public float Gravity = -25f;

		public float MaxFallSpeed = 15;
	}
}
