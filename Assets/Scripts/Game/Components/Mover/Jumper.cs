using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	[DisallowMultipleComponent]
	public class Jumper : MonoBehaviour {
		[SerializeField]
		float _jumpHeight = 3f;

		public float JumpHeight { get { return _jumpHeight; } }
	}
}
