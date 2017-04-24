using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	/// Indicates an entity is affected by gravity, and can possibly jump
	[DisallowMultipleComponent]
	public class Descends : MonoBehaviour {
		[SerializeField]
		float _jumpHeight = 3f;
		[SerializeField]
		float _gravity = -25;
		[SerializeField]
		float _maxFallSpeed = 15;

		/// How many units the entity can jump.
		/// If 0, then the entity cannot jump
		public float JumpHeight { get { return _jumpHeight; } }
		/// Force of gravity. Negative numbers indicate gravity goes down.
		public float Gravity { get { return _gravity; } }
		/// Fall speed caps to this value.
		public float MaxFallSpeed { get { return _maxFallSpeed; } }
	}
}
