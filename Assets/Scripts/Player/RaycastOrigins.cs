using UnityEngine;
using System.Linq;

///Stores 2D Raycast origin points as arrays
public class RaycastOrigin {
	public readonly Vector2[] left;
	public readonly Vector2[] right;
	public readonly Vector2[] top;
	public readonly Vector2[] bottom;

	public Transform transform;
	readonly Bounds boundsCopy;

	static int minRays = 2; static int maxRays = 20;

	public RaycastOrigin(
		Bounds bounds, int horizontalRayCount = 4, int verticalRayCount = 4, 
		Transform transform = null, float skinWidth = 0
	) {
		boundsCopy = bounds; Bounds bnds;
		if (transform != null) {
			bnds = new Bounds(transform.InverseTransformPoint(bounds.center), 
				bounds.size);
		} else {
			bnds = bounds;
		}
		bnds.Expand(skinWidth * -1);

		Mathf.Clamp(horizontalRayCount, minRays, maxRays);
		Mathf.Clamp(verticalRayCount, minRays, maxRays);

		left = new Vector2[horizontalRayCount];
		for (int i = 0; i < horizontalRayCount; i++) {
			float fraction = (i / horizontalRayCount);
			float offset = (fraction * bnds.size.y);
			left[i] = new Vector2(bnds.min.x, bnds.min.y + offset);
		}

		bottom = new Vector2[verticalRayCount];
		for (int i = 0; i < verticalRayCount; i++) {
			float fraction = (i / verticalRayCount);
			float offset = (fraction * bnds.size.x);
			bottom[i] = new Vector2(bnds.min.x + offset, bnds.min.y);
		}

		right = left.Select(v => v + (bnds.size.x * Vector2.right)).ToArray();
		top = bottom.Select(v => v + (bnds.size.y * Vector2.up)).ToArray();
	}

	public RaycastOrigin Clone(int? xCount, int? yCount) {
		int _x = xCount ?? left.Length;
		int _y = yCount ?? bottom.Length;
		return new RaycastOrigin(boundsCopy, _x, _y);
	}

	static Vector2[] PickFromDir(
		float direction, 
		Vector2[] lessThan, Vector2[] greaterThan
	) {
		if (direction == 0) 
			throw new System.ArgumentException("Direction cannot be 0");
		
		if (direction < 0) return lessThan;
		else return greaterThan;
	} 

	static Vector2 Sign(Vector2 velocity) {
		return new Vector2(
			Mathf.Sign(velocity.x),
			Mathf.Sign(velocity.y)
		);
	}

	public Vector2[] Get(float velocity, Vector2 basic) {
		if (basic == Vector2.up) {
		 	Debug.Log(PickFromDir(velocity, bottom, top));
			return PickFromDir(velocity, bottom, top);
		} else if (basic == Vector2.right) {
			return PickFromDir(velocity, left, right);
		} else {
			throw new System.ArgumentException(
				"Must be either Vector2.up or Vector2.right");
		}
	}

	public RaycastHit2D CastRelative(
		Vector2 origin, Vector2 distance, 
		int layerMask = Physics2D.DefaultRaycastLayers
	) {
		return Physics2D.Raycast(
			transform.TransformPoint(origin),
			distance.normalized,
			distance.magnitude,
			layerMask
		);
	}
}