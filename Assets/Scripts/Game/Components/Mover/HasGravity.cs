using UnityEngine;

namespace LatchOn.ECS.Components.Mover {
	[DisallowMultipleComponent]
	public class HasGravity : MonoBehaviour {
		[SerializeField]
		float _gravity = -25;
		[SerializeField]
		float _maxFallSpeed = 15;

		/// Force of gravity. Negative numbers indicate gravity goes down.
		public float Gravity { get { return _gravity; } }
		/// Fall speed caps to this value.
		public float MaxFallSpeed { get { return _maxFallSpeed; } }
	}
}
