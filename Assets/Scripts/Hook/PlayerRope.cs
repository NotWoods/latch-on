using UnityEngine;
using System.Collections.Generic;
using System;

namespace Hook.Rope {
	public class PhysicsRope : IRope {	
		///<summary>The world position of the spring joint anchor</summary>
		public Vector2 anchor;
		///<summary>Where the hook connects with the wall</summary>
		public Vector2 connectedAnchor; 
		
		///<summary>Refers to the Spring Joint used for the rope</summary>
		///TODO: Figure out some way to support DistanceJoint2D as well
		private SpringJoint2D spring; 
		
		///<summary>Layers that the hook can collide with</summary>
		public int layerIsGrapplable; 
		///<summary>Layers that the rope should wrap around</summary>
		public int layerIsSolid; 
		
		PhysicsRope(SpringJoint2D _spring) {
			spring = _spring;
		}
		
		void ConnectTo(Vector2 location) {
			
		}
		
		void Break() {
			
		}
	}
}