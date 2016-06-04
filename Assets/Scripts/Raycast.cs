using UnityEngine;

///<summary>Helper functions for creating 2D Raycasts</summary>
public static class Raycast2D {
	public static RaycastHit2D towardsPoint(Vector2 fromPoint, Vector2 toPoint,
	 float maxDistance = Mathf.Infinity, 
	 int layerMask = Physics.DefaultRaycastLayers) {
		Vector2 direction = (toPoint - fromPoint);
		direction = direction.normalized;
		return Physics2D.Raycast(fromPoint, direction, maxDistance, layerMask);
	}
	
	public static RaycastHit2D towardsAngle(Vector2 fromPoint, float angle,
	 float maxDistance = Mathf.Infinity,
	 int layerMask = Physics.DefaultRaycastLayers) {
		Vector2 direction = new Vector2(
			Mathf.Cos(angle * Mathf.Deg2Rad), 
			Mathf.Sin(angle * Mathf.Deg2Rad));
		return Physics2D.Raycast(fromPoint, direction, maxDistance, layerMask);
	}
}