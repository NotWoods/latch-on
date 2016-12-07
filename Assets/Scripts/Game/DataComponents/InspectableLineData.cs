using System;
using System.Collections.Generic;
using UnityEngine;

/// LineData, but easier to look at inspector.
[DisallowMultipleComponent]
public class InspectableLineData : LineData {
	[SerializeField]
	private List<Vector2> points = new List<Vector2>();

	public new void WrapPoint(Vector2 pos) {
		points.Add(pos);
	}

	public new void UnwrapLast() {
		if (points.Count > 0) points.RemoveAt(points.Count - 1);
		else throw new InvalidOperationException("The LineComponent is empty");
	}

	public new Vector2 GetLast() {
		return points[points.Count - 1];
	}

	public new Vector2 GetPenultimate() {
		return points[points.Count - 2];
	}

	public new bool IsAnchored() {
		return points.Count > 0;
	}

	/// Remove all points from the line
	public new void ClearPoints() {
		points.Clear();
	}

	public new IEnumerable<Vector2> Points() {
		foreach (Vector2 p in points) yield return p;
	}
}
