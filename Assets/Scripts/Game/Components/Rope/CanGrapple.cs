using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	[DisallowMultipleComponent]
	public class CanGrapple : MonoBehaviour {
		[SerializeField]
		float _startingLength = 20f;

		/// Initial length of the rope (usually used for raycast limits)
		public float StartingLength { get { return _startingLength; } }
		/// Solid objects that will block the grapple. If they are also in
		/// `ShouldGrapple`, then the grapple will attach to them.
		public LayerMask Solids;
		/// Layers that can be hit by the grapple.
		public LayerMask ShouldGrapple;

		/// Hook object entity reference
		public EgoComponent Hook;

		public bool DidThrow = false;

		[ContextMenu("Reset Line Length")]
		void ResetLineLength() {
			var line = GetComponent<LineData>();
			if (line == null) {
				Debug.LogWarning("Missing LineData component, can't reset line length");
			} else {
				line.CurrentLength = _startingLength;
			}
		}
	}
}
