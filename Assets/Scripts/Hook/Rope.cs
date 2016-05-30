using UnityEngine;
using System.Collections.Generic;

namespace Hook {
	public class Rope : MonoBehaviour {
		public SpringJoint2D spring;
		public List<Vector2> joints = new List<Vector2>();
		private Vector2 lastPoint {
			get {return joints[joints.Count - 1];}
		}
		private Vector2 prevPoint {
			get {
				if (joints.Count < 2) return joints[0];
				else return joints[joints.Count - 2];
			}
		}
		
		public Vector2 anchor;
		public Vector2 connectedAnchor {
			get {return joints[0];}
			set {joints[0] = value;}
		}
		
		[HideInInspector]
		public float wrapDistance = 0;
		[HideInInspector]
		public float flexDistance {
			get {
				return spring.distance;
			}
			set {
				spring.distance = value;
			}
		}
		
		private Rigidbody2D hook;
		
		static int layerIsGrapplable;
		static int layerIsSolid;
		
		static RaycastHit2D CheckCollision(Vector2 fromPoint, Vector2 toPoint) {
			Vector2 heading = toPoint - fromPoint;
			float distance = heading.magnitude;
			Vector2 direction = heading / distance;
			
			return Physics2D.Raycast(fromPoint, direction, distance, layerIsSolid);
		}
		
		void AddJoint(Vector2 newPoint) {
			float distance = Vector2.Distance(newPoint, lastPoint);
			
			wrapDistance += distance;
			flexDistance -= distance;
			
			hook.transform.position = newPoint;
			
			joints.Add(newPoint);
		}
		
		void RemoveJoint() {
			float distance = Vector2.Distance(lastPoint, prevPoint);
			
			wrapDistance -= distance;
			flexDistance += distance;
			
			hook.transform.position = prevPoint;
			
			joints.RemoveAt(joints.Count - 1);
		}
		
		void Wrap() {
			RaycastHit2D hit = CheckCollision(anchor, lastPoint);
			if (hit.collider != null) {
				AddJoint(hit.point);
			}
		}
		
		void Unwrap() {
			RaycastHit2D hit = CheckCollision(anchor, prevPoint);
			if (hit.collider == null) {
				RemoveJoint();
			}
		}
		
		void FixedUpdate() {
			Unwrap();
		}
	}
}