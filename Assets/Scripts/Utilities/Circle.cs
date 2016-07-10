using UnityEngine;

public static class Circle {
	public static Vector2 PointOnCircumference(
		float radius, float rad, Vector2 origin
	) {
		float x = origin.x + radius * Mathf.Cos(rad);
		float y = origin.y + radius * Mathf.Sin(rad);

		x *= Mathf.Rad2Deg; 
		y *= Mathf.Rad2Deg;
		return new Vector2(x, y);
	}
}