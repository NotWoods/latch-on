using UnityEngine;
using System.Collections;
using Hook.Rope;

namespace Hook {
	///<summary>This class will attach to the player gameObject</summary>
	public class PlayerGrapple : MonoBehaviour {
		public float maxRange = 20f;
		
		private PhysicsRope rope;
		
		///TODO: Code to grapple onto an object
		///TODO: Code to call Hook.Rope functions in Game Loops
		
		public void ConnectTowards(Vector2 point) {
			RaycastHit2D hit = Raycast2D.towardsPoint(transform.position, point, 
				maxRange, rope.layerIsGrapplable);
			if (hit.collider != null) {
				rope.ConnectTo(hit);
			}
		}
		
		public void ConnectTowards(float angle) {
			RaycastHit2D hit = Raycast2D.towardsAngle(transform.position, angle, 
				maxRange, rope.layerIsGrapplable);
			if (hit.collider != null) {
				rope.ConnectTo(hit);
			}
		}
		
		void Awake() {
			
		}
		
		void FixedUpdate() {
			
		}
	}
}