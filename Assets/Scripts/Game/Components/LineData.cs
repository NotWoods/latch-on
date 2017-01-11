using System;
using System.Collections.Generic;
using UnityEngine;

/// LineData, but easier to look at inspector.
[DisallowMultipleComponent]
public class LineData : MonoBehaviour {
	public float StartingLength = 10f;
	public float FreeLength = 10f;

	public float RetractSpeed = 1f;

	/// Layers which can be grappled
	public LayerMask NormalGround;
	/// Layers which cannot be grappled but will still impact the rope
	public LayerMask NoHookGround;

	internal Stack<int> MarkedSides = new Stack<int>();

	[SerializeField]
	private List<Vector2> points = new List<Vector2>();

	public void WrapPoint(Vector2 pos) {
		points.Add(pos);
	}

	public void UnwrapLast() {
		if (points.Count > 0) {
			int count = points.Count;
			FreeLength += Vector2.Distance(points[count - 1], points[count - 2]);
			points.RemoveAt(points.Count - 1);
		}
		else throw new InvalidOperationException("The LineComponent is empty");
	}

	public Vector2 GetLast() {
		return points[points.Count - 1];
	}

	public Vector2 GetPenultimate() {
		return points[points.Count - 2];
	}

	public bool IsAnchored() {
		return points.Count > 0;
	}

	public void SetAnchor(Vector2 pos) {
		if (IsAnchored()) throw new InvalidOperationException();
		WrapPoint(pos);
	}

	/// Remove all points from the line
	public void ClearPoints() {
		points.Clear();
	}

	public IEnumerable<Vector2> Points() {
		foreach (Vector2 p in points) yield return p;
	}

	public int Count {
		get { return points.Count; }
	}

	public int Side(Vector2 point) {
		if (Count < 2) throw new InvalidOperationException();
		return ExtraMath.SideOfLine(point, GetLast(), GetPenultimate());
	}
}
