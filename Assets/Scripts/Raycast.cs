using UnityEngine;

///<summary>Helper functions for creating 2D Raycasts</summary>
public class Raycast2D {
	static RaycastHit2D towardsPoint(Vector2 fromPoint, Vector2 toPoint,
	 float maxDistance = Mathf.infinity, 
	 int layerMask = Physics.DefaultRaycastLayers) {
		Vector2 direction = (toPoint - fromPoint);
		direction = direction.normalized;
		return Physics2D.Raycast(fromPoint, direction, distance, layerMask);
	}
	
	static RaycastHit2D towardsAngle(Vector2 fromPoint, float angle,
	 float maxDistance = Mathf.infinity,
	 int layerMask = Physics.DefaultRaycastLayers) {
		Vector2 direction = new Vector2(
			Mathf.Cos(degree * Mathf.Degree2Rad), 
			Mathf.Sin(degree * Mathf.Degree2Rad));
		return Physics2D.Raycast(fromPoint, direction, distance, layerMask);
	}
}