using System;
using System.Collections.Generic;
using UnityEngine;

/// Used to store data representing a Rope/Line.
[DisallowMultipleComponent]
public class LineData : MonoBehaviour, IDataComponent {
	public float StartingLength = 10f;
	public float FreeLength = 10f;

	public float RetractSpeed = 1f;

	/// Layers which can be grappled
	public LayerMask NormalGround;
	/// Layers which cannot be grappled but will still impact the rope
	public LayerMask NoHookGround;

	public LayerMask CollideOnlyGround {
		// TODO get normal ground minus no hook ground
		get { return (LayerMask) NormalGround.value & NoHookGround.value; }
	}

	// Internally, a stack is used for most points but a seperate variable
	// represents the very top of the stack. This is done so that the
	// 2nd-to-top point in the line can be easily returned.
	private Stack<Vector2> points = new Stack<Vector2>();
	private Vector2? lastPoint = null;

	public void WrapPoint(Vector2 pos) {
		if (lastPoint.HasValue) points.Push(lastPoint.Value);
		lastPoint = pos;
	}

	public void UnwrapLast() {
		if (points.Count > 0) lastPoint = points.Pop();
		else if (lastPoint.HasValue) lastPoint = null;
		else throw new InvalidOperationException("The LineComponent is empty");
	}

	/// Last static point in the line.
	/// Throws InvalidOperationException if there are no points
	public Vector2 GetLast() {
		return lastPoint.Value;
	}

	/// 2nd to last static point in the line.
	public Vector2 GetPenultimate() {
		return points.Peek();
	}

	/// returns true if the Line is attached to anything.
	public bool IsAnchored() {
		return lastPoint.HasValue;
	}

	/// An alias for WrapPoint that throws if there are points in the line
	public void SetAnchor(Vector2 pos) {
		if (IsAnchored()) throw new InvalidOperationException();
		WrapPoint(pos);
	}

	/// Remove all points from the line
	public void ClearPoints() {
		lastPoint = null;
		points.Clear();
	}

	public IEnumerable<Vector2> Points() {
		foreach (Vector2 p in points) yield return p;
		if (lastPoint.HasValue) yield return lastPoint.Value;
	}

	public int Count {
		get { return points.Count + (lastPoint.HasValue ? 1 : 0); }
	}
}
