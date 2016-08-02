using UnityEngine;

namespace Util {
	public static class Circle {
		public static Vector2 PointOnCircumference(
			float radianAngle, float radius, Vector2 origin
		) {
			float x = Mathf.Cos(radianAngle);
			float y = Mathf.Sin(radianAngle);

			//x *= Mathf.Rad2Deg; 
			//y *= Mathf.Rad2Deg;
			return origin + new Vector2(x, y) * radius;
		}

		public static Vector2 PointOnCircumference(float radianAngle, float radius = 1) {
			return PointOnCircumference(radianAngle, radius, Vector2.zero);
		}
	}
}