using UnityEngine;

namespace Hook.Rope {
	public class PhysicsWrappable : PhysicsRope, IWrappable {	
		/// <summary>List of joints in the rope</summary>
		public JointList joints {get; set;}
		
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
		
		public PhysicsWrappable(SpringJoint2D _spring) : base(_spring) {
			
		}
		
		public void WrapOn(RaycastHit2D hit) {
			
		}
		
		public void Unwrap() {
			
		}
	}
}