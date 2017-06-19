using UnityEngine;

public static class ExtraMath {
	/// Like Mathf.Sign, but treats 0 seperate from positive numbers.
	public static int Sign(float number) {
		if (number < 0) return -1;
		else if (number > 0) return 1;
		else return 0;
	}

	/// <summary>
	/// Returns the side of the line a point lies on. Result will be -1, 0, or 1.
	/// Uses determinants to compute.
	/// </summary>
	/// <see cref="http://stackoverflow.com/questions/1560492/" />
	/// <param name="point">Point to check</param>
	/// <param name="A">Start point of the line</param>
	/// <param name="B">End point of the line</param>
	public static int SideOfLine(Vector2 point, Vector2 A, Vector2 B) {
		return Sign((B.x - A.x) * (point.y - A.y) - (B.y - A.y) * (point.x - A.x));
	}

	/// <summary>
	/// Checks if a number is between two values (inclusive).
	/// </summary>
	/// <param name="toCheck">Number to check</param>
	/// <param name="min">Smallest valid value</param>
	/// <param name="max">Largest valid value</param>
	public static bool InRange(float toCheck, float min, float max) {
		return toCheck >= min && toCheck <= max;
	}

	public static bool InLayerMask(int layer, LayerMask mask) {
		return mask == (mask | (1 << layer));
	}
}
