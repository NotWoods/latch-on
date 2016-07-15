using UnityEngine;

public static class RaycastOriginDebug {
	public static void DebugDraw(
		this RaycastOrigin rayOrigns,
		Vector2 origin, Vector2 distance,
		float duration = 0, bool depthTest = true
	) {
		Vector2 world = rayOrigns.transform.TransformPoint(origin);
		Debug.DrawRay(world, distance, Color.red, duration, depthTest);
	}
}