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

		[Serializable]
		public struct Entry {
			public Vector2 point;
			public Side side;

			public Entry(Vector2 point, Side side) {
				this.point = point;
				this.side = side;
			}
		}

		public List<Entry> Entries = new List<Entry>();

		public int Count { get { return Entries.Count; } }

		public void Push(Vector2 point, Side side) {
			Entries.Add(new Entry(point, side));
		}
		public void Push(Vector2 point) {
			Push(point, SideOfLine(point));
		}

		public Entry Peek() {
			return Entries[Entries.Count - 1];
		}
		public Entry Pop() {
			Entry removed = Entries[Entries.Count - 1];
			Entries.RemoveAt(Entries.Count - 1);
			return removed;
		}

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
