using UnityEngine;

namespace Hook {
	///<summary>
	///Standard code for a rope wrapper. Enhances a rope passed in
	///the constructor.
	///</summary>
	public class Wrapper {			
		static int layerIsGrapplable; //Layers that the hook can collide with
		static int layerIsSolid; //Layers that the rope should wrap around
		
		private IWrappable rope;
		Wrapper(IWrappable _rope) {
			rope = _rope;
		}
		
		///<summary>
		///Wrapper for Raycast that takes points instead of direction and distance.
		///</summary>
		///<param name="fromPoint">Where the Raycast starts</param>
		///<param name="toPoint">Where the Raycast aims for</param>
		///<returns>A RaycastHit2D from the raycast</returns>
		static RaycastHit2D CheckCollision(Vector2 fromPoint, Vector2 toPoint) {
			Vector2 heading = toPoint - fromPoint;
			float distance = heading.magnitude;
			Vector2 direction = heading / distance;
			
			return Physics2D.Raycast(fromPoint, direction, distance, layerIsSolid);
		}
		
		///<summary>
		///Checks for rope collisions and
		///wraps the rope around an object if its is in the way
		///</summary>
		private void WrapCheck() {
			RaycastHit2D hit = CheckCollision(rope.anchor, rope.joints.Peek());
			if (hit.collider != null) {
				rope.OnWrap(hit);
			}
		}
		
		///<summary>
		///Checks if the rope is in a position to unwrap itself then unwraps
		///if possible
		///</summary>
		private void UnwrapCheck() {
			if (rope.joints.Count > 0) {
				RaycastHit2D hit = CheckCollision(rope.anchor, rope.joints.PeekNext());
				if (hit.collider == null) {
					rope.OnUnwrap();
				}
			}
		}
		
		private public void OnFixedUpdate() {
			WrapCheck();
			UnwrapCheck();
		}
	}
}