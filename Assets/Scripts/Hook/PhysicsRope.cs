using UnityEngine;

namespace Hook.Rope {
	public class PhysicsRope : IRope {	
		///<summary>The world position of the spring joint anchor</summary>
		public Vector2 anchor { get; set; }
		
		///<summary>Where the hook connects with the wall</summary>
		public Vector2 connectedAnchor { get; set; }
		
		public float maxRange;
		
		public bool isActive {
			get {return spring.enabled;}
		}
		
		///<summary>Refers to the Spring Joint used for the rope</summary>
		///TODO: Figure out some way to support DistanceJoint2D as well
		protected SpringJoint2D spring;
		
		///<summary>Layers that the hook can collide with</summary>
		public int layerIsGrapplable; 
		///<summary>Layers that the rope should wrap around</summary>
		public int layerIsSolid; 
		
		public PhysicsRope(SpringJoint2D _spring) {
			spring = _spring;
		}
		
		///<summary>Connect the rope to something</summary>
		public void ConnectTo(RaycastHit2D hit) {
			connectedAnchor = hit.point;
			spring.connectedBody = hit.rigidbody;
			if (hit.rigidbody != null) {
				spring.connectedAnchor = 
					hit.transform.InverseTransformPoint(connectedAnchor);
			} else {
				spring.connectedAnchor = connectedAnchor;
			}
			//spring.distance = distanceBetween(anchor, hit.point);
			spring.enabled = true;
		}
		
		///<summary>
		///Break the rope and reset anything that needs to be reset</summary>
		public void Break() {
			spring.enabled = false;
		}
		
		///<summary>
		///Retract rope at speed. Use negative speed to extend.
		///Retract at a constant rate.</summary>
		public float Retract(float speed = 1f) {
			spring.distance -= speed;
			return spring.distance;
		}
	}
}