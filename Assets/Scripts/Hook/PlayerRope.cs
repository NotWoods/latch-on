using UnityEngine;
using System.Collections.Generic;
using System;

namespace Hook {
	public class PlayerRope : IWrappable {	
		///<summary>The world position of the spring joint anchor</summary>
		public Vector2 anchor;
		///<summary>Where the hook connects with the wall</summary>
		public Vector2 connectedAnchor;
		/// <summary>List of joints in the rope</summary>
		public JointList joints; 
		
		/// <summary>Refers to the Spring Joint used for the rope</summary>
		private SpringJoint2D spring; 
		
		///<summary>Length of the rope's wrapped part</summary>
		private float wrapDistance = 0;
		///<summary>Length of the rope's flexible part</summary>
		private float flexDistance { 
			get {
				return spring.distance;
			}
			set {
				spring.distance = value;
			}
		}
		
		///<summary>Layers that the hook can collide with</summary>
		public int layerIsGrapplable; 
		///<summary>Layers that the rope should wrap around</summary>
		public int layerIsSolid; 
		
		void ConnectTo(Vector2 location) {
			
		}
		
		void Break() {
			
		}
		
		void WrapOn(RaycastHit2D hit) {
			
		}
		
		void Unwrap() {
			
		}
	}
}