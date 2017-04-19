using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	[DisallowMultipleComponent]
	public class CanGrapple : MonoBehaviour {
		[SerializeField]
		float _startingLength = 10f;

		/// Initial length of the rope (usually used for raycast limits)
		public float StartingLength { get { return _startingLength; } }
		/// Solid objects that will block the grapple. If they are also in
		/// `ShouldGrapple`, then the grapple will attach to them.
		public LayerMask Solids;
		/// Layers that can be hit by the grapple.
		public LayerMask ShouldGrapple;
	}
}
