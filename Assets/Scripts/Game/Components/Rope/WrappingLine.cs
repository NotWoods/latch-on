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
		}

		[SerializeField]
		List<Entry> _wrappedItems = new List<Entry>();

		public IList<Entry> WrappedItems {
			get { return _wrappedItems.AsReadOnly(); }
		}

		public void Push(Vector2 point, Side side) {
			Entry toAdd = new Entry();
			toAdd.point = point;
			toAdd.side = side;
			_wrappedItems.Add(toAdd);
		}

		public Entry Peek() { return _wrappedItems[_wrappedItems.Count - 1]; }
		public void Peek(out Vector2 point, out Side side) {
			Entry top = Peek();
			point = top.point;
			side = top.side;
		}

		public Entry Pop() {
			Entry top = Peek();
			_wrappedItems.RemoveAt(_wrappedItems.Count - 1);
			return top;
		}
		public void Pop(out Vector2 point, out Side side) {
			Peek(out point, out side);
			_wrappedItems.RemoveAt(_wrappedItems.Count - 1);
		}

		public void Clear() {
			_wrappedItems.Clear();
		}
	}
}
