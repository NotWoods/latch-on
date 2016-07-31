using UnityEngine;

namespace Player {
	///Contains shared fields that can be accessed by 
	///classes extending BaseController 
	[DisallowMultipleComponent]
	public class BaseControllerFields : MonoBehaviour {
		public LayerMask platformMask = 0;
		public Vector2 centerOfMass = Vector2.zero;
	}
}