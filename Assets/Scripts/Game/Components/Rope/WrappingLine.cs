using System;
using System.Collections.Generic;
using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// Wrapping line data, enhancing the normal LineData item
	[DisallowMultipleComponent]
	public class WrappingLine : MonoBehaviour {
		[Serializable]
		public struct Entry {
			public Vector2 point;
			public Side side;

			public Entry(Vector2 point, Side side) {
				this.point = point;
				this.side = side;
			}
		}

		[SerializeField]
		List<Entry> Entries = new List<Entry>();

		// The current free/unwrapped length of the line is stored
		// in the LineData#CurrentLength
		// Wrapped portions of the line are static. To fake the effect, that
		// section is left stationary and the unwrapped portion of the rope acts
		// like a normal rope.

		/// Layers that the rope should wrap around
		public LayerMask ShouldWrap;

		public Side SideOfLine(Vector2 point) {
			int count = Entries.Count;
			if (count < 2) throw new InvalidOperationException();

			// The line checked agaisnt is represented by these two points
			Vector2 currentAnchor = Entries[count - 1].point;
			Vector2 topWrappedPoint = Entries[count - 2].point;

			return (Side) ExtraMath.SideOfLine(point, currentAnchor, topWrappedPoint);
		}
	}
}
