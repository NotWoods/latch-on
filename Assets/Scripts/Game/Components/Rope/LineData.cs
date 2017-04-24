using System;
using System.Collections.Generic;
using UnityEngine;

namespace LatchOn.ECS.Components.Rope {
	/// Data for the line that connects the entity and some point
	[DisallowMultipleComponent]
	public class LineData : MonoBehaviour {
		[SerializeField]
		float _minLength = 0.5f;
		[SerializeField]
		float _retractSpeed = 2;

		/// Smallest valid length of the rope
		public float MinLength { get { return _minLength; } }
		/// The current length of the line.
		public float CurrentLength = 10f;
		/// How quickly the line will retract.
		public float RetractSpeed { get { return _retractSpeed; } }
		/// True if the rope is anchored to a point
		public bool IsAnchored = false;
		/// Point where the rope is anchored. Should be ignored if !IsAnchored
		public Vector2 AnchorPoint {
			get { return points[points.Count - 1]; }
			set { Push(value); }
		}

		// TODO: Move to wrap component
		public LayerMask NoHookGround;

		internal Stack<int> MarkedSides = new Stack<int>();

		[SerializeField]
		private List<Vector2> points = new List<Vector2>();

		public void Push(Vector2 pos) {
			points.Add(pos);
		}

		public void Pop() {
			if (points.Count > 0) {
				int count = points.Count;
				CurrentLength += Vector2.Distance(points[count - 1], points[count - 2]);
				points.RemoveAt(points.Count - 1);
			}
			else throw new InvalidOperationException("The LineComponent is empty");
		}

		public Vector2 Peek() { return points[points.Count - 2]; }

		public bool Anchored() { return points.Count > 0; }

		/// Remove all points from the line
		public void Clear() { points.Clear(); }

		public IEnumerable<Vector2> Points() {
			foreach (Vector2 p in points) yield return p;
		}

		public int Count { get { return points.Count; } }

		public int Side(Vector2 point) {
			if (Count < 2) throw new InvalidOperationException();
			return ExtraMath.SideOfLine(point, AnchorPoint, Peek());
		}
	}
}
