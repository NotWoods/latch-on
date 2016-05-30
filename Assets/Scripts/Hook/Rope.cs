using UnityEngine;
using System.Collections.Generic;

namespace Hook {
	public class Rope {
		/// <summary>Refers to the Spring Joint used for the rope</summary>
		public SpringJoint2D spring; 
		
		/// <summary>List of joints in the rope</summary>
		/// <remarks>The 0th joint is the connectedAnchor of the rope</remark>
		public List<Vector2> joints = new List<Vector2>();
		/// <summary>Point that the SpringJoint current connected to</summary>
		/// <remarks>AKA a point with nothing between it and the player</remarks>
		private Vector2 lastPoint { 
			get {return joints[joints.Count - 1];}
		}
		///<summary>The point right before the last point, used for unwrap</summary>
		///<exception>Throws when there are 0 joints (aka straight rope)</exception>
		private Vector2 prevPoint {
			get {
				if (joints.Count <= 1) {
					throw new InvalidOperationException(
						"Rope is straight, wait until there is some bend in the rope");
				}	else {return joints[joints.Count - 2];}
			}
		}
		
		///<summary>The world position of the spring joint anchor</summary>
		public Vector2 anchor;
		///<summary>Where the hook connects with the wall</summary>
		public Vector2 connectedAnchor {
			get {return joints[0];}
			set {joints[0] = value;}
		}
		
		[HideInInspector]
		public float wrapDistance = 0; //Length of the rope's wrapped part
		[HideInInspector]
		public float flexDistance { //Length of the rope's flexible part
			get {
				return spring.distance;
			}
			set {
				spring.distance = value;
			}
		}
		
		/// <summary>
		/// Reference to the hook object that the spring connects to</summary>
		private Rigidbody2D hook;
		
		static int layerIsGrapplable; //Layers that the hook can collide with
		static int layerIsSolid; //Layers that the rope should wrap around
		
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
		
		///<summary>Adds a joint to the rope</summary>
		///<param name="newPoint">Where the new joint is located</param>
		void AddJoint(Vector2 newPoint) {
			float distance = Vector2.Distance(newPoint, lastPoint);
			
			wrapDistance += distance;
			flexDistance -= distance;
			
			hook.transform.position = newPoint;
			
			joints.Add(newPoint);
		}
		
		///<summary>Removes a joint from the rope</summary>
		void RemoveJoint() {
			float distance = Vector2.Distance(lastPoint, prevPoint);
			
			wrapDistance -= distance;
			flexDistance += distance;
			
			hook.transform.position = prevPoint;
			
			joints.RemoveAt(joints.Count - 1);
		}
		
		///<summary>
		///Checks for rope collisions and
		///wraps the rope around an object if its is in the way
		///</summary>
		void Wrap() {
			RaycastHit2D hit = CheckCollision(anchor, lastPoint);
			if (hit.collider != null) {
				AddJoint(hit.point);
			}
		}
		
		///<summary>
		///Checks if the rope is in a position to unwrap itself then unwraps
		///if possible
		///</summary>
		void Unwrap() {
			try {
				RaycastHit2D hit = CheckCollision(anchor, prevPoint);
				if (hit.collider == null) {
					RemoveJoint();
				}
			} catch (InvalidOperationException e) {
				//rope is currently straight, try again next time to see if it
				//has a bend.
			}
		}
		
		///<summary>
		///Draws the rope sprite along the given line</summary>
		void Draw() {
			
		}
	}
}