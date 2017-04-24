using System;
using System.Collections.Generic;
using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// Wrapping line data, enhancing the normal LineData item
	[DisallowMultipleComponent]
	public class WrappingLine : MonoBehaviour {
		/// Layers that the rope should wrap around
		public LayerMask ShouldWrap;

		// The current free/unwrapped length of the line is stored
		// in the LineData#CurrentLength
		// Wrapped portions of the line are static. To fake the effect, that
		// section is left stationary and the unwrapped portion of the rope acts
		// like a normal rope.

		public List<Vector2> WrappedPoints = new List<Vector2>();
		public List<Side> MarkedSides = new List<Side>();
	}
}
