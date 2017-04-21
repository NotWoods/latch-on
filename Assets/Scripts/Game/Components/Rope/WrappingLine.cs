using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// Wrapping line data, enhancing the normal LineData item
	[DisallowMultipleComponent]
	public class WrappingLine : MonoBehaviour, IEnumerable<WrappingLine.Entry> {
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

		[SerializeField]
		List<Entry> entries = new List<Entry>();

		public IEnumerator<Entry> GetEnumerator() { return entries.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public int Count { get { return entries.Count; } }

		public void Push(Vector2 point, Side side) {
			entries.Add(new Entry(point, side));
		}
		public void Push(Vector2 point, Vector2 playerPosition) {
			Push(point, Side.None);
			Entry justPushed = Peek();
			justPushed.side = SideOfLine(playerPosition);
		}

		public Entry Peek() {
			return entries[entries.Count - 1];
		}
		public Entry Pop() {
			Entry removed = entries[entries.Count - 1];
			entries.RemoveAt(entries.Count - 1);
			return removed;
		}

		public void Clear() { entries.Clear(); }

		public Side SideOfLine(Vector2 point) {
			int count = entries.Count;
			if (count < 2) throw new InvalidOperationException();

			// The line checked agaisnt is represented by these two points
			Vector2 currentAnchor = entries[count - 1].point;
			Vector2 topWrappedPoint = entries[count - 2].point;

			return (Side) ExtraMath.SideOfLine(point, currentAnchor, topWrappedPoint);
		}
	}
}
